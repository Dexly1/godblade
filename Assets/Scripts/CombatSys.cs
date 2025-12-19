using System.Collections;
using UnityEngine;

public class CombatSys : MonoBehaviour
{
    public GameObject _currentTarget;
    public Attacks[] _attacks;
    public int _currentAttacks;
    public float _distanceForAttack;
    public Coroutine _attackProcess;
    public PlayerMovement pm;
    public PlayerSys ps;
    public HealthSys hs;
    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        ps = GetComponent<PlayerSys>();
        hs = GetComponent<HealthSys>();
        AttackProcess(true);
    }

    public void AttackProcess(bool value)
    {
        if (value)
            _attackProcess = StartCoroutine(AttackProcess());
        else
            StopCoroutine(_attackProcess);
    }

    public IEnumerator AttackProcess()
    {
        PlayerAttributes pattr = GetComponent<PlayerAttributes>();
        PlayerAnimations panim = GetComponent<PlayerAnimations>();
        CharacterAttribute dmg = pattr.FindAttribute(PlayerAttributes.AttributeType.MulPhysicalDamage);

        int attackNumber = 0;
        //float timer = 0;

        while (true)
        {
            if (GameManager.single._gameStatus == GameManager.GameStatus.Playing || GameManager.single._gameStatus == GameManager.GameStatus.BossBattle)
            {
                if (ps._currentAction == PlayerSys.Actions.Attacking)
                {
                    GameObject target = null;
                    if (_currentTarget != null)
                    {
                        if (target == null)
                        {
                            target = _currentTarget;
                        }
                    }
                    else
                        yield return null;

                    yield return null;

                    //panim.ChangeAnimation(PlayerAnimations.AnimationType.Attack);

                    //bool attacked = false;
                    //timer = 0;

                    //while (target != null)
                    //{
                    //    timer += Time.deltaTime;
                    //    Debug.Log(timer);

                    //    if (!attacked && timer >= _attacks[_currentAttacks].durationOfAttackBeforeDamage[attackNumber])
                    //    {
                    //        if (_currentTarget == target)
                    //        {
                    //            float chance = Random.Range(0f, 100f);
                    //            HealthSys.DamageType type = HealthSys.DamageType.Common;
                    //            float damage = dmg.currentValue;

                    //            if (chance < PlayerAttributes.single.FindAttribute(PlayerAttributes.AttributeType.CriticalDamageChance).currentValue)
                    //            {
                    //                damage *= 1f + (PlayerAttributes.single.FindAttribute(PlayerAttributes.AttributeType.CriticalDamageStrength).currentValue / 100f);
                    //                type = HealthSys.DamageType.Critical;
                    //            }

                    //            if (_currentTarget != null)
                    //                _currentTarget.GetComponent<HealthSys>().ChangeHealth(-Mathf.Round(damage), type);

                    //        }

                    //        attacked = true;
                    //        timer = 0;
                    //    }

                    //    if (attacked && timer >= _attacks[_currentAttacks].durationOfAttackAfterDamage[attackNumber])
                    //    {
                    //        attackNumber++;

                    //        if (attackNumber >= _attacks[_currentAttacks].durationOfAttackBeforeDamage.Length)
                    //        {
                    //            attackNumber = 0;
                    //        }

                    //        attacked = false;
                    //        timer = 0;
                    //    }

                    //    yield return null;
                    //}

                    //начать анимацию атаки
                    //panim.ChangeAnimation(PlayerAnimations.AnimationType.Attack, $"{attackNumber}");

                    panim.ChangeAnimation(PlayerAnimations.AnimationType.Attack);

                    yield return new WaitForSeconds(_attacks[_currentAttacks].durationOfAttackBeforeDamage[attackNumber]);

                    if (_currentTarget == target)
                    {
                        float chance = Random.Range(0f, 100f);
                        HealthSys.DamageType type = HealthSys.DamageType.Common;
                        float damage = dmg.currentValue;

                        if (chance < PlayerAttributes.single.FindAttribute(PlayerAttributes.AttributeType.CriticalDamageChance).currentValue)
                        {
                            damage *= 1f + (PlayerAttributes.single.FindAttribute(PlayerAttributes.AttributeType.CriticalDamageStrength).currentValue / 100f);
                            type = HealthSys.DamageType.Critical;
                        }

                        if (_currentTarget != null)
                        {
                            _currentTarget.GetComponent<HealthSys>().ChangeHealth(-Mathf.Round(damage), type);
                            AudioManager.single.Play("slash");
                        }

                        yield return new WaitForSeconds(_attacks[_currentAttacks].durationOfAttackAfterDamage[attackNumber]);

                        attackNumber++;
                        if (attackNumber >= _attacks[_currentAttacks].durationOfAttackBeforeDamage.Length)
                            attackNumber = 0;
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                yield return null;
            }

            yield return null;
        }

        //_attackProcess = null;
    }
}

[System.Serializable]
public class Attacks
{
    public float[] durationOfAttackBeforeDamage;
    public float[] durationOfAttackAfterDamage;
}
