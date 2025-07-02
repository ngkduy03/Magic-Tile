using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DecorAnimationController : ControllerBase
{
    private readonly Image backgroundImage;
    private readonly Image deadLineImage;
    private Color backgroundColor;
    private Color deadlineColor;

    public DecorAnimationController(
        Image backgroundImage,
        Image deadLineImage
    )
    {
        this.backgroundImage = backgroundImage;
        this.deadLineImage = deadLineImage;
        backgroundColor = backgroundImage.color;
        deadlineColor = deadLineImage.color;
    }

    public void FadeLoopDecoration()
    {
        // Create a sequence to handle both fade animations
        Sequence fadeSequence = DOTween.Sequence();

        // Create simultaneous fades (fade both to 0.5)
        Tween bgFadeOut = backgroundImage.DOFade(0.3f, 0.3f);
        Tween dlFadeOut = deadLineImage.DOFade(0.3f, 0.3f);
        fadeSequence.Append(bgFadeOut);
        fadeSequence.Join(dlFadeOut);

        // Create simultaneous fades back (fade both back to original)
        Tween bgFadeIn = backgroundImage.DOFade(backgroundColor.a, 0.5f);
        Tween dlFadeIn = deadLineImage.DOFade(deadlineColor.a, 0.5f);
        fadeSequence.Append(bgFadeIn);
        fadeSequence.Join(dlFadeIn);

        fadeSequence.AppendInterval(0.5f);

        // Set the sequence to loop indefinitely
        fadeSequence.SetLoops(-1, LoopType.Restart);
    }
}
