using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the game canvas.
/// </summary>
public class GameCanvasController : ControllerBase
{
    private readonly CanvasScaler canvasScaler;

    public GameCanvasController(
        CanvasScaler canvasScaler)
    {
        this.canvasScaler = canvasScaler;
    }

    /// <summary>
    /// Sets the reference resolution for the canvas scaler.
    /// </summary>
    public void SetReferenceResolution()
    {
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);
        canvasScaler.matchWidthOrHeight = 0f;
    }
}
