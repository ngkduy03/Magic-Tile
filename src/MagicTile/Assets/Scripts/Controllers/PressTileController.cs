using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PressTileController : TileAbstractController
{
    private readonly RectTransform pressedRectTransform;
    private Image pressedImage;
    private float pressHeight;
    private CancellationTokenSource fadeCTS = new();

    public PressTileController(
        RectTransform tileRectTransform,
        RectTransform pressedRectTransform,
        RectTransform laneRectTransform,
        Image image,
        IScoreService scoreService,
        float speed,
        ref bool isGameOver)
    {
        this.tileRectTransform = tileRectTransform;
        this.pressedRectTransform = pressedRectTransform;
        this.laneRectTransform = laneRectTransform;
        this.image = image;
        this.scoreService = scoreService;
        this.speed = speed;
        this.isGameOver = isGameOver;
        pressedImage = pressedRectTransform.GetComponent<Image>();
    }

    public float PressProgress()
    {
        // increase the height of the pressedRectTransform, clamping it to the height of the tileRectTransform
        pressHeight = Mathf.Clamp(pressedRectTransform.sizeDelta.y + speed * 2 * Time.deltaTime, 0, tileRectTransform.sizeDelta.y);
        pressedRectTransform.sizeDelta = new Vector2(pressedRectTransform.sizeDelta.x, pressHeight);
        return pressHeight;
    }

    public override void DestroyTile()
    {
    }

    public override async UniTask FadeTile()
    {
        if (image != null && !isPressed)
        {
            isPressed = true;
            if (pressHeight >= tileRectTransform.sizeDelta.y)
            {
                scoreService.ScorePoint((int)ScoreGradeEnum.Cool);
            }
            // Create sequence for animations
            Sequence sequence = DOTween.Sequence();

            // First phase: scale up to 1.3 and change color to white
            pressedImage.material = null;
            sequence.Append(tileRectTransform.DOScale(Vector3.one * 1.15f, 0.2f));
            sequence.Join(image.DOColor(Color.white, 0.2f));
            sequence.Join(pressedImage.DOColor(Color.white, 0.2f));

            // Second phase: scale down to 0 and fade out
            sequence.Append(tileRectTransform.DOScale(0f, 0.5f));
            sequence.Join(image.DOFade(0f, 0.5f));
            sequence.Join(pressedImage.DOFade(0f, 0.5f));

            MoveCTS?.Cancel();
            MoveCTS?.Dispose();
            await sequence.Play().WithCancellation(fadeCTS.Token);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }
}
