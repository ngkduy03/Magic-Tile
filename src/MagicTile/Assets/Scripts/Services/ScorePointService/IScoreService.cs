using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for the score service that manages scoring in the game.
/// </summary>
public interface IScoreService
{
    int TotalPoint { get; }

    int ComboPoint { get; }

    int ScorePoint(ScoreGradeEnum grade);

    int ResetPoint();
}
