using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShape : MonoBehaviour
{
    public static Vector3 maximumSize = new Vector3(5,5,5);
    public static Vector3 minimumSize = new Vector3(1,1,1);
    
    

    void Start()
    {
        Reshape();
    }

    void Reshape()
    {
        transform.localScale = RandomShape();
    }

    Vector3 RandomShape()
    {
        Vector3 shape;

        shape.x = Random.Range(minimumSize.x, maximumSize.x);
        shape.y = Random.Range(minimumSize.y, maximumSize.y);
        shape.z = Random.Range(minimumSize.z, maximumSize.z);

        return shape;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, maximumSize);
    }

    private void OnEnable()
    {
      
            Destroy(gameObject, 2f);
    }
}
