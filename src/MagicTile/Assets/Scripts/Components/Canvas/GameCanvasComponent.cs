using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the game canvas.
/// </summary>
public class GameCanvasComponent : SceneComponent<GameCanvasController>
{

    [SerializeField]
    private CanvasScaler canvasScaler;

    private GameCanvasController controller;
    protected override GameCanvasController CreateControllerImpl()
    {
        controller = new GameCanvasController(canvasScaler);
        return controller;
    }

    private void Awake()
    {
        controller = CreateController();
    }

    private void Start()
    {
        controller.SetReferenceResolution();
    }
}
