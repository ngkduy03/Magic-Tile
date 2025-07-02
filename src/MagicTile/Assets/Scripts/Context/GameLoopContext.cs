using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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

    private bool isGameOver = false;
    private List<TileAbstractController> tileControllers = new();
    private ILoadSceneService loadSceneService;
    private IScoreService scoreService;

    protected override void Initialize(IServiceContainer serviceResolver)
    {
        loadSceneService = serviceResolver.Resolve<ILoadSceneService>();
        scoreService = serviceResolver.Resolve<IScoreService>();
        startTileComponent.Initialized(tileControllers);
        var startTileController = startTileComponent.CreateController();

        // Initialize all the tiles controllers in the lanes.
        foreach (Transform lane in lanes)
        {
            for (int i = 0; i < lane.childCount; i++)
            {
                if (lane.GetChild(i).TryGetComponent(out TileAbstract tileComponent))
                {
                    if (tileComponent is TapTileComponent tapTileComponent)
                    {
                        tapTileComponent.Initialize(scoreService, lane as RectTransform, speed, ref isGameOver);
                        var tileController = tapTileComponent.CreateController();
                        tileControllers.Add(tileController);
                    }
                    else if (tileComponent is PressTileComponent pressTileComponent)
                    {
                        pressTileComponent.Initialize(scoreService, lane as RectTransform, speed, ref isGameOver);
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

    private void Update()
    {
        if (isGameOver)
        {
            foreach (var tileController in tileControllers)
            {
                tileController.MoveCTS?.Cancel();
                tileController.MoveCTS?.Dispose();
            }
        }
    }
}
