using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShape : MonoBehaviour
{
    public GameObject effectFail;
    public GameObject effectNothing;

    public static Vector3 maximumSize = new Vector3(4,4,4);
    public static Vector3 minimumSize = new Vector3(2.5f,2.5f,2.5f);


    public string enemyColor;
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
        if (gameObject != null)
        {
            StartCoroutine(DestroyEnemy());
        }

    }

    IEnumerator DestroyEnemy() {

        yield return new WaitForSeconds(5f);
       

        if (gameObject != null)
        {

            if (RoundSystem.actualRound == 1)
            {
                if (!EnemySpawner2.actualColor.name.Equals(enemyColor))
                {
                    GameEvent.instance.EnemySuccess();
                }
                else
                {
                    gameObject.SetActive(false);
                    GameObject effectN = Instantiate(effectNothing, transform.position, Quaternion.identity);
                    Destroy(effectN, 2);
                    Destroy(gameObject, 2);
                    yield return null;
                }
            }
            else
                GameEvent.instance.EnemySuccess();
        }

        gameObject.SetActive(false);
        GameObject effect = Instantiate(effectFail, transform.position, Quaternion.identity);
        Destroy(effect, 2);
        Destroy(gameObject, 2);
    }

}
