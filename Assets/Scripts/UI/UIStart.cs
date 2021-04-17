// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------

using System.Collections.Generic; //Libreria de funcionalidades basicas de C#
using UnityEngine; // Importación de la libreria principal de Unity
using System; //Libreria de funcionalidades basicas de C#
using UnityEngine.SceneManagement; //Importancion de la libreria para manejar escenas de Unity
/// <summary>
/// Maneja la interfaz de usuario en la escena 2, cuando se va a iniciar el juego
/// </summary>
public class UIStart : MonoBehaviour
{
    [Serializable]
    //Clase para perzonalizar el tamaño de los botones presentados en la interfaz de usuario
    public class CustomBtn {
        public string name;
        public GameObject btn;
        [Range(0,1)]
        public float percen;
        [HideInInspector]
        public RectTransform rect;
    }
    [SerializeField]
    //Lista con los botones de la interfaz grafica
    List<CustomBtn> customBtns = new List<CustomBtn>();
    //Nombre de la siguiente escena
    public string nextScene;
    //Instancia de la clase que indica la posición a la que se apunta
    Pointer pointer; 
    //Velocidad en que cambia el tamaño del botón
    public float delta;
    /// <summary>
    /// Metodo por default de Unity al arrancar el juego
    /// </summary>
    private void Start()
    {
        //Se asigna el singlenton de la clase Pointer
        pointer = Pointer.instance;
        //Se asigna la Propiedad RectTransform a cada boton de la interfaz
        customBtns.ForEach(elem => elem.rect = elem.btn.GetComponent<RectTransform>());
        //Si se esta controlando con el mouse
        if (UIControls.mouseController)
        {
            //Se cambia la velocidad de crecimiento del botón
            delta /= 10;
        }
    }
    /// <summary>
    /// Salir del juego
    /// </summary>
    public void Exit() 
    {
       //Cierra el juego
        Application.Quit();
    }
    /// <summary>
    /// Metodo por default de Unity que se ejecuta cuando el juego termina o se recarga la escena
    /// </summary>
    private void Update()
    {
        //Obtiene la ubicación del mouse o del objeto a detectar e incrementa el valor de porcentaje
        SetPercentajes();

       //Segun el valor de porcentaje se asigna el tamaño a cad abotón
        customBtns.ForEach(elem => elem.rect.offsetMax = new Vector2(elem.rect.offsetMax.x, -720 * (1 - elem.percen)));
    }

  
    /// <summary>
    /// Asigna los porcentajes para cambiar el tamaño de los botones
    /// </summary>
    void SetPercentajes()
    {
         //Optiene la posición del mouse o del apuntador si se esta utilizando la camará
        Vector2 pos = (!UIControls.mouseController)?Vector2.up*720+ pointer.rect.anchoredPosition: (Vector2)Input.mousePosition;
        //Condicion para incrementar porcentaje en eje horizontal y vertical del boton de Start
        bool horizontalCheckStart = pos.x >= 315 && pos.x <= 715;
        bool verticalCheckStart = pos.y >= 31 && pos.y <= 264;

        //Condicion para incrementar porcentaje en eje horizontal y vertical del boton de Exit
        bool horizontalCheckExit = pos.x >=990;
        bool verticalCheckExit = pos.y >= 86 && pos.y <= 316;
        //asigna el porcentaje de un solo boton segun el tamaño definido 
        SetPercentaje(customBtns[0], horizontalCheckStart, verticalCheckStart); // Set start percentaje
        SetPercentaje(customBtns[1], horizontalCheckExit, verticalCheckExit); // Set exit percentaje
        //Si el boton de Start esta en su maximo tamaño 
        if (customBtns[0].percen == 1)
        {
            //Se ejecuta la accion cargar siguiente escena y se notifica a todos los suscriptores de este evento
            GameEvent.instance.LoadingNextScene();
            //Se notifica a todos los suscriptores que la opción fue seleccionada
            GameEvent.instance.OptionSelected();
            //Se inicia el juego
            StarGame();
        }


    }
    /// <summary>
    /// Calcula el porcentaje segun el tamaño defenido 
    /// </summary>
    /// <param name="b">Boton</param>
    /// <param name="horizontal">Condicion de tamaño en el eje horizontal</param>
    /// <param name="vertical">Condición de tamaño en el eje vertical</param>
    void SetPercentaje(CustomBtn b,bool horizontal, bool vertical) => b.percen = (horizontal && vertical) ? Mathf.Clamp(b.percen + delta, 0, 1) : Mathf.Clamp(b.percen - delta, 0, 1);
  
    /// <summary>
    /// Se inicia el juego
    /// </summary>
    public void StarGame()
    { 
        // Se carga la siguiente escena
        SceneManager.LoadScene(nextScene);
    } 
}


