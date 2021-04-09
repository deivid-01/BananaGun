using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEvent : MonoBehaviour
{
    // Start is called before the first frame update

    #region Singlenton
    public static GameEvent instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(instance);
        }
    }
    #endregion

    public event Action<Vector3> OnShoot;


    public void Shooting(Vector3 direction) => OnShoot?.Invoke(direction);

}
