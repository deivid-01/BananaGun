//----------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------

using UnityEngine; // Importación de la libreria principal de Unity
/// <summary>
/// Administra la posición que va a tener  la posicion del elemento para apuntar
/// </summary>
public class Pointer : MonoBehaviour
{
    [HideInInspector]
    //Ancho y largo del elemento
    public RectTransform rect;
    //Velocidad para cambiar de posicion
     public float speed = 20;
    //Siguiente posicion
    Vector2 desirePosition = new Vector3();

    #region Singlenton
    public static Pointer instance;
    private void Awake()
    {
        instance = this;
    }

    #endregion
    void Start()
    {
       //Suscripción al evento para ubicarlo cada vez que detecta la bolita
        GameEvent.instance.OnMuzzelDetected += SetPosition;

        //Asigna el valor de ancho y largo

        rect = GetComponent<RectTransform>();
        //Oculta el cursor

        Cursor.visible = false;
    }
    private void OnDestroy()
    {
     //Cancela suscripcion para ubicar al objeto
        GameEvent.instance.OnMuzzelDetected -= SetPosition;
  

    }

 
  /// <summary>
  /// Se ejecuta cada frame durante el juego
  /// </summary>
    void Update()
    {
       //Si el mouse esta activo pone la posicion del mouse
        if (UIControls.mouseController)
            SetMousePosition();
        //SIno pone la posicion de la bolita donde ha sido detectada
        else
        {
            UpdatePosition();
        }
    }

    /// <summary>
    /// Pone posicion del mouse
    /// </summary>
    void SetMousePosition() => transform.position = Input.mousePosition;
    /// <summary>
    /// Pone la posicion a la que hay que a puntar cuando detecta la bolita
    /// </summary>
    /// <param name="position"></param>
    void SetPosition(Vector2 position)
    {
        //Se asigna la siguiente posicion
        desirePosition = position;
        //y se activa si esta desactivado en la escena
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);

      
    }
/// <summary>
/// Actualiza la posicion del apuntador cada frame
/// </summary>
    void UpdatePosition()
    {
        //Se actualiza la posicion de forma suave
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, desirePosition, speed * Time.deltaTime);
    }

}
