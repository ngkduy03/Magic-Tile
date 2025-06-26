using System.Collections;
using System.Collections.Generic;
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
    protected bool IsPressed = false;

    public abstract void DestroyTile();

    public void MoveTileDown()
    {
        if (!HasReachedParentYPosition())
        {
            tileRectTransform.anchoredPosition += Vector2.down * speed * Time.deltaTime;
        }
    }

    public abstract UniTask FadeTile(CancellationToken cancellationToken);

    /// <summary>
    /// Checks if this tile's RectTransform has reached the Y position of its parent RectTransform
    /// </summary>
    /// <returns>True if the tile has reached the parent's Y position</returns>
    private bool HasReachedParentYPosition()
    {
        float childY = tileRectTransform.anchoredPosition.y;
        float parentY = laneRectTransform.anchoredPosition.y;
        return childY <= parentY;
    }
}
