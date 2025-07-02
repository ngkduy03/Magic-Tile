using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartTileComponent : TileAbstract, IPointerDownHandler
{
    [SerializeField]
    private RectTransform tileRectTransform;

    [SerializeField]
    private Image image;

    private TileAbstractController controller;
    private StartTileController startTileController;
    private List<TileAbstractController> tileControllers;

    public void Initialized(
        List<TileAbstractController> tileControllers)
    {
        this.tileControllers = tileControllers;
    }

    protected override TileAbstractController CreateControllerImpl()
    {
        controller = new StartTileController(tileRectTransform, image, tileControllers);
        return controller;
    }

    private void Awake()
    {
        startTileController = controller as StartTileController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startTileController.ActivateTiles();
        startTileController.FadeTile().Forget();
    }
}
