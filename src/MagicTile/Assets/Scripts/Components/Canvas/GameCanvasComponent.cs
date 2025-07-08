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
    private AudioSource bgAudioSource;

    private IEventBusService eventBusService;
    private GameCanvasController controller;
    
    public void Initialize(IEventBusService eventBusService)
    {
        this.eventBusService = eventBusService;
    }

    protected override GameCanvasController CreateControllerImpl()
    {
        controller = new GameCanvasController(canvasScaler, backgroundImage, deadlineImage, progressSlider, scoreText, gradeText, bgAudioSource, eventBusService);
        return controller;
    }

    private void Start()
    {
        controller.SetReferenceResolution();
    }
}
