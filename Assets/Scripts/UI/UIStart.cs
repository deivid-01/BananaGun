using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
public class UIStart : MonoBehaviour
{

   public UnityEngine.Object nextScene; 


    public void Exit() 
    {
        Application.Quit();
    }

    public void StarGame()
    {
        SceneManager.LoadScene(nextScene.name);
       
    }


    
}


