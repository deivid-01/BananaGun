using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    public GameObject pointsSection;
    public Text textTotalPoints;
    void Start()
    {
        GameEvent.instance.OnRoundEnds += DisplayPoints;        
    }

    void DisplayPoints()
    {
        pointsSection.SetActive(true);
        textTotalPoints.text = (PointsController.totalPoints).ToString();
    }

    public void CounterIsOver()
    {
        GameEvent.instance.StartGame();
    }


}
