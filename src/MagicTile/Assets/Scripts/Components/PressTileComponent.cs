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
        if (isPressed)
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
        if (!isPressed)
        {
            isPressed = true;
            pressController.OnTileScored();
        }
    }

    private void OnDestroy()
    {
    }
}
