//----------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------

using UnityEngine;// Importación de la libreria principal de Unity
/// <summary>
/// Instancia las balas cada vez que dispara
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    //Modelo 3d de la bala
    public GameObject bullet;
    //Singlenton
    public static BulletSpawner instance;
    //Fuerza de disparo
    public int forceMagnitude=1;
    /// <summary>
    /// Se ejecuta milisegundos antes de arrancar el juego
    /// </summary>
    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Se ejecuta cuando juego inicia
    /// </summary>
    private void Start()
    {
      // Se suscribe al evento
        GameEvent.instance.OnShooting += SpawnBullet;
    }
    /// <summary>
    /// Se ejecuta cuando el objeto se destruye
    /// </summary>
    private void OnDestroy()
    {
      //Cancela suscripción
        GameEvent.instance.OnShooting -= SpawnBullet;

    }
    /// <summary>
    ///Genera la bala en cierta dirección
    /// </summary>
    /// <param name="direction"></param>
    public void SpawnBullet(Vector3 direction)
    {
        GameObject bulletInst = Instantiate(bullet, transform.position, Quaternion.identity);

        //Añade fuerza a la bala
        bulletInst.GetComponent<Rigidbody>().AddForce( direction* forceMagnitude);
        //La destruye despues de 0.25 segundos
        Destroy(bulletInst, 0.25f);

    }
}
