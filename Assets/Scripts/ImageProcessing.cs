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
using OpenCvSharp; // Importación de la Libreria de OpenCv
using UnityEngine.UI; // Importación de la libreria de interfaz de usuario de Unity

/// <summary>
/// Clase que se encarga de controlar la camara y tomar cada snapshot que sera procesado
/// por la clase ObjectDetection
/// </summary>
/// 
public class ImageProcessing : MonoBehaviour
{
    //Objeto que contiene el contenedor para mostrar la imagen de fondo
    public RawImage rawImageCamera;
    //Tolerancia de movimiento de la bolita
    public int toleranceMuzzle;
    //Suma minima que debe tener la matriz con el lazer para ser detectado
    int umbralSum = 50000;

    //Objetos que se desean detectar
    public bool findMuzzle;
    public  bool findLazer;

    //Instancia de la clase pointer para obtener la posicion a la que esta apuntando
    Pointer pointer;
    //Textura de la camara
    WebCamTexture webCamTexture;


    /// <summary>
    /// Metodo que se corre por default de Unity al iniciar el juego
    /// </summary>
    private void Start()
    {
       //Se obtiene el Singlenton de la clase pointer
        pointer = Pointer.instance;

        //Si se va a jugar con el mouse 
        if (UIControls.mouseController)
        {           
            //Se desactiva este objeto
            gameObject.SetActive(false);
           
        }
        else
        {
            //Obtiene la camara principal del computador
            OpenCvTools.SetCameraTexture(ref webCamTexture);
            //Se asigna la textura de la camara al contenedor que la va a mostrar
            rawImageCamera.texture = webCamTexture;
         
            //Subscripción a las acciones para detener la camara
            GameEvent.instance.OnRoundEnds += StopCamera;
            GameEvent.instance.OnLoadingNextScene += StopCamera;
        }
    }
    /// <summary>
    /// Metodo por default de Unity que se ejecuta cuando el juego termina o se recarga la escena
    /// </summary>
    private void OnDestroy()
    {
        //Se detiene la cámara
        StopCamera();
        //Se cancela la suscripción al evento
        GameEvent.instance.OnRoundEnds -= StopCamera;
    }
    /// <summary>
    /// Metodo por default de Unity que se ejecuta cuando el juego termina
    /// </summary>
    private void OnApplicationQuit()
    {
        StopCamera();
    }

    /// <summary>
    /// Metodo por default de Unity que se ejecuta cada frame
    /// </summary>

    private void Update()
    {
        //Si la camara está encendida
        if (webCamTexture.isPlaying)
        {
           //Se toma el snapshot de la camara y se obtiene la matriz de la imagen
            Mat mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
            //Se convierte al espacio de colores Lab
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2Lab);

            //Si esta habilitada la opcion de encontrar la bolita
            if (findMuzzle)
            {
                // Se detecta y se obtiene ( si existe ) la posición del obeto
                //Se manda solo el canal 3, es decir (b)  ya que es el canal donde se puede notar más
                ObjectDetection.SetMuzzlePosition(mat.ExtractChannel(2).Flip(FlipMode.Y),
                                                toleranceMuzzle, 
                                                (int)pointer.rect.anchoredPosition.x, // Posicion del objeto con lo que se apunta en el eje X
                                               -(int)pointer.rect.anchoredPosition.y); // Posicion del objeto con lo que se apunta en el eje Y


            }
            //Si esta habilitada la opcion de encontrar el led
            if (findLazer)
            {
                //Se detecta si el lazer existe o no
                //Se manda el canal 2, es decir (a) ya que es el canal donde se puede notar mas el led
                if (ObjectDetection.DetectLazer( mat.ExtractChannel(1).Flip(FlipMode.Y), umbralSum))
                {
                    //Si detecta el lazer empieza a disparar en la posición que se esta apuntando
                    GameEvent.instance.StartShoot(new Vector2(pointer.rect.anchoredPosition.x, 720 + pointer.rect.anchoredPosition.y));
                }
            }
         
        }
    }
    /// <summary>
    /// Detiene la camara
    /// </summary>
    void StopCamera()
    {
        //Si la camara no existe no la detiene
        if (webCamTexture is null) return;
        //Si la camara esta encendida
        if (webCamTexture.isPlaying)
        {
            //Se detiene la camara
            webCamTexture.Stop();
        }
    }

   
}
