using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefabEnemy;
    public GameObject effectShowup;
    

    int maxNumEnemys;
    public Vector3 dimensions;

    Vector3[,,] enemysPositions;

    List<int> idxsX = new List<int>();
    List<int> idxsY = new List<int>();
    List<int> idxsZ = new List<int>();

    private void Awake()
    {
        maxNumEnemys = (int)(dimensions.x * dimensions.y * dimensions.z);
        SetEnemyPositions();
        SetGoldIndexs();
        

    }


    void Start()
    {
        GameEvent.instance.OnStartGame += StartSpawn;
    }

    void StartSpawn()
    {
        StartCoroutine(SpawnEnemys());
    }


    IEnumerator SpawnEnemys()
    {
        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                for (int k = 0; k < dimensions.z; k++)
                {
                    if (!idxsX.Contains(i) || !idxsY.Contains(j) || !idxsZ.Contains(k) ) continue;

                    yield return new WaitForSeconds(0.6f);
                    GameObject effect= Instantiate(effectShowup, this.transform.position + enemysPositions[i, j, k], Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                    Destroy(effect, 1);
                    GameObject enemy = Instantiate(prefabEnemy, this.transform.position + enemysPositions[i,j,k], Quaternion.identity);

                    enemy.transform.parent = gameObject.transform;
                }
            }
        }

        yield return new WaitForSeconds(2);
        GameEvent.instance.RoundEnds();
    }

    void SetEnemyPositions()
    {
        enemysPositions = new Vector3[(int)dimensions.x, (int)dimensions.y,(int)dimensions.z];
        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                for (int k = 0; k < dimensions.z; k++)
                {
                enemysPositions[i,j,k] = GeneratePosition(i,j,k);
                }
            }
            
            
        }
    }

    Vector3 GeneratePosition(int i,int j, int k)
    {
        return new Vector3( i * EnemyShape.maximumSize.x, j * EnemyShape.maximumSize.y, k * EnemyShape.maximumSize.z);
    }

    void SetGoldIndexs()
    {
        AddIndexs(ref idxsX, (int)dimensions.x);
        AddIndexs(ref idxsY, (int)dimensions.y);
        AddIndexs(ref idxsZ, (int)dimensions.z);
    }

    void AddIndexs(ref List<int> idxs, int maxSize)
    {
        while (idxs.Count < maxSize/2)
        {
            int idx = Random.Range(-1, maxSize);
            if (!idxs.Contains(idx))
            {
                idxs.Add(idx);
            }
        }
    }

}
