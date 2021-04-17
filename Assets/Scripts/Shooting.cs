// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------
using System.Collections; //Libreria de funcionalidades basicas de C#
using UnityEngine;// Importación de la libreria principal de Unity

public class Shooting : MonoBehaviour
{
    //Efecto cuando hay un impacto
    public GameObject impactEffect;
    //Velocidad de disparo
    public float fireRate = 3f; 
   //auxiliar para controlar la velocidad de disparo
    private float nextTimeToFire = 0F;

    /// <summary>
    /// Metodo que se correr por defecto en Unity al momento de Iniciar el juego
    /// </summary>
    private void Start()
    {
        
        
        //Se suscribe al evento para saber cuando esta disparando
        GameEvent.instance.OnStartShoot += Shoot;
    }
    /// <summary>
    /// Metodo por defecto de Unity que se ejecuta cuando el objetro se destruye
    /// </summary>
    private void OnDestroy()
    {
    //Cancela la suscripción para cuando se dispara
        GameEvent.instance.OnStartShoot -= Shoot;
    }



   /// <summary>
   /// Metodo default de Unity que se ejecuta cada frame
   /// </summary>
    void Update()
    {
        //Si el mouse esta activo y presiona el boton izquierdo empieza a disparar
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

        //Para controlar la velocidad de disparo
        if (Time.time >= nextTimeToFire)
        {
            //Dispara un rayo en las 3d dimensiones
            Ray ray = Camera.main.ScreenPointToRay(position);
            //Verifica si el rato tocó algun objeto
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                GameEvent.instance.Shooting(ray.direction);
                //Verifica si el objetivo realmente fue el enemigo
                if (hit.transform.tag.Equals("Enemy"))
                {
                    //Destruye el enemigo 
                    GameEvent.instance.EnemyDestroyed();
                    //Muestra explosión
                    StartCoroutine(InstantiateEffect(hit));
                }
            }
            //Velocidad de disparo
            nextTimeToFire = Time.time + (1f / fireRate);
        }

       
    
    }
    /// <summary>
    /// Coroutina para mostrar efecto cada vez que dispara
    /// </summary>
    /// <param name="hit">Dirección de impacto</param>
    /// <returns></returns>

    IEnumerator InstantiateEffect(RaycastHit hit)
    {
       //Coroutina para que el enemigo dure un momento
        yield return new  WaitForSeconds(0.25f);
        if (hit.transform != null)
        {
            //Destruye el objetivo
            Destroy(hit.transform.gameObject);
        }
        
            //Muestra efecto de explosión
        GameObject impObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        //Despues de 1 segundo se destruye
        Destroy(impObj, 1f);

    }

}
