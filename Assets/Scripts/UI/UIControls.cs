// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------

using UnityEngine;// Importación de la libreria principal de Unity
using UnityEngine.SceneManagement; //Importancion de la libreria para manejar escenas de Unity
/// <summary>
/// Esta clase se encarga de manejar los eventoss de la interfaz de usuario en la escena para
/// seleccionar el controlador
/// </summary>
public class UIControls : MonoBehaviour
{
    //Nombre de la siguiente escena a cargar
    public string nextScene;
    //Variable que contiene si se va a jugar con la cámara o con el mouse
    public static bool mouseController = false;
    /// <summary>
    /// Metodo por default de Unity al arrancar el juego
    /// </summary>
    private void Start()
    {
        //Borra lo que se haya guardado en memoria
        PlayerPrefs.DeleteAll();
    }
/// <summary>
/// Asigna el controlador que se va a usar
/// </summary>
/// <param name="option"> la opción elegida</param>
    public void SetController(bool option)
    {
        //Se guarda la opción elegida
        PlayerPrefs.SetInt("mouseActive", option?1:0);
        //Se asigna la opción elegida
        mouseController = option; 
        //Se carga la siguiente escena
        SceneManager.LoadScene(nextScene);
    }
}
