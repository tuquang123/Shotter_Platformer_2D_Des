using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class TriggerPointBoss : MonoBehaviour
{
    public BaseEnemy bossPrefab;
    public int levelInNormal = 1;
    public Collider2D lockWallLeft;
    public Collider2D lockWallRight;
    public Transform lockPointTop;
    public Transform playerStandPosition;
    public Transform spawnPosition;
    public Transform basePosition;
    public bool isAutoSpawnEnemy = false;

    protected BoxCollider2D sensor;


    void Awake()
    {
        sensor = GetComponent<BoxCollider2D>();
        lockWallLeft.gameObject.SetActive(false);
        lockWallRight.gameObject.SetActive(false);

        EventDispatcher.Instance.RegisterListener(EventID.WarningBossDone, (sender, param) => SpawnBoss());
    }

    void OnEnable()
    {
        ActiveSensor(true);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            ActiveSensor(false);
            Lock();
            UIController.Instance.hudBoss.WarningBoss();

            if (GameData.mode == GameMode.Campaign)
                ((CampaignModeController)GameController.Instance.modeController).IsAllowSpawnSideEnemy = isAutoSpawnEnemy;
        }
    }

    protected virtual void Lock()
    {
        lockWallLeft.gameObject.SetActive(true);
        lockWallRight.gameObject.SetActive(true);
        CameraFollow.Instance.SetMarginLeft(lockWallLeft.transform.position.x);
        CameraFollow.Instance.SetMarginRight(lockWallRight.transform.position.x);

        if (lockPointTop)
        {
            CameraFollow.Instance.SetMarginTop(lockPointTop.position.y);
        }
    }

    public void ActiveSensor(bool isActive)
    {
        sensor.enabled = isActive;
    }

    protected virtual void SpawnBoss()
    {
        if (gameObject.activeInHierarchy)
        {
            BaseEnemy boss = Instantiate(bossPrefab);

            boss.basePosition = basePosition.position;
            int level = GetLevel();
            boss.Active(bossPrefab.id, level, spawnPosition.position);
            boss.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(boss.gameObject, boss);
            UIController.Instance.hudBoss.UpdateHP(1f);
            SwitchMusic();
        }
    }

    protected int GetLevel()
    {
        int level = levelInNormal;

        if (GameData.mode == GameMode.Campaign)
        {
            if (GameData.currentStage.difficulty == Difficulty.Hard)
            {
                level += StaticValue.LEVEL_INCREASE_MODE_HARD;
            }
            else if (GameData.currentStage.difficulty == Difficulty.Crazy)
            {
                level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
            }
        }

        level = Mathf.Clamp(level, 1, StaticValue.MAX_LEVEL_ENEMY);

        return level;
    }

    protected void SwitchMusic()
    {
        if (GameData.mode == GameMode.Campaign)
        {
            StartCoroutine(CoroutineFadeMusic());
        }
    }

    private IEnumerator CoroutineFadeMusic()
    {
        float currentVolume = SoundManager.Instance.audioMusic.volume;
        WaitForSeconds waitFadeMusic = new WaitForSeconds(0.2f);

        while (SoundManager.Instance.audioMusic.volume > 0)
        {
            SoundManager.Instance.audioMusic.volume -= 0.2f;
            yield return waitFadeMusic;
        }

        SoundManager.Instance.audioMusic.volume = currentVolume;
        SoundManager.Instance.PlayMusic(StaticValue.SOUND_MUSIC_BOSS, 0f);
    }
}
