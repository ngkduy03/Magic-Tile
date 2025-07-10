using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the game canvas.
/// </summary>
public class GameCanvasComponent : SceneComponent<GameCanvasController>
{
    // Scale reference resolution for the canvas scaler
    [SerializeField]
    private CanvasScaler canvasScaler;

    // Images for background and deadline decoration.
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image deadlineImage;

    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text gradeText;

    [SerializeField]
    private TMP_Text comboText;

    [SerializeField]
    private AudioSource bgAudioSource;

    [SerializeField]
    private Canvas overCanvas;

    [SerializeField]
    private TMP_Text overScoreText;

    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private Button menuButton;

    private IEventBusService eventBusService;
    private IScoreService scoreService;
    private ILoadSceneService loadSceneService;
    private GameCanvasController controller;

    public void Initialize(
        IEventBusService eventBusService,
        IScoreService scoreService,
        ILoadSceneService loadSceneService)
    {
        this.eventBusService = eventBusService;
        this.scoreService = scoreService;
        this.loadSceneService = loadSceneService;
    }

    protected override GameCanvasController CreateControllerImpl()
    {
        controller = new GameCanvasController(canvasScaler, backgroundImage, deadlineImage, progressSlider, scoreText, gradeText, comboText, bgAudioSource, overCanvas, overScoreText, retryButton, menuButton, eventBusService, scoreService, loadSceneService);
        return controller;
    }

    private void Start()
    {
        controller.SetReferenceResolution();
    }

    private void OnDestroy()
    {
        controller?.Dispose();
        controller = null;
    }
}
