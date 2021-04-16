using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject impactEffect;
    public float fireRate = 3f; 
   
    private float nextTimeToFire = 0F;

    private void Start()
    {
        GameEvent.instance.OnStartShoot += Shoot;
    }

    private void OnDestroy()
    {
        GameEvent.instance.OnStartShoot -= Shoot;
    }



    // Update is called once per frame
    void Update()
    {

        if ( UIControls.mouseController)
        {
            if (Input.GetMouseButton(0) )
            {
                  Shoot(Input.mousePosition);
            }
        }
        
    }

    public void Shoot(Vector2 position) 
    {

        if (Time.time >= nextTimeToFire)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                GameEvent.instance.Shooting(ray.direction);

                if (hit.transform.tag.Equals("Enemy"))
                {
                    GameEvent.instance.EnemyDestroyed();
                    StartCoroutine(InstantiateEffect(hit));
                }
            }

            nextTimeToFire = Time.time + (1f / fireRate);
        }

       
    
    }


    IEnumerator InstantiateEffect(RaycastHit hit)
    {
        yield return new  WaitForSeconds(0.25f);
        if (hit.transform != null)
        {
            Destroy(hit.transform.gameObject);
        }
            
        GameObject impObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impObj, 1f);

    }

}
