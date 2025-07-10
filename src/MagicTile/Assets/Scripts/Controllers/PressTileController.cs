using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PressTileController : TileAbstractController
{
    private readonly RectTransform pressedRectTransform;
    private Image pressedImage;
    private float pressHeight;
    private bool isFaded = false;

    public PressTileController(
        RectTransform tileRectTransform,
        RectTransform pressedRectTransform,
        RectTransform laneRectTransform,
        Image image,
        IScoreService scoreService,
        IEventBusService eventBusService,
        float speed) : base()
    {
        this.tileRectTransform = tileRectTransform;
        this.pressedRectTransform = pressedRectTransform;
        this.laneRectTransform = laneRectTransform;
        this.image = image;
        this.scoreService = scoreService;
        this.eventBusService = eventBusService;
        this.speed = speed;
        pressedImage = pressedRectTransform.GetComponent<Image>();
    }

    public float PressProgress()
    {
        // Increase the height of the pressedRectTransform, clamping it to the height of the tileRectTransform
        pressHeight = Mathf.Clamp(pressedRectTransform.sizeDelta.y + speed * 2 * Time.deltaTime, 0, tileRectTransform.sizeDelta.y);
        pressedRectTransform.sizeDelta = new Vector2(pressedRectTransform.sizeDelta.x, pressHeight);
        return pressHeight;
    }

    public override void DestroyTile()
    {
    }

    public override async UniTask FadeTile()
    {
        if (image != null && !isFaded)
        {
            isFaded = true;
            moveCTS?.Cancel();
            moveCTS?.Dispose();

            if (pressHeight >= tileRectTransform.sizeDelta.y)
            {
                scoreService.ScorePoint(ScoreGradeEnum.EndPress);
                eventBusService.TriggerEvent(new ScorePointParam(scoreService.TotalPoint.ToString(), ScoreGradeEnum.None.ToString(), string.Empty));
            }

            var sequence = DOTween.Sequence();

            pressedImage.material = null;
            sequence.Append(tileRectTransform.DOScale(Vector3.one * ScaleUpSize, FadeInDuration));
            sequence.Join(image.DOColor(Color.white, FadeInDuration));
            sequence.Join(pressedImage.DOColor(Color.white, FadeInDuration));

            // Second phase: scale down to 0 and fade out
            sequence.Append(tileRectTransform.DOScale(0f, FadeOutDuration));
            sequence.Join(image.DOFade(0f, FadeOutDuration));
            sequence.Join(pressedImage.DOFade(0f, FadeOutDuration));

            await sequence.Play().WithCancellation(fadeCTS.Token);
            Object.Destroy(tileRectTransform.gameObject);
        }
    }
}
