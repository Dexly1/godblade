using System.Collections;
using UnityEngine;
using YG;

public class SpawnerSys : MonoBehaviour
{
    public static SpawnerSys single;
    public EnemiesManager em;
    public GameObject _player;
    public int _levelOfEnemies;
    public float _timeBeforeLevelUp;
    public Coroutine _levelOfEnemiesUpProcess;
    public Coroutine _spawnerProcess;
    public float _timer;
    public float _minDistanceForSpawn;
    public float _maxDistanceForSpawn;
    public float _delay;
    public float _maxDelay;
    public float _minDelay;
    public float _stepDelay;

    private void Awake()
    {
        single = this;
    }

    void Start()
    {
        em = EnemiesManager.single;
        _player = GameObject.FindGameObjectWithTag("Player");
        Invoke("StartSpawn", 1.5f);
    }

    public void StartSpawn()
    {
        _levelOfEnemiesUpProcess = StartCoroutine(LevelOfEnemiesUpPrcoess());
        _spawnerProcess = StartCoroutine(SpawnerProcess());
    }

    public void DestroyAllEnemies()
    {
        PlayerSys.single.DestroyEffect();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemies)
        {
            Destroy(e);
        }
    }

    public void BossSpawn()
    {
        StartCoroutine(BossSpawnProc());
    }

    public IEnumerator BossSpawnProc()
    {
        EnemyPack ep = em._enemies[QuestsManager.single._currentQuestPackIndex];
        Enemy qBoss = null;
        int index = -1;

        for (int i = 0; i < ep.enemy.Length; i++)
        {
            if (ep.enemy[i].type == EnemiesManager.EnemyType.Boss)
            {
                qBoss = ep.enemy[i];
                index = i;
            }
        }

        GameObject boss = Instantiate(qBoss.prefab, RandomPosition(), Quaternion.identity);
        EnemySys bs = boss.GetComponent<EnemySys>();

        bs._index = index;
        bs._attributes[0].value = qBoss.health.x;
        bs._attributes[1].value = qBoss.originDamage;
        bs._type = EnemiesManager.EnemyType.Boss;

        yield return new WaitForEndOfFrame();

        PlayerSys.single.AfterKillingEnemy();
    }

    public Vector3 RandomPosition()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-74, 74), 0f, Random.Range(-74, 74));

            if (Vector3.Distance(_player.transform.position, pos) > _minDistanceForSpawn &&
                Vector3.Distance(_player.transform.position, pos) < _maxDistanceForSpawn)
            {
                return pos;
            }
        }
    }

    public void GetLastTimeAndLevelAfterLevelUp()
    {
        YG2.saves.enemiesLevel = _levelOfEnemies;
        YG2.saves.enemiesSpawnerTimer = _timer;
        YG2.saves.enemiesSpawnerDelay = _delay;
    }

    public void SetLastTimeAndLevelAfterLevelUp()
    {
        _levelOfEnemies = YG2.saves.enemiesLevel;
        _timer = YG2.saves.enemiesSpawnerTimer;

        if (YG2.saves.enemiesSpawnerDelay > 0f)
        {
            _delay = YG2.saves.enemiesSpawnerDelay;
        }
        else
        {
            _delay = _maxDelay;
        }
    }

    public IEnumerator LevelOfEnemiesUpPrcoess()
    {
        SetLastTimeAndLevelAfterLevelUp();

        while (true)
        {
            if (GameManager.single._gameStatus == GameManager.GameStatus.Playing)
            {
                _timer += Time.deltaTime;

                if (_timer >= _timeBeforeLevelUp)
                {
                    _levelOfEnemies++;
                    _delay -= _stepDelay;
                    _delay = Mathf.Clamp(_delay, _minDelay, _maxDelay);
                    _timer = 0;
                }
            }

            yield return null;
        }
    }

    public IEnumerator SpawnerProcess()
    {
        while (true)
        {
            if (GameManager.single._gameStatus == GameManager.GameStatus.Playing)
            {
                Vector3 pos = RandomPosition();

                EnemyPack ep = em._enemies[QuestsManager.single._currentQuestPackIndex];
                int rand;

                while (true)
                {
                    rand = Random.Range(0, ep.enemy.Length);

                    if (ep.enemy[rand].type == EnemiesManager.EnemyType.Common)
                        break;

                }

                Enemy selEnemy = ep.enemy[rand];

                GameObject enemy = Instantiate(selEnemy.prefab, pos, Quaternion.identity);
                EnemySys es = enemy.GetComponent<EnemySys>();

                es._index = rand;
                es._attributes[0].value = Random.Range(selEnemy.health.x, selEnemy.health.y) * (_levelOfEnemies + 1);
                es._attributes[1].value = selEnemy.originDamage * (_levelOfEnemies + 1);
                es._type = EnemiesManager.EnemyType.Common;

                yield return new WaitForSeconds(_delay);
            }
            else
            {
                yield return null;
            }
        }
    }
}
