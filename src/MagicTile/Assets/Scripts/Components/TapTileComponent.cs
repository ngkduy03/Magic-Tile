using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class TapTileComponent : TileAbstract, IPointerDownHandler
{
    private TapTileController tapController;

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new TapTileController(tileRectTransform, laneRectTransform, image, scoreService, eventBusService, speed);
        return controller;
    }

    private void Awake()
    {
        tapController = controller as TapTileController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPressed)
        {
            isPressed = true;
            tapController.FadeTile().Forget();
            tapController.OnTileScored();
        }
    }
    
    private void OnDestroy()
    {
    }
}
