using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTarget : MonoBehaviour
{

    Shooting shooting;
    void Start()
    {
        shooting = Shooting.intance;
        //Hide Cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting.shootWithMouse)
            SetPosition();
           
    }

    void SetPosition() => transform.position = Input.mousePosition;
}
