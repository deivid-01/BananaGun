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
///  Controla cada uno de los Rounds del juego
/// </summary>
public class RoundSystem : MonoBehaviour
{
    //Numero del round actual
    public static int actualRound = 0;
    /// <summary>
    /// Metodo por defecto de Unity que se corre al iniciar el juego
    /// </summary>
    void Start()
    {
       ///Se Suscribe al evento para notificarle cuando el round termina
        GameEvent.instance.OnRoundEnds += RoundEnds;
    }
    /// <summary>
    /// Incrementa el valor del round actual
    /// </summary>
    public void RoundEnds()
    {
        //Cuando el round termina incrementa en uno
        actualRound += 1;
    }


}
