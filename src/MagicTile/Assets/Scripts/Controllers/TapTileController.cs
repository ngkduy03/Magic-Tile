using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TapTileController : TileAbstractController
{
    private CancellationTokenSource fadeCTS = new();
    public TapTileController(
        RectTransform tileRectTransform,
        RectTransform endLineRectTransform,
        Image image,
        IScoreService scoreService,
        float speed,
        ref bool isGameOver)
    {
        this.tileRectTransform = tileRectTransform;
        this.laneRectTransform = endLineRectTransform;
        this.image = image;
        this.scoreService = scoreService;
        this.speed = speed;
        this.isGameOver = isGameOver;
    }

    public override async UniTask FadeTile()
    {
        if (image != null && !isPressed)
        {
            isPressed = true;
            // Create sequence for animations
            Sequence sequence = DOTween.Sequence();

            // First phase: scale up to 1.3 and change color to white
            sequence.Append(tileRectTransform.DOScale(Vector3.one * 1.15f, 0.2f));
            sequence.Join(image.DOColor(Color.white, 0.2f));

            // Second phase: scale down to 0 and fade out
            sequence.Append(tileRectTransform.DOScale(0f, 0.7f));
            sequence.Join(image.DOFade(0f, 0.5f));

            MoveCTS?.Cancel();
            MoveCTS?.Dispose();
            await sequence.Play().WithCancellation(fadeCTS.Token);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }

    public override void DestroyTile()
    {
    }
}
