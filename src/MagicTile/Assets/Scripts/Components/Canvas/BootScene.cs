using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BootScene : MonoBehaviour
{
    private const string GameScene = "GameLoopScene";

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button quitButton;

    private static ServiceContainer serviceContainer;
    private ILoadSceneService loadSceneService;

    private void Awake()
    {
        serviceContainer = new ServiceContainer();
        var serviceInitializer = new ServiceInitializer();
        serviceInitializer.InitializeServices(serviceContainer);
        loadSceneService = serviceContainer.Resolve<ILoadSceneService>();
        playButton.onClick.AddListener(OnPlayScene);
        quitButton.onClick.AddListener(OnQuitGame);
    }

    private void OnPlayScene()
    {
        loadSceneService.LoadSceneAsync(GameScene);
    }

    private void OnQuitGame()
    {
        loadSceneService.QuitGame();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayScene);
        quitButton.onClick.RemoveListener(OnQuitGame);
    }
}
