using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PressTileController : TileAbstractController
{
    Material materialInstance;
    private readonly RectTransform pressedRectTransform;
    private Image pressedImage;

    public PressTileController(
        RectTransform tileRectTransform,
        RectTransform pressedRectTransform,
        RectTransform laneRectTransform,
        Image image,
        float speed)
    {
        this.tileRectTransform = tileRectTransform;
        this.pressedRectTransform = pressedRectTransform;
        this.laneRectTransform = laneRectTransform;
        this.image = image;
        this.speed = speed;
        pressedImage = pressedRectTransform.GetComponent<Image>();
    }

    public float PressProgress()
    {
        // increase the height of the pressedRectTransform, clamping it to the height of the tileRectTransform
        float newHeight = Mathf.Clamp(pressedRectTransform.sizeDelta.y + speed * 2 * Time.deltaTime, 0, tileRectTransform.sizeDelta.y);
        pressedRectTransform.sizeDelta = new Vector2(pressedRectTransform.sizeDelta.x, newHeight);
        return newHeight;
    }

    public override void DestroyTile()
    {
    }

    public override async UniTask FadeTile(CancellationToken cancellationToken)
    {
        if (image != null && !IsPressed)
        {
            IsPressed = true;
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

            await sequence.Play().WithCancellation(cancellationToken);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }
}
