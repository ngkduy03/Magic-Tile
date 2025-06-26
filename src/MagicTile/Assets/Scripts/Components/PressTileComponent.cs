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
    private RectTransform laneRectTransform;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Image image;

    private bool isPressed = false;
    private TileAbstractController controller;
    private PressTileController pressController;
    private CancellationTokenSource cts = new();

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new PressTileController(tileRectTransform, pressedRectTransform, laneRectTransform, image, speed);
        return controller;
    }

    private void Awake()
    {
        controller = CreateController();
        pressController = controller as PressTileController;
    }

    private void Update()
    {
        controller.MoveTileDown();
        if (isPressed)
        {
            float progress = pressController.PressProgress();
            if (progress >= tileRectTransform.sizeDelta.y)
            {
                isPressed = false;
                controller.FadeTile(cts.Token).Forget();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        controller.FadeTile(cts.Token).Forget();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }
}
