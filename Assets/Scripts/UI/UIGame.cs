using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
