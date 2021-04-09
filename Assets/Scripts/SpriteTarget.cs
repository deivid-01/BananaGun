using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTarget : MonoBehaviour
{

    void Start()
    {
        
        //Hide Cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIControls.mouseController)
            SetPosition();
           
    }

    void SetPosition() => transform.position = Input.mousePosition;
}
