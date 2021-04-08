using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMovement : MonoBehaviour
{
    public Transform[] targets;

    public Transform customPivot;

    public float rotateAmount = 1;

    public float turnSpeed=1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out  hit, 200f))
        {
            transform.LookAt(hit.point);


        }


        // transform.Rotate(Vector3.up,-turnSpeed*Time.deltaTime);
        //((transform.Rotate(customPivot.position, Vector3.up, 20 * Time.deltaTime);


    }
}
