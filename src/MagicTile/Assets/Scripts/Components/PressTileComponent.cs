using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressTileComponent : TileAbstract, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private RectTransform tileRectTransform;

    [SerializeField]
    private RectTransform pressedRectTransform;

    [SerializeField]
    private Image image;

    private float speed;
    private RectTransform endLineRectTransform;
    private bool isPressed = false;
    private TileAbstractController controller;
    private IScoreService scoreService;
    private PressTileController pressController;

    public void Initialize(
        IScoreService scoreService,
        RectTransform endLineRectTransform,
        float speed)
    {
        this.scoreService = scoreService;
        this.endLineRectTransform = endLineRectTransform;
        this.speed = speed;
    }

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new PressTileController(tileRectTransform, pressedRectTransform, endLineRectTransform, image, scoreService, speed);
        return controller;
    }

    private void Awake()
    {
        pressController = controller as PressTileController;
    }

    private void Update()
    {
        if (isPressed)
        {
            float progress = pressController.PressProgress();
            if (progress >= tileRectTransform.sizeDelta.y)
            {
                isPressed = false;
                pressController.FadeTile().Forget();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        pressController.FadeTile().Forget();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressController.OnTileScored();
    }

    private void OnDestroy()
    {
    }
}
