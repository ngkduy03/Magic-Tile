using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapTileComponent : TileAbstract, IPointerDownHandler
{
    [SerializeField]
    private RectTransform tileRectTransform;

    [SerializeField]
    private Image image;

    private float speed;
    private RectTransform laneRectTransform;
    private IScoreService scoreService;
    private TileAbstractController controller;
    private TapTileController tapController;

    public void Initialize(
        IScoreService scoreService,
        RectTransform laneRectTransform,
        float speed)
    {
        this.scoreService = scoreService;
        this.laneRectTransform = laneRectTransform;
        this.speed = speed;
    }

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new TapTileController(tileRectTransform, laneRectTransform, image, scoreService, speed);
        return controller;
    }

    private void Awake()
    {
        tapController = controller as TapTileController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tapController.FadeTile().Forget();
        tapController.OnTileScored();
    }
    
    private void OnDestroy()
    {
    }
}
