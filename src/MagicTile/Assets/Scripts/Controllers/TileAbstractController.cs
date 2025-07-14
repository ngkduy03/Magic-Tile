using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstract controller class for handling tile interactions in the game.
/// </summary>
public abstract class TileAbstractController : ControllerBase
{
    protected RectTransform tileRectTransform;
    protected RectTransform laneRectTransform;
    protected Image image;
    protected float speed;
    public bool IsPressed { get; set; }
    private const float perfectGradeEpsilon = 70f;
    protected IScoreService scoreService;
    protected IEventBusService eventBusService;
    protected CancellationTokenSource fadeCTS = new();
    protected CancellationTokenSource moveCTS = new();
    protected static CancellationTokenSource MoveGlobalCTS = new();
    protected const float FadeInDuration = 0.2f;
    protected const float ScaleUpSize = 1.15f;
    protected const float FadeOutDuration = 0.4f;

    protected TileAbstractController()
    {
        fadeCTS = new();
        MoveGlobalCTS = CancellationTokenSource.CreateLinkedTokenSource(fadeCTS.Token);
    }

    public abstract void DestroyTile();

    public abstract UniTask FadeTile();

    public virtual void OnTileScored()
    {
        ScoreGradeEnum grade = PerfectScoreCheck() ? ScoreGradeEnum.Perfect :
                             HasTileSizeReachedLanePivot() ? ScoreGradeEnum.Great :
                             ScoreGradeEnum.Cool;

        scoreService.ScorePoint(grade);
        var pointTxt = scoreService.TotalPoint.ToString();
        var gradeTxt = grade.ToString();
        var comboTxt = scoreService.ComboPoint.ToString();
        eventBusService.TriggerEvent(new ScorePointParam(pointTxt, gradeTxt, comboTxt));
        Debug.Log($"Score Grade: {grade}");
    }

    public virtual async UniTask MoveTileDown()
    {
        var tileYPivot = tileRectTransform.anchoredPosition.y;
        var laneYPivot = laneRectTransform.anchoredPosition.y;
        float distance = Mathf.Abs(tileYPivot - laneYPivot);
        float duration = distance / speed;

        using var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(MoveGlobalCTS.Token, moveCTS.Token);
        await tileRectTransform.DOAnchorPosY(laneYPivot, duration)
            .SetEase(Ease.Linear)
            .OnComplete(async () =>
            {
                if (IsPressed == true)
                {
                    await FadeTile();
                }
                else
                {
                    MoveGlobalCTS?.Cancel();
                    MoveGlobalCTS?.Dispose();
                    eventBusService.TriggerEvent(new GameOverParam());
                }
            })
            .WithCancellation(linkedCTS.Token);
    }

    private bool HasTileSizeReachedLanePivot()
    {
        var tileBottom = tileRectTransform.anchoredPosition.y - (tileRectTransform.sizeDelta.y * tileRectTransform.pivot.y);
        var lanePivotY = laneRectTransform.anchoredPosition.y;
        return tileBottom <= lanePivotY - perfectGradeEpsilon;
    }

    private bool PerfectScoreCheck()
    {
        var tileBottom = tileRectTransform.anchoredPosition.y - (tileRectTransform.sizeDelta.y * tileRectTransform.pivot.y);
        var lanePivotY = laneRectTransform.anchoredPosition.y;
        return FloatEquals(tileBottom, lanePivotY, perfectGradeEpsilon);
    }

    private bool FloatEquals(float value, float other, float epsilon)
    {
        return Mathf.Abs(value - other) <= epsilon;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            fadeCTS?.Cancel();
            fadeCTS?.Dispose();
            fadeCTS = null;

            moveCTS?.Cancel();
            moveCTS?.Dispose();
            moveCTS = null;

            MoveGlobalCTS?.Cancel();
            MoveGlobalCTS?.Dispose();
            MoveGlobalCTS = null;

            speed = 0f;
        }
    }
}
