using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [HideInInspector]
    public RectTransform rect;

     public float speed = 20;
    Vector2 desirePosition = new Vector3();

    #region Singlenton
    public static Pointer instance;
    private void Awake()
    {
        instance = this;
    }

    #endregion
    void Start()
    {
        GameEvent.instance.OnMuzzelDetected += SetPosition;


        rect = GetComponent<RectTransform>();
        //Hide Cursor
        Cursor.visible = false;
    }
    private void OnDestroy()
    {
        GameEvent.instance.OnMuzzelDetected -= SetPosition;
  

    }

    void Disable() {
        //if (gameObject.activeInHierarchy) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIControls.mouseController)
            SetMousePosition();
        else
        {
            UpdatePosition();
        }
    }


    void SetMousePosition() => transform.position = Input.mousePosition;

    void SetPosition(Vector2 position)
    {
        desirePosition = position;
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);

        //rect.anchoredPosition = position;
      
    }

    void UpdatePosition()
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, desirePosition, speed * Time.deltaTime);
    }

}
