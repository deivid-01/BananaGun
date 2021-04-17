// --------------------------------------------------------------------------
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
/// Se encarga de todo el movimiento de la banana en escena
/// </summary>
public class BananaMovement : MonoBehaviour
{
    //Velocidad de rotación
    public float speedRotation = 15;
    //Velocidad de translación
    public float speedTranslate = 10;
    //Posición inicial
    Vector3 originPosition;
 /// <summary>
 /// Metodo por defecto de Unity que se corrre al iniciar el juego
 /// </summary>
    private void Start()
    {
        //se asigna la posición inicial de la banana
        originPosition = transform.position;
        //Se suscribe al evento para saber cuando están disparando
        GameEvent.instance.OnShooting += Recoil;
        
    }
    /// <summary>
    /// Aplica retroceso cuando la banana dispara
    /// </summary>
    void Recoil(Vector3 v)
    {
        //La rota hacia arriba en una posición aleatoria entre 20 y 40 grados
        transform.Rotate(Vector3.left* Random.Range(20f, 40f));
        //La translada hacia atras y un poco hacia arriba de forma aleatoria
        transform.Translate(new Vector3(0, Random.Range(0.1f, 0.5f),-Random.Range(0.3f, 1.0f)));
    }
    /// <summary>
    /// Metodo por defecto de Unity que se ejecuta cuando el objeto es destruido
    /// </summary>
    private void OnDestroy()
    {
       //Cancela la suscripción para cuando se está disparando
        GameEvent.instance.OnShooting -= Recoil;
    }
    /// <summary>
    /// Método por defecto de Unity que se ejecuta cada Frame
    /// </summary>
    void Update()
    {
        //Si no esta en el origen
        if(transform.position.x!=originPosition.x)
            //La mueve suavemnete al origen
            transform.position =Vector3.Lerp(transform.position,originPosition, speedTranslate * Time.deltaTime);
        //Aplica una rotación suave para que siga la posición del apuntador
        SmoothLookAt( FindTarget() );
    }
    /// <summary>
    /// Encuentra la posición exacta del mouse o del apuntador si la cámara esta encendida
    /// </summary>
    /// <returns></returns>
    Vector3 FindTarget()
    {
        //Optiene la posición del mouse o del apuntador en 2D
        Vector2 pos = (UIControls.mouseController) ? Input.mousePosition : new Vector3(Pointer.instance.rect.anchoredPosition.x, 720 + Pointer.instance.rect.anchoredPosition.y);
    
        //rayo que se dispara en el mundo 3d para saber la posición en 3D de la posicióm
        Ray ray = Camera.main.ScreenPointToRay(pos);
        //Se retorna la dirección del objtivo al que se apunta
        return (Physics.Raycast(ray, out RaycastHit hit, 100)) ? hit.point : ray.direction * 100;
    }
    /// <summary>
    /// Aplica una rotación suave a la banana
    /// </summary>
    /// <param name="target">La dirección objetivo a la que tiene que apuntar</param>
    void SmoothLookAt(Vector3 target)
    {
        //Rotación inicial
        Quaternion originalRot = transform.rotation;
        //Se asigna la posición a la que debe observar
        transform.LookAt(target);
        //Nueva rotación
        Quaternion newRot = transform.rotation;
        //Rotación previa
        transform.rotation = originalRot;
        //De la rotación previa apuntará suavemente a la nueva
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, speedRotation * Time.deltaTime);
    }


}
