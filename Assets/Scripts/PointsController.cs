using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public static int totalPoints;
    void Start()
    {
        GameEvent.instance.OnEnemyDestroyed += IncreasePoints;
        GameEvent.instance.OnEnemySuccess += DecreasePoints;
        GameEvent.instance.OnRestartingGame += ResetPoints;
    }

    private void OnDestroy()
    {
        GameEvent.instance.OnEnemyDestroyed -= IncreasePoints;
        GameEvent.instance.OnRestartingGame -= ResetPoints;
        GameEvent.instance.OnEnemySuccess -= DecreasePoints;
    }


    void IncreasePoints()
    {
        totalPoints += 100;
       
    }
    void DecreasePoints()
    {
       
        totalPoints -= 100;
        if (totalPoints < 0)
            totalPoints = 0;
    }

    void ResetPoints()
    {
        totalPoints = 0;
    }
}
