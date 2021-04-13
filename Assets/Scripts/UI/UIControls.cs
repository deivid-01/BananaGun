using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    public string nextScene;

    public static bool mouseController = true;

    public void SetController(bool option)
    {
        PlayerPrefs.SetInt("mouseActive", option?1:0);
        mouseController = option;

        SceneManager.LoadScene(nextScene);
    }
}
