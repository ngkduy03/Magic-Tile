using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePointParam : IEventParameter
{
    public readonly string ScoreText;
    public readonly string GradeText;

    public ScorePointParam(
        string scoreText,
        string gradeText)
    {
        ScoreText = scoreText;
        GradeText = gradeText;
    }
}
