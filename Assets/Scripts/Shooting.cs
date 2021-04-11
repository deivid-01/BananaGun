using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject impactEffect;

   


    public static Shooting intance;
    
 

    private void Awake()
    {
        intance = this;
    }



    // Update is called once per frame
    void Update()
    {

        if ( UIControls.mouseController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                  Shoot(Input.mousePosition);

            }
        }
        
    }

    public void Shoot(Vector2 position) 
    {
 
        Ray ray = Camera.main.ScreenPointToRay(position);
       
        if (Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            GameEvent.instance.Shooting( ray.direction);
                   
            if (hit.transform.tag.Equals("Enemy"))
            {
                GameEvent.instance.EnemyDestroyed();
               StartCoroutine( InstantiateEffect(hit));
            }
        }
    
    }


    IEnumerator InstantiateEffect(RaycastHit hit)
    {
        yield return new  WaitForSeconds(0.25f);
        Destroy(hit.transform.gameObject);
        GameObject impObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impObj, 1f);

    }

}
