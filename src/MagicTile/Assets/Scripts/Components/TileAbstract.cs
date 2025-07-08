using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TileAbstract : SceneComponent<TileAbstractController>
{
    [SerializeField]
    protected RectTransform tileRectTransform;

    [SerializeField]
    protected Image image;

    protected bool isPressed = false;
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
