using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMovement : MonoBehaviour
{
    public float speedRotation = 15;
    public float speedTranslate = 10;
    Vector3 originPosition;
 
    private void Start()
    {
        originPosition = transform.position;
        GameEvent.instance.OnShooting += Recoil;
        
    }

    void Recoil(Vector3 v)
    {
        transform.Rotate(Vector3.left* Random.Range(20f, 40f));
        transform.Translate(new Vector3(0, Random.Range(0.1f, 0.5f),-Random.Range(0.3f, 1.0f)));
    }

    private void OnDestroy()
    {
        GameEvent.instance.OnShooting -= Recoil;
    }

    void Update()
    {
        if(transform.position.x!=originPosition.x)
            transform.position =Vector3.Lerp(transform.position,originPosition, speedTranslate * Time.deltaTime);
        SmoothLookAt( FindTarget() );
    }

    Vector3 FindTarget()
    {
        Vector2 pos = (UIControls.mouseController) ? Input.mousePosition : new Vector3(Pointer.instance.rect.anchoredPosition.x, 720 + Pointer.instance.rect.anchoredPosition.y);
    
        Ray ray = Camera.main.ScreenPointToRay(pos);

        return (Physics.Raycast(ray, out RaycastHit hit, 100)) ? hit.point : ray.direction * 100;
    }

    void SmoothLookAt(Vector3 target)
    {
        Quaternion originalRot = transform.rotation;
        transform.LookAt(target);
        Quaternion newRot = transform.rotation;
        transform.rotation = originalRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, speedRotation * Time.deltaTime);

    }


}
