using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTarget : MonoBehaviour
{
    RectTransform rectTransform;

    Shooting shooting;
    void Start()
    {
        shooting = Shooting.intance;
        rectTransform = GetComponent<RectTransform>();

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(shooting.shootWithMouse)
            transform.position = Input.mousePosition;
    }
}
