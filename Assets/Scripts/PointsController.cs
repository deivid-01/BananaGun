using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public static int totalPoints;
    void Start()
    {
        GameEvent.instance.OnEnemyDestroyed += IncreasePoints;
    }


    void IncreasePoints()
    {
        totalPoints += 100;
       
    }
    void DecreasePoints()
    {
        totalPoints -= 100;
    }
}
