using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class TileAbstractController : ControllerBase
{
    protected RectTransform tileRectTransform;
    protected RectTransform laneRectTransform;
    protected Image image;
    protected float speed;
    protected bool isGameOver = false;
    protected bool isPressed = false;
    private const float perfectGradeEpsilon = 50f;
    protected IScoreService scoreService;
    public CancellationTokenSource MoveCTS = new();

    public abstract void DestroyTile();

    public abstract UniTask FadeTile();

    public virtual void OnTileScored()
    {
        if (PerfectScoreCheck())
        {
            scoreService.ScorePoint((int)ScoreGradeEnum.Perfect);
        }
        else if (HasTileSizeReachedLanePivot())
        {
            scoreService.ScorePoint((int)ScoreGradeEnum.Great);
        }
        else
        {
            scoreService.ScorePoint((int)ScoreGradeEnum.Cool);
        }

        Debug.Log($"Total Score: {scoreService.TotalPoint}");
    }

    public virtual async UniTask MoveTileDown()
    {
        var tileYPivot = tileRectTransform.anchoredPosition.y;
        var laneYPivot = laneRectTransform.anchoredPosition.y;
        float distance = Mathf.Abs(tileYPivot - laneYPivot);
        float duration = distance / speed;
        await tileRectTransform.DOAnchorPosY(laneYPivot, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => isGameOver = true)
            .WithCancellation(MoveCTS.Token);
    }

    private bool HasTilePivotReachedLanePivot()
    {
        var tileYPivot = tileRectTransform.anchoredPosition.y;
        var laneYPivot = laneRectTransform.anchoredPosition.y;
        return FloatEquals(tileYPivot, laneYPivot, 1);
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
}
