using UnityEngine;

public abstract class BaseModeController : MonoBehaviour
{
    public abstract void InitMode();

    public abstract void StartGame();

    public abstract void QuitGame();

    public abstract void EndGame(bool isWin);

    public abstract void ReplayGame();

    public abstract void OnPlayerRevive();

    public abstract void OnPlayerDie();

    public abstract BaseEnemy GetEnemyPrefab(int id);

    public virtual void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public virtual void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
