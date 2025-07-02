using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreService : IScoreService
{
    private int totalPoint;
    public int TotalPoint => totalPoint;

    public int ScorePoint(int point)
    {
        totalPoint += point;
        Debug.Log($"Total Score: {totalPoint}");
        return totalPoint;
    }
}
