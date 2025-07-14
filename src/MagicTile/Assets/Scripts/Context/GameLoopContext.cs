using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Context for managing the game loop and tile interactions.
/// </summary>
[DefaultExecutionOrder(-1)]
public class GameLoopContext : BaseContext<ServiceInitializer>
{
    [SerializeField]
    private List<Transform> lanes;

    [SerializeField]
    private RectTransform endLineRectTransform;

    [SerializeField]
    private StartTileComponent startTileComponent;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameCanvasComponent gameCanvasComponent;

    private List<TileAbstractController> tileControllers = new();
    private ILoadSceneService loadSceneService;
    private IScoreService scoreService;
    private IEventBusService eventBusService;

    protected override void Initialize(IServiceContainer serviceResolver)
    {
        loadSceneService = serviceResolver.Resolve<ILoadSceneService>();
        scoreService = serviceResolver.Resolve<IScoreService>();
        eventBusService = serviceResolver.Resolve<IEventBusService>();

        gameCanvasComponent.Initialize(eventBusService,scoreService,loadSceneService);
        var gameCanvasController = gameCanvasComponent.CreateController();

        startTileComponent.Initialized(tileControllers, eventBusService);
        var startTileController = startTileComponent.CreateController();

        // Initialize all the tiles controllers in the lanes.
        foreach (Transform lane in lanes)
        {
            for (int i = 0; i < lane.childCount; i++)
            {
                if (lane.GetChild(i).TryGetComponent(out TileAbstract tileComponent))
                {
                    tileComponent.Initialize(scoreService, eventBusService, lane as RectTransform, speed);

                    if (tileComponent is TapTileComponent tapTileComponent)
                    {
                        var tileController = tapTileComponent.CreateController();
                        tileControllers.Add(tileController);
                    }
                    else if (tileComponent is PressTileComponent pressTileComponent)
                    {
                        var tileController = pressTileComponent.CreateController();
                        tileControllers.Add(tileController);
                    }
                }
            }
        }
    }

    protected override void Deinitialize()
    {
        tileControllers.Clear();
    }

}
