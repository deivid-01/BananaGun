using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIGame : MonoBehaviour
{
    public GameObject pointsSection;
    public GameObject colorAlertMessage;
    public Image colorBackground;
    public Text colorText;
    public Text textTotalPoints;
    public string nextScene;
    
    private void Awake()
    {
      // UIControls.mouseController = true;
    }
    void Start()
    {
        GameEvent.instance.OnRoundEnds += DisplayPoints;
        GameEvent.instance.OnChangeEnemyColor += DisplayAlertColor;

     
       
         UIControls.mouseController = (PlayerPrefs.GetInt("mouseActive", 0)==1)?true:false;
    }
    private void OnDestroy()
    {
        GameEvent.instance.OnRoundEnds -= DisplayPoints;
        GameEvent.instance.OnChangeEnemyColor -= DisplayAlertColor;
    }

    void DisplayPoints()
    {
     
        Cursor.visible = true;
        pointsSection.SetActive(true);
        textTotalPoints.text = (PointsController.totalPoints).ToString();
    }

    void DisplayAlertColor(Color color,string name)
    {
        //colorBackground.color.a = color.a;
        colorBackground.color = color;
        colorText.text = name;
        colorAlertMessage.SetActive(true);

        StartCoroutine(HideColorAlert());

    }

    IEnumerator HideColorAlert()
    { 
        yield return new WaitForSeconds(3f);
        colorAlertMessage.SetActive (false);
    }

    public void CounterIsOver()
    {
  
        GameEvent.instance.StartGame();
    }

    public void NextRound()
    {
        ResetAll();
        GameEvent.instance.RoundEnds();
        GameEvent.instance.LoadingNextScene();
        SceneManager.LoadScene(nextScene);


    }

    public void RestartGame()
    {
        GameEvent.instance.RestartingGame();
        ResetAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ResetAll()
    {
        PointsController.totalPoints = 0;
        StopAllCoroutines();
    }


}
