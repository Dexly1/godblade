using System.Collections;
using TMPro;
using UnityEngine;

public class HealthSys : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;
    public float _regenerationHealth;
    public Coroutine _regenerationProcess;
    public bool _isPlayer;
    public bool _isDead;
    public float _yOffsetDmgText;
    public enum DamageType
    {
        Common,
        Critical,
        Regeneration
    }

    private void Start()
    {
        if (!_isPlayer)
        {
            _maxHealth = GetComponent<EnemySys>()._attributes[0].value;
            _currentHealth = _maxHealth;
        }

        if (_regenerationHealth > 0)
            RegenerationStatus(true);
    }

    public void RegenerationStatus(bool value)
    {
        if (value)
        {
            if (_regenerationProcess == null)
                _regenerationProcess = StartCoroutine(RegenerationProcess());
        }
        else
        {
            if (_regenerationProcess != null)
                StopCoroutine(_regenerationProcess);
        }
    }

    public void ChangeHealth(float value, DamageType type = DamageType.Common)
    {
        if (!_isDead)
        {
            _currentHealth += value;

            if (value < 0)
            {
                if (GetComponent<EnemySys>() != null)
                {
                    if (GetComponent<EnemySys>()._type == EnemiesManager.EnemyType.Boss)
                    {
                        GetComponent<EnemySys>()._attributes[1].value += 2;
                    }
                }
            }

            if (_currentHealth >= _maxHealth)
                _currentHealth = _maxHealth;

            if (_currentHealth <= 0)
            {
                _isDead = true;
                _currentHealth = 0;

                Debug.Log("Death");

                if (!_isPlayer)
                {
                    if (PlayerSys.single.ds._neariestEnemy == gameObject)
                        PlayerSys.single.ds._neariestEnemy = null;

                    PlayerSys.single.AfterKillingEnemy();
                    PlayerAttributes.single.TakeExperience(GetComponent<EnemySys>()._attributes[3].value);

                    if (GetComponent<EnemySys>()._type == EnemiesManager.EnemyType.Common)
                        QuestsManager.single._currentQuestsPack.ChangeValueCurrentQuest(1);
                    else
                    {
                        QuestsManager.single._currentQuestsPack.currentQuest.isComplete = true;
                        QuestsManager.single._currentQuestsPack.CheckQuest();
                    }

                    GetComponent<EnemySys>()._enemyObject.SetActive(false);
                    GetComponent<EnemySys>().ShowDeathEffect();

                    Destroy(gameObject, 1f);
                }
                else
                {
                    AudioManager.single.Play("death");

                    if (GameManager.single._gameStatus == GameManager.GameStatus.Playing)
                    {
                        GameManager.single.ChangeGameStatus(GameManager.GameStatus.Reincornation);
                    }
                    else
                    {
                        PlayerSys.single.Death();
                        SpawnerSys.single.DestroyAllEnemies();
                        InterfaceSys.single.ShowAbsoluteRebirthPanel();
                    }

                    InterfaceSys.single.UpdateHealthPanel(_currentHealth, _maxHealth);
                }
            }

            SendShowHealth(value, type);
        }
    }

    public IEnumerator RegenerationProcess()
    {
        float delay = 1;
        float value = 1;

        while (true)
        {
            if (GameManager.single._gameStatus == GameManager.GameStatus.Playing || GameManager.single._gameStatus == GameManager.GameStatus.BossBattle)
            {
                if (delay >= 0.02f)
                {
                    delay = 1f / _regenerationHealth;
                }
                else
                {
                    delay = 0.02f;
                    value = _regenerationHealth / 50f;
                }

                ChangeHealth(value, DamageType.Regeneration);

                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void SendShowHealth(float value, DamageType type)
    {
        if (_isPlayer)
        {
            InterfaceSys.single.UpdateHealthPanel(_currentHealth, _maxHealth);
        }
        else
        {
            if (type != DamageType.Regeneration)
            {
                InterfaceSys.single.ShowDamageText(transform.position, value, _yOffsetDmgText, type);
            }
        }
    }
}
