using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreService
{
    int TotalPoint { get; }

    int ScorePoint(int points);
}
