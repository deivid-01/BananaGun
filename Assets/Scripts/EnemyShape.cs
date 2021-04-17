//----------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------
using System.Collections; //Libreria de funcionalidades basicas de C#
using UnityEngine; // Importación de la libreria principal de Unity
// ----------------------
/// <summary>
/// Administra la generación aleatoria de la forma de los enemigos
/// </summary>
public class EnemyShape : MonoBehaviour
{
    //EFecto cuando se autodestruye
    public GameObject effectFail;
    //Efecto cuando no ocurre nada
    public GameObject effectNothing;
    //Maximo tamaño a ser generado
    public static Vector3 maximumSize = new Vector3(4,4,4);
    //Minimo tamaño a ser generado
    public static Vector3 minimumSize = new Vector3(2.5f,2.5f,2.5f);

    //nombre del color que lleva e enemigo
    public string enemyColor;
    /// <summary>
    /// Metodo que se corre por default de Unity al iniciar el juego
    /// </summary>
    void Start()
    {
        //Genera y asigna forma aleatoria
        Reshape();
    }
    /// <summary>
    /// Asigna forma aleatoria al objeto
    /// </summary>
    void Reshape()
    {
        transform.localScale = RandomShape();
    }
    /// <summary>
    /// Genera forma aleatoria
    /// </summary>
    /// <returns></returns>
    Vector3 RandomShape()
    {
        Vector3 shape;

        
        shape.x = Random.Range(minimumSize.x, maximumSize.x);
        shape.y = Random.Range(minimumSize.y, maximumSize.y);
        shape.z = Random.Range(minimumSize.z, maximumSize.z);

        return shape;
    }
    /// <summary>
    /// Metodo para dibujar en el editor, es una herramienta de testing de Unity
    /// </summary>
    private void OnDrawGizmos()
    {
        //Color del lapiz a dibujar
        Gizmos.color = Color.green;
        //Forma que se quiere dibujar
        Gizmos.DrawWireCube(transform.position, maximumSize);
    }
    /// <summary>
    /// metodo por default de unity cuando un objeto es activado
    /// </summary>
    private void OnEnable()
    {
        
        //si no es nulo lo destruye
        if (gameObject != null)
        {
            StartCoroutine(DestroyEnemy());
        }

    }
    /// <summary>
    /// Destruye el enemigo despues de cierto tiempo
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyEnemy() {

       //Espera 5 segundos
        yield return new WaitForSeconds(5f);
       
        

        if (gameObject != null)
        {

           //Verifica si debe tener en cuenta el color
           // Si el usuario se equivoca pierde puntos
            if (RoundSystem.actualRound == 1)
            {
                if (!EnemySpawner2.actualColor.name.Equals(enemyColor))
                {
                    GameEvent.instance.EnemySuccess();
                }
                else
                {
                   //Muestra efecto cuando  no hay puntos
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
        //Muestra efecto cuando pierde puntos
        gameObject.SetActive(false);
        GameObject effect = Instantiate(effectFail, transform.position, Quaternion.identity);
        Destroy(effect, 2);
        Destroy(gameObject, 2);
    }

}
