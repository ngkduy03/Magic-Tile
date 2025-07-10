using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressTileComponent : TileAbstract, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private RectTransform pressedRectTransform;

    private PressTileController pressController;

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new PressTileController(tileRectTransform, pressedRectTransform, laneRectTransform, image, scoreService, eventBusService, speed);
        return controller;
    }

    private void Awake()
    {
        pressController = controller as PressTileController;
    }

    private void Update()
    {
        if (pressController.IsPressed)
        {
            float progress = pressController.PressProgress();
            if (progress >= tileRectTransform.sizeDelta.y)
            {
                pressController.FadeTile().Forget();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressController.FadeTile().Forget();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!pressController.IsPressed)
        {
            pressController.IsPressed = true;
            pressController.OnTileScored();
        }
    }
}
