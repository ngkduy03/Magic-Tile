using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        totalPoint += (int)grade;

        if (grade == ScoreGradeEnum.Perfect)
        {
            comboPoint++;
            return totalPoint * comboPoint;
        }
        else if (grade == ScoreGradeEnum.EndPress)
        {
            return totalPoint;
        }
        else
        {
            comboPoint = 0;
            return totalPoint;
        }
    }
}
