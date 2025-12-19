using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager single;
    public int _angelicEssence;
    public GameStatus _gameStatus;
    public enum GameStatus
    {
        None,
        PreStart,
        Playing,
        Reincornation,
        Rebirth,
        GameOver,
        StartBossBattle,
        BossBattle,
        EndGame
    }

    private void Awake()
    {
        if (single == null)
        {
            single = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeGameStatus(GameStatus gs)
    {
        _gameStatus = gs;

        switch (_gameStatus)
        {
            case GameStatus.PreStart:
                StartCoroutine(ChangeSceneProcess(1));
                StartCoroutine(PreStartProcess());
                break;
            case GameStatus.Playing:
                break;
            case GameStatus.Reincornation:
                if (PlayerSys.single != null)
                {
                    SpawnerSys.single.DestroyAllEnemies();
                    PlayerSys.single.StartReincarnationProcess();
                }
                break;
            case GameStatus.Rebirth:
                ChangeGameSpeed(1f);
                YG2.onCloseAnyAdv -= Rebirth;
                SaveSys.single.ResetCharacter();
                StartCoroutine(ChangeSceneProcess(2));
                break;
            case GameStatus.GameOver:
                if (SpawnerSys.single != null)
                {
                    SpawnerSys.single.DestroyAllEnemies();
                }
                break;
            case GameStatus.StartBossBattle:
                if (SpawnerSys.single != null)
                {
                    SpawnerSys.single.DestroyAllEnemies();
                    SpawnerSys.single.BossSpawn();
                    InterfaceSys.single.ReshowEnemyHealth();
                    ChangeGameStatus(GameStatus.BossBattle);
                }
                break;
            case GameStatus.EndGame:
                InterfaceSys.single.ShowEndGamePanel();
                break;
        }
    }

    public void Rebirth()
    {
        ChangeGameStatus(GameStatus.Rebirth);
    }

    public void ChangeAngelicEssenceAmount(int value)
    {
        _angelicEssence += value;
    }

    public void ChangeGameSpeed(float value = -1)
    {
        float speed = Time.timeScale;
        speed++;

        if (value > 0)
        {
            speed = value;
        }

        if (speed < 5)
        {
            Time.timeScale = speed;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public IEnumerator ChangeSceneProcess(int index)
    {
        AudioManager.single.Stop("battle_music" + AudioManager.single._currentBattleMusic, true);
        AudioManager.single.Stop("rain", true);
        BlackScreenSys.single.Show(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }

    public IEnumerator PreStartProcess()
    {
        yield return new WaitForSeconds(1f);

        AudioManager.single.Play("rain", true);

        string add = "";
        if (Random.value > .5f)
            add = "1";

        AudioManager.single.Play("battle_music" + add, true);
        AudioManager.single._currentBattleMusic = add;
        YG2.GameplayStart();
    }
}
