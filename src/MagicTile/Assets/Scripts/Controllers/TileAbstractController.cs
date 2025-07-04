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
    protected bool isPressed = false;
    private const float perfectGradeEpsilon = 50f;
    protected IScoreService scoreService;
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

        using var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(MoveGlobalCTS.Token, moveCTS.Token);
        await tileRectTransform.DOAnchorPosY(laneYPivot, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                MoveGlobalCTS?.Cancel();
                MoveGlobalCTS?.Dispose();
            })
            .WithCancellation(linkedCTS.Token);
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
