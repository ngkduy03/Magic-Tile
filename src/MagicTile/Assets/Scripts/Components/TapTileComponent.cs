using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the tap tile interaction in the game.
/// </summary>
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
        if (!tapController.IsPressed)
        {
            tapController.IsPressed = true;
            tapController.FadeTile().Forget();
            tapController.OnTileScored();
        }
    }
}
