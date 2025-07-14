using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the start tile interaction in the game.
/// </summary>
public class StartTileComponent : TileAbstract, IPointerDownHandler
{
    private StartTileController startTileController;
    private List<TileAbstractController> tileControllers;

    public void Initialized(
        List<TileAbstractController> tileControllers,
        IEventBusService eventBusService)
    {
        this.tileControllers = tileControllers;
        this.eventBusService = eventBusService;
    }

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new StartTileController(tileRectTransform, image, tileControllers, eventBusService);
        return controller;
    }

    private void Awake()
    {
        startTileController = controller as StartTileController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!startTileController.IsPressed)
        {
            startTileController.IsPressed = true;
            startTileController.FadeTile().Forget();
            startTileController.ActivateTiles();
        }
    }
}
