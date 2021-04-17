// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------

using System.Collections;//Libreria de funcionalidades basicas de C#
using UnityEngine; // Importación de la libreria principal de Unity
using UnityEngine.UI;// Importación de la libreria de interfaz de usuario de Unity
using UnityEngine.SceneManagement;//Importancion de la libreria para manejar escenas de Unity
/// <summary>
/// Controla la interfaz gráfica del juego principal
/// </summary>
public class UIGame : MonoBehaviour
{
   //Objeto que muestra los puntos
    public GameObject pointsSection;
    //Objeto que muestra la alertas para cambiar el color
    public GameObject colorAlertMessage;
    //Panel de fondo de los mensajes que alertan para cambiar de color
    public Image colorBackground;
    //Texto que contiene el nombre del color al que hay que cambiar
    public Text colorText;
    //Texto que contiene el total de puntos del jugador
    public Text textTotalPoints;
    //Nombre de la siguiente escena
    public string nextScene;

    /// <summary>
    /// Metodo por default de Unity al arrancar el juego
    /// </summary>
    void Start()
    {
        //Suscripción a los eventos del GameEvent
        GameEvent.instance.OnRoundEnds += DisplayPoints;
        GameEvent.instance.OnChangeEnemyColor += DisplayAlertColor;

     
        //Verifica si la camara o el mouse está encendido
         UIControls.mouseController = (PlayerPrefs.GetInt("mouseActive", 0)==1)?true:false;
    }
    /// <summary>
    /// Metodo por defecto de unity que corre al cambiar de escena o terminar el jeugo
    /// </summary>
    private void OnDestroy()
    {
        //Cancelar suscricpción a los eventos del Game Event
        GameEvent.instance.OnRoundEnds -= DisplayPoints;
        GameEvent.instance.OnChangeEnemyColor -= DisplayAlertColor;
    }
    /// <summary>
    /// Muestra el puntaje total del jugador
    /// </summary>
    void DisplayPoints()
    {
        //Se habila el cursor
        Cursor.visible = true;
        //Se habilita la sección que muestra los puntos
        pointsSection.SetActive(true);
        //Se pone en el texto el total de puntos
        textTotalPoints.text = (PointsController.totalPoints).ToString();
    }
/// <summary>
/// Muestra en la interfaz de usuario cuando hay que cambiar de color para destruir los cubos
/// </summary>
/// <param name="color"> Color que se va a mostrar</param>
/// <param name="name">Nombre del color</param>
    void DisplayAlertColor(Color color,string name)
    {
        //Se asigna el color de fondo
        colorBackground.color = color;
        //Se pone el nombre en el texto
        colorText.text = name;
        //Se habila la elerta
        colorAlertMessage.SetActive(true);
        //Se lanza coroutina para ocultar la alerta segundos despues
        StartCoroutine(HideColorAlert());

    }
    /// <summary>
    /// Coroutina para ocultar el mensaje de alerta segundos despues
    /// </summary>
    /// <returns></returns>
    IEnumerator HideColorAlert()
    { 
        //Espera dura 3 segundos para correr la siguiente instrucción
        yield return new WaitForSeconds(3f);
        //Se desactiva el mensaje de alerta
        colorAlertMessage.SetActive (false);
    }
    /// <summary>
    /// Avisa cuando la animación del contador ha terminado
    /// </summary>
    public void CounterIsOver()
    {
        //Se inicia el juego
        GameEvent.instance.StartGame();
    }
    /// <summary>
    /// Se carga el siguiente round
    /// </summary>
    public void NextRound()
    {
       //Se limpian todos los datos del round actual
        ResetAll();
        //Se notifica a todos los eventos suscritos que el evento termino
        GameEvent.instance.RoundEnds();
        //Se notifica a todos los eventos suscritos que se va a cargar la siguente escena

        GameEvent.instance.LoadingNextScene();
        //Se carga la escena
        SceneManager.LoadScene(nextScene);


    }
    /// <summary>
    /// Se reinicia el juego
    /// </summary>
    public void RestartGame()
    {
        //Se notifica a todos los eventos suscritos que se va a reiniciar el juego
        GameEvent.instance.RestartingGame();
        //Limpia todos los datos de la escena actual
        ResetAll();
        //Se recarga la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Limpia los datos de la escena actual
    /// </summary>
    void ResetAll()
    {
        //Pone el total de puntos en cero
        PointsController.totalPoints = 0;
        //Detiene todos los hilos creados
        StopAllCoroutines();
    }


}
