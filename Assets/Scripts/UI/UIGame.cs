using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIGame : MonoBehaviour
{
    public GameObject pointsSection;
    public Text textTotalPoints;
    public string nextScene;

    private void Awake()
    {
        UIControls.mouseController = true;
    }
    void Start()
    {
        GameEvent.instance.OnRoundEnds += DisplayPoints;

     
        // UIControls.mouseController = (PlayerPrefs.GetInt("mouseActive", 0)==1)?true:false;
    }
    private void OnDestroy()
    {
        GameEvent.instance.OnRoundEnds -= DisplayPoints;
    }

    void DisplayPoints()
    {
     
        Cursor.visible = true;
        pointsSection.SetActive(true);
        textTotalPoints.text = (PointsController.totalPoints).ToString();
    }

    public void CounterIsOver()
    {
  
        GameEvent.instance.StartGame();
    }

    public void RestartGame()
    {
        GameEvent.instance.RestartingGame();
        PointsController.totalPoints = 0;
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
