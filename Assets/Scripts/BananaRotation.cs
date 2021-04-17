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
/// Administra la rotación de la banana en la escena principal
/// </summary>
public class BananaRotation : MonoBehaviour
{
    // velocidad de rotación
    public float speed;
    

/// <summary>
/// Se ejecuta cada frame
/// </summary>
    // Update is called once per frame
    void Update()
    {
       //Se rota solo el eje Y
        transform.Rotate(Vector3.up * speed*Time.deltaTime);
    }
}
