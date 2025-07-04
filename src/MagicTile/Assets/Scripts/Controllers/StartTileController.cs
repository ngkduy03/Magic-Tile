using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartTileController : TileAbstractController
{
    private List<TileAbstractController> tileControllers;
    public StartTileController(
        RectTransform tileRectTransform,
        Image image,
        List<TileAbstractController> tileControllers) : base()
    {
        this.tileRectTransform = tileRectTransform;
        this.image = image;
        this.tileControllers = tileControllers;
    }

    // Start is called before the first frame update
    public override void DestroyTile()
    {
        throw new System.NotImplementedException();
    }

    public override UniTask MoveTileDown()
    {
        return UniTask.CompletedTask;
    }

    public override async UniTask FadeTile()
    {
        if (image != null && !isPressed)
        {
            isPressed = true;
            // Create sequence for animations
            Sequence sequence = DOTween.Sequence();

            // First phase: scale up to 1.3 and change color to white
            sequence.Append(tileRectTransform.DOScale(Vector3.one * ScaleUpSize, FadeInDuration));
            sequence.Join(image.DOColor(Color.white, FadeInDuration));

            // Second phase: scale down to 0 and fade out
            sequence.Append(tileRectTransform.DOScale(0f, FadeOutDuration));
            sequence.Join(image.DOFade(0f, FadeOutDuration));

            await sequence.Play().WithCancellation(fadeCTS.Token);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }

    public void ActivateTiles()
    {
        foreach (var tileController in tileControllers)
        {
            tileController.MoveTileDown().Forget();
        }
    }
}
