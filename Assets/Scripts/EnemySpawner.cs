using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;
using System;

public static class RandomNew
{
    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
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
    List<int> randomDimX = new List<int>();
    List<int> randomDimY = new List<int>();
    List<int> randomDimZ = new List<int>();

    private void Awake()
    {
        maxNumEnemys = (int)(dimensions.x * dimensions.y * dimensions.z);
        SetEnemyPositions();
        SetGoldIndexs();
        SetRandomStacks();
        

    }




    void Start()
    {

        GameEvent.instance.OnStartGame += StartSpawn;
    }

    void SetRandomStacks() {

        InitializeList(ref randomDimX,(int)dimensions.x);
        InitializeList(ref randomDimY,(int)dimensions.y);
        InitializeList(ref randomDimZ,(int)dimensions.z);

        randomDimX.Shuffle();
       

        randomDimY.Shuffle();
        randomDimZ.Shuffle();
 


    }

    private void OnDestroy()
    {
        GameEvent.instance.OnStartGame -= StartSpawn;
    }

    void InitializeList(ref List<int> list, int dim)
    {
        for (int i = 0; i < dim; i++)
        {
            list.Add(i);
        }
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
                    if (!idxsX.Contains(randomDimX[i]) || !idxsY.Contains(randomDimY[j]) || !idxsZ.Contains(randomDimZ[k]) ) continue;

                    yield return new WaitForSeconds(0.6f);
                    GameObject effect= Instantiate(effectShowup, this.transform.position + enemysPositions[randomDimX[i], randomDimY[j], randomDimZ[k]], Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                    Destroy(effect, 1);
                    GameObject enemy = Instantiate(prefabEnemy, this.transform.position + enemysPositions[randomDimX[i], randomDimY[j], randomDimZ[k]], Quaternion.identity);

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
            int idx = UnityEngine.Random.Range(0, maxSize);
            if (!idxs.Contains(idx))
            {
                idxs.Add(idx);
            }
        }
    }

}
