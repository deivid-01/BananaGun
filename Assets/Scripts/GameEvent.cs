﻿// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------
using UnityEngine;// Importación de la libreria principal de Unity
using System;//Libreria de funcionalidades basicas de C#
public class GameEvent : MonoBehaviour
{
    
    #region Singlenton
    public static GameEvent instance;
    ///
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(instance);
        }
    }
    #endregion
    // Todas las posibles acciones que ocurren durante el videojuego
    // Cada uno de los gameobject se puede suscribir a ellas para ser notificado cuando ocurran
    public event Action<Vector2> OnStartShoot;

    public event Action<Vector3> OnShooting;
    public event Action OnEnemyDestroyed;
    public event Action OnStartGame;
    public event Action OnRoundEnds;
    public event Action OnOptionSelected;
    public event Action<Vector2> OnMuzzelDetected;
    public event Action OnMuzzelNotDetected;
    public event Action OnLoadingNextScene;
    public event Action OnRestartingGame;
    public event Action OnEnemySuccess;
    public event Action <Color,string>OnChangeEnemyColor;


    //Metodos que invocan las acciones,son los triggers que se invocan con la instancia del gameObject
    public void Shooting(Vector3 direction) => OnShooting?.Invoke(direction);
    public void StartShoot(Vector2 position) => OnStartShoot?.Invoke(position);

    internal void EnemyDestroyed() => OnEnemyDestroyed?.Invoke();
    internal void StartGame() => OnStartGame?.Invoke();
    internal void RoundEnds() => OnRoundEnds?.Invoke();
    internal void OptionSelected() => OnOptionSelected?.Invoke();
    internal void MuzzelDetected(Vector2 position) => OnMuzzelDetected?.Invoke(position);
    internal void MuzzelNotDetected() => OnMuzzelNotDetected?.Invoke();
    internal void LoadingNextScene() => OnLoadingNextScene?.Invoke();
    internal void RestartingGame() => OnRestartingGame?.Invoke();
    internal void EnemySuccess() => OnEnemySuccess?.Invoke();
    internal void ChangeEnemyColor(Color color,string name) => OnChangeEnemyColor?.Invoke(color,name);

}
