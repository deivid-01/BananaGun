using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    public UnityEngine.Object nextScene;

    public static bool mouseController = true;

    public void SetController(bool option)
    {
        mouseController = option;

        SceneManager.LoadScene(nextScene.name);
    }
}
