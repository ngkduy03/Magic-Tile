using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <inheritdoc/>
public class ScoreService : IScoreService
{
    private int totalPoint = 0;
    public int TotalPoint => totalPoint;

    private int comboPoint = 0;
    public int ComboPoint => comboPoint;

    public int ResetPoint()
    {
        comboPoint = 0;
        return totalPoint = 0;
    }

    public int ScorePoint(ScoreGradeEnum grade)
    {
        var scorePoint = (int)grade;

        if (grade == ScoreGradeEnum.Perfect)
        {
            comboPoint++;
            scorePoint *= comboPoint;
            totalPoint += scorePoint;
        }
        else if (grade == ScoreGradeEnum.EndPress)
        {
            totalPoint += 1;
        }
        else
        {
            comboPoint = 0;
            totalPoint += scorePoint;
        }

        return totalPoint;
    }
}
