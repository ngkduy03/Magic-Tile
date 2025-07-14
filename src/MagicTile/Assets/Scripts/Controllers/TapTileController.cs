using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for handling the tap tile interactions in the game.
/// </summary>
public class TapTileController : TileAbstractController
{
    public TapTileController(
        RectTransform tileRectTransform,
        RectTransform endLineRectTransform,
        Image image,
        IScoreService scoreService,
        IEventBusService eventBusService,
        float speed) : base()
    {
        this.tileRectTransform = tileRectTransform;
        this.laneRectTransform = endLineRectTransform;
        this.image = image;
        this.scoreService = scoreService;
        this.eventBusService = eventBusService;
        this.speed = speed;
    }

    public override async UniTask FadeTile()
    {
        if (image != null)
        {
            moveCTS.Cancel();
            moveCTS.Dispose();

            var sequence = DOTween.Sequence();

            sequence.Append(tileRectTransform.DOScale(Vector3.one * ScaleUpSize, FadeInDuration));
            sequence.Join(image.DOColor(Color.white, FadeInDuration));

            sequence.Append(tileRectTransform.DOScale(0f, FadeOutDuration));
            sequence.Join(image.DOFade(0f, FadeOutDuration));

            await sequence.Play().WithCancellation(fadeCTS.Token);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }

    public override void DestroyTile()
    {
    }
}
