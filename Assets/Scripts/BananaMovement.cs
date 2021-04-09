using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMovement : MonoBehaviour
{
    public float speed = 15;

    void Update()
    {
        SmoothLookAt( FindTarget() );
    }

    Vector3 FindTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return (Physics.Raycast(ray, out RaycastHit hit, 100)) ? hit.point : ray.direction * 100;
    }

    void SmoothLookAt(Vector3 target)
    {
        Quaternion originalRot = transform.rotation;
        transform.LookAt(target);
        Quaternion newRot = transform.rotation;
        transform.rotation = originalRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, speed * Time.deltaTime);

    }


}
