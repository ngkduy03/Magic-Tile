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
    private RectTransform laneRectTransform;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Image image;

    private TileAbstractController controller;
    private CancellationTokenSource cts = new();

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new TapTileController(tileRectTransform, laneRectTransform, image, speed);
        return controller;
    }

    private void Awake()
    {
        controller = CreateController();
    }

    private void Update()
    {
        controller.MoveTileDown();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        controller.FadeTile(cts.Token).Forget();
    }
    
    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }
}
