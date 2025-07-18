using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstract class for tile components in the game.
/// </summary>
public abstract class TileAbstract : SceneComponent<TileAbstractController>
{
    [SerializeField]
    protected RectTransform tileRectTransform;

    [SerializeField]
    protected Image image;

    protected float speed;
    protected RectTransform laneRectTransform;
    protected IScoreService scoreService;
    protected IEventBusService eventBusService;
    protected TileAbstractController controller;

    public void Initialize(
        IScoreService scoreService,
        IEventBusService eventBusService,
        RectTransform laneRectTransform,
        float speed)
    {
        this.scoreService = scoreService;
        this.eventBusService = eventBusService;
        this.laneRectTransform = laneRectTransform;
        this.speed = speed;
    }
}
