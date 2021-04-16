using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEvent : MonoBehaviour
{
    // Start is called before the first frame update

    #region Singlenton
    public static GameEvent instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(instance);
        }
    }
    #endregion
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

}
