//----------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------

using UnityEngine;// Importación de la libreria principal de Unity
/// <summary>
/// Administra los puntos por cada Round
/// </summary>
public class PointsController : MonoBehaviour
{
    //Total de puntos del jugador

    public static int totalPoints;

    /// <summary>
    /// Metodo que se corre por default de Unity al iniciar el juego
    /// </summary>
    void Start()
    {
        // Se suscribe a los eventos
        GameEvent.instance.OnEnemyDestroyed += IncreasePoints;
        GameEvent.instance.OnEnemySuccess += DecreasePoints;
        GameEvent.instance.OnRestartingGame += ResetPoints;
    }
    /// <summary>
    /// Se ejecuta cuando el objeto se destruye
    /// </summary>
    private void OnDestroy()
    {
        // Cancela suscripción de los eventos
        GameEvent.instance.OnEnemyDestroyed -= IncreasePoints;
        GameEvent.instance.OnRestartingGame -= ResetPoints;
        GameEvent.instance.OnEnemySuccess -= DecreasePoints;
    }

    /// <summary>
    /// Aumenta los puntos
    /// </summary>
    void IncreasePoints()
    {
        totalPoints += 100;
       
    }
    /// <summary>
    /// Disminuye los puntos
    /// </summary>
    void DecreasePoints()
    {
       
        totalPoints -= 100;
        if (totalPoints < 0)
            totalPoints = 0;
    }
    /// <summary>
    /// Pone el total de puntos en cero
    /// </summary>
    void ResetPoints()
    {
        totalPoints = 0;
    }
}
