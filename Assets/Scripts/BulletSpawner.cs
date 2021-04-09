using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    public GameObject bullet;

    public static BulletSpawner instance;

    public int forceMagnitude=1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameEvent.instance.OnShoot += SpawnBullet;
    }

    public void SpawnBullet(Vector3 direction)
    {
        GameObject bulletInst = Instantiate(bullet, transform.position, Quaternion.identity);

        bulletInst.GetComponent<Rigidbody>().AddForce( direction* forceMagnitude);

        Destroy(bulletInst, 0.25f);

    }
}
