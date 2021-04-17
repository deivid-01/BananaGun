
using UnityEngine; // Importación de la libreria de  de Unity
using UnityEngine.UI;  // Importación de la libreria de interfaz de usuario de Unity
/// <summary>
/// Asegura que la camara siempre este puesta en escena
/// </summary>
public class OpenCvTools : MonoBehaviour
{
    //Imagen donde se va a mostra la camara en escena
    public static RawImage rawImage;

    #region Singlenton
    public static OpenCvTools instance;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Pone la texture la camara en el contenedor donde se muestra en escena
    /// </summary>
    public static  void  SetCameraTexture(ref WebCamTexture webCamTexture)
    {
        //Listado de todas las camaras
        WebCamDevice[] devices = WebCamTexture.devices;
        //Pone por defecto la camara principal del computador
        webCamTexture  =  new WebCamTexture(devices[0].name);
        //Activa la camara
        webCamTexture.Play();
    }

    #endregion


}
