using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Parameter class for score point events in the game.
/// </summary>
public class ScorePointParam : IEventParameter
{
    public readonly string ScoreText;
    public readonly string GradeText;
    public readonly string ComboText;

    public ScorePointParam(
        string scoreText,
        string gradeText,
        string comboText)
    {
        ScoreText = scoreText;
        GradeText = gradeText;
        ComboText = comboText;
    }
}
