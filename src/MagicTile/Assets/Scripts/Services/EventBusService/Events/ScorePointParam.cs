using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
