using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// Controller for the game canvas.
/// </summary>
public class GameCanvasController : ControllerBase
{
    // Scale reference resolution for the canvas scaler.
    private readonly CanvasScaler canvasScaler;

    // Images for background and deadline decoration.
    private readonly Image backgroundImage;
    private readonly Image deadLineImage;
    private Color backgroundColor;
    private Color deadlineColor;

    // UI of game progress.
    private readonly Slider progressSlider;
    private readonly TMP_Text scoreText;
    private readonly TMP_Text gradeText;
    private AudioSource bgAudioSource;
    private CancellationTokenSource scoreCTS = new();
    private CancellationTokenSource gradeCTS = new();
    private CancellationTokenSource progressCTS = new();
    private Sequence fadeLoopSequence;
    private IEventBusService eventBusService;

    public GameCanvasController(
        CanvasScaler canvasScaler,
        Image backgroundImage,
        Image deadLineImage,
        Slider progressSlider,
        TMP_Text scoreText,
        TMP_Text gradeText,
        AudioSource bgAudioSource,
        IEventBusService eventBusService)
    {
        this.canvasScaler = canvasScaler;
        this.backgroundImage = backgroundImage;
        this.deadLineImage = deadLineImage;
        this.progressSlider = progressSlider;
        this.scoreText = scoreText;
        this.gradeText = gradeText;
        this.bgAudioSource = bgAudioSource;
        this.eventBusService = eventBusService;

        backgroundColor = backgroundImage.color;
        deadlineColor = deadLineImage.color;
        scoreText.text = "0";
        gradeText.text = string.Empty;
        eventBusService.RegisterListener<ScorePointParam>(OnPointScore);
        eventBusService.RegisterListener<StartGameParam>(OnGameStart);
        eventBusService.RegisterListener<GameOverParam>(OnGameOver);
    }

    /// <summary>
    /// Sets the reference resolution for the canvas scaler.
    /// </summary>
    public void SetReferenceResolution()
    {
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);
        canvasScaler.matchWidthOrHeight = 0f;
    }

    public void FadeLoopDecoration()
    {
        fadeLoopSequence = DOTween.Sequence();

        Tween bgFadeOut = backgroundImage.DOFade(0.3f, 0.3f);
        Tween dlFadeOut = deadLineImage.DOFade(0.3f, 0.3f);
        fadeLoopSequence.Append(bgFadeOut);
        fadeLoopSequence.Join(dlFadeOut);

        Tween bgFadeIn = backgroundImage.DOFade(backgroundColor.a, 0.5f);
        Tween dlFadeIn = deadLineImage.DOFade(deadlineColor.a, 0.5f);
        fadeLoopSequence.Append(bgFadeIn);
        fadeLoopSequence.Join(dlFadeIn);

        fadeLoopSequence.AppendInterval(0.5f);

        // Set the sequence to loop indefinitely
        fadeLoopSequence.SetLoops(-1, LoopType.Restart);
    }

    private void OnPointScore(ScorePointParam param)
    {
        ScoreEffect(param).Forget();
    }

    private void OnGameStart(StartGameParam param)
    {
        scoreText.text = "0";
        gradeText.text = string.Empty;
        scoreCTS?.Cancel();
        scoreCTS?.Dispose();
        scoreCTS = new CancellationTokenSource();
        FadeLoopDecoration();
        PlayMusicAndSlider();
    }

    private void OnGameOver(GameOverParam param)
    {
        scoreCTS?.Cancel();
        scoreCTS?.Dispose();
        scoreCTS = null;

        gradeCTS?.Cancel();
        gradeCTS?.Dispose();
        gradeCTS = null;

        progressCTS?.Cancel();
        progressCTS?.Dispose();
        progressCTS = null;

        bgAudioSource?.Stop();

        gradeText.text = string.Empty;

        fadeLoopSequence.Kill();
        fadeLoopSequence = null;
        backgroundImage.color = backgroundColor;
        deadLineImage.color = deadlineColor;
    }

    private async UniTask ScoreEffect(ScorePointParam param)
    {
        scoreCTS?.Cancel();
        scoreCTS?.Dispose();
        scoreCTS = new();
        scoreText.text = param.ScoreText;
        var scoreSequence = DOTween.Sequence();
        var gradeSequence = DOTween.Sequence();
        // Add the score text animation
        scoreSequence.Append(scoreText.transform.DOScale(0.5f, 0f));
        scoreSequence.Append(scoreText.transform.DOScale(1f, 0.2f));
        await scoreSequence.Play().WithCancellation(scoreCTS.Token);

        if (param.GradeText != ScoreGradeEnum.None.ToString())
        {
            gradeCTS?.Cancel();
            gradeCTS?.Dispose();
            gradeCTS = new();
            // Add the grade text animations
            gradeText.text = param.GradeText;
            gradeSequence.Append(gradeText.DOFade(1.0f, 0f));
            gradeSequence.Join(gradeText.transform.DOScale(0.5f, 0f));
            gradeSequence.Append(gradeText.transform.DOScale(1f, 0.2f));
            gradeSequence.Append(gradeText.DOFade(0.0f, 1f));
            await gradeSequence.Play().WithCancellation(gradeCTS.Token);
        }
    }

    private void PlayMusicAndSlider()
    {
        progressCTS?.Cancel();
        progressCTS?.Dispose();
        progressCTS = new();

        bgAudioSource?.Play();
        var bgDuration = bgAudioSource?.clip.length;
        progressSlider.value = 0f;
        progressSlider.DOValue(1f, bgDuration.Value)
            .SetEase(Ease.Linear)
            .WithCancellation(progressCTS.Token);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            scoreCTS?.Cancel();
            scoreCTS?.Dispose();
            scoreCTS = null;

            gradeCTS?.Cancel();
            gradeCTS?.Dispose();
            gradeCTS = null;

            progressCTS?.Cancel();
            progressCTS?.Dispose();
            progressCTS = null;

            eventBusService.UnregisterListener<ScorePointParam>(OnPointScore);
            eventBusService.UnregisterListener<StartGameParam>(OnGameStart);
            eventBusService.UnregisterListener<GameOverParam>(OnGameOver);

        }
    }
}
