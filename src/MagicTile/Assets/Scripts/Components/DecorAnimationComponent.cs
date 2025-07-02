using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorAnimationComponent : SceneComponent<DecorAnimationController>
{
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image deadlineImage;

    private DecorAnimationController controller;

    protected override DecorAnimationController CreateControllerImpl()
    {
        controller = new DecorAnimationController(backgroundImage, deadlineImage);
        return controller;
    }

    private void Awake()
    {
        controller = CreateController();
        controller.FadeLoopDecoration();
    }
}
