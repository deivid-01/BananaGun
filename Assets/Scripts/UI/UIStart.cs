using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
public class UIStart : MonoBehaviour
{
    [Serializable]
    public class CustomBtn {
        public string name;
        public GameObject btn;
        [Range(0,1)]
        public float percen;
        [HideInInspector]
        public RectTransform rect;
    }
    [SerializeField]
    List<CustomBtn> customBtns = new List<CustomBtn>();

    public string nextScene;

    Pointer pointer; 

    public float delta;
    private void Start()
    {
        pointer = Pointer.instance;
        
        customBtns.ForEach(elem => elem.rect = elem.btn.GetComponent<RectTransform>());

        if (UIControls.mouseController)
        {
            delta /= 10;
        }
    }

    public void Exit() 
    {
        Application.Quit();
    }

    private void Update()
    {
        SetPercentajes();

       //Set size button
        customBtns.ForEach(elem => elem.rect.offsetMax = new Vector2(elem.rect.offsetMax.x, -720 * (1 - elem.percen)));
    }

  

    void SetPercentajes()
    {
         
        Vector2 pos = (!UIControls.mouseController)?Vector2.up*720+ pointer.rect.anchoredPosition: (Vector2)Input.mousePosition;
        
        bool horizontalCheckStart = pos.x >= 315 && pos.x <= 715;
        bool verticalCheckStart = pos.y >= 31 && pos.y <= 264;

        //Exit
        bool horizontalCheckExit = pos.x >=990;
        bool verticalCheckExit = pos.y >= 86 && pos.y <= 316;

        SetPercentaje(customBtns[0], horizontalCheckStart, verticalCheckStart); // Set start percentaje
        SetPercentaje(customBtns[1], horizontalCheckExit, verticalCheckExit); // Set exit percentaje

        if (customBtns[0].percen == 1)
        {
            GameEvent.instance.LoadingNextScene();
            GameEvent.instance.OptionSelected();
            StarGame();
        }


    }

    void SetPercentaje(CustomBtn b,bool horizontal, bool vertical) => b.percen = (horizontal && vertical) ? Mathf.Clamp(b.percen + delta, 0, 1) : Mathf.Clamp(b.percen - delta, 0, 1);
  

    public void StarGame()
    { 
        SceneManager.LoadScene(nextScene);
    }






    
}


