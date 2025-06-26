using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TapTileController : TileAbstractController
{
    public TapTileController(
        RectTransform tileRectTransform,
        RectTransform laneRectTransform,
        Image image,
        float speed)
    {
        this.tileRectTransform = tileRectTransform;
        this.laneRectTransform = laneRectTransform;
        this.image = image;
        this.speed = speed;
    }
    public override async UniTask FadeTile(CancellationToken cancellationToken)
    {
        if (image != null && !IsPressed)
        {
            IsPressed = true;
            // Create sequence for animations
            Sequence sequence = DOTween.Sequence();

            // First phase: scale up to 1.3 and change color to white
            sequence.Append(tileRectTransform.DOScale(Vector3.one * 1.15f, 0.2f));
            sequence.Join(image.DOColor(Color.white, 0.2f));

            // Second phase: scale down to 0 and fade out
            sequence.Append(tileRectTransform.DOScale(0f, 0.7f));
            sequence.Join(image.DOFade(0f, 0.5f));

            await sequence.Play().WithCancellation(cancellationToken);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }

    public override void DestroyTile()
    {
    }
}
