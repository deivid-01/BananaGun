using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed*Time.deltaTime);
    }
}
