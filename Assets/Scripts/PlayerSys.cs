using System.Collections;
using UnityEngine;

public class PlayerSys : MonoBehaviour
{
    public static PlayerSys single;
    public GameObject _playerObject;
    public Animation _reincarnationEffect;
    public Animation _destroyEffect;
    public Actions _currentAction;

    public enum Actions
    {
        Waiting,
        GoToAttackTarget,
        Attacking,
        Death
    }

    public PlayerMovement pm;
    public PlayerAnimations pa;
    public DetectorSys ds;
    public CombatSys cs;
    public Coroutine _checkAttackTargetProcess;
    public Coroutine _startReincarnationProcess;

    private void Awake()
    {
        single = this;
    }

    IEnumerator Start()
    {
        pm = GetComponent<PlayerMovement>();
        pa = GetComponent<PlayerAnimations>();
        ds = GetComponent<DetectorSys>();
        cs = GetComponent<CombatSys>();
        StartCheckAttackTarget(true);
        yield return new WaitForEndOfFrame();
        Showing(true);
    }

    public void ChangeCurrentAction(Actions action)
    {
        _currentAction = action;
        switch (_currentAction)
        {
            case Actions.Waiting:
                pa.ChangeAnimation(PlayerAnimations.AnimationType.Idle);
                break;
            case Actions.GoToAttackTarget:
                pm.ChangeMovementStatus(PlayerMovement.MovementStatus.Free);
                pm.SetCurrentTarget(ds._neariestEnemy);
                cs._currentTarget = ds._neariestEnemy;
                pa.ChangeAnimation(PlayerAnimations.AnimationType.Run);
                break;
            case Actions.Attacking:
                pm.ChangeMovementStatus(PlayerMovement.MovementStatus.Locked);      
                pa.ChangeAnimation(PlayerAnimations.AnimationType.Attack);
                break;
        }
    }

    public void AfterKillingEnemy()
    {
        ds.FindEnemy();

        if (ds._neariestEnemy != null)
        {
            ChangeCurrentAction(Actions.GoToAttackTarget);
        }
        else
        {
            ChangeCurrentAction(Actions.Waiting);
        }
    }

    public void StartCheckAttackTarget(bool value)
    {
        if (value)
        {
            _checkAttackTargetProcess = StartCoroutine(CheckAttackTargetProcess());
        }
        else
        {
            StopCoroutine(_checkAttackTargetProcess);
        }
    }

    public void StartReincarnationProcess()
    {
        if (_startReincarnationProcess == null)
            _startReincarnationProcess = StartCoroutine(ReincarnationProcess());

    }

    public IEnumerator CheckAttackTargetProcess()
    {
        while(true)
        {
            if (_currentAction == Actions.Waiting)
            {
                if (ds._neariestEnemy != null)
                {
                    ChangeCurrentAction(Actions.GoToAttackTarget);
                }
            }

            if (_currentAction == Actions.GoToAttackTarget)
            {
                if (ds._neariestEnemy != null)
                {
                    if (Vector3.Distance(transform.position, ds._neariestEnemy.transform.position) < cs._distanceForAttack)
                    {
                        ChangeCurrentAction(Actions.Attacking);
                    }
                }
            }

            yield return null;
        }
    }

    public void DestroyEffect()
    {
        _destroyEffect.Play("destroy");
    }

    public void Death()
    {
        _playerObject.SetActive(false);
        _reincarnationEffect.Play("death");
        ChangeCurrentAction(Actions.Death);

        if (InterfaceSys.single._bossBattleButtonWasEnabled)
        {
            InterfaceSys.single.ShowBossBattleButton(false);

            if (InterfaceSys.single._bossBattlePanel.activeSelf == true)
                InterfaceSys.single.ShowBossBattlePanel();
        }
    }

    public IEnumerator ReincarnationProcess()
    {
        Death();
        cs._currentTarget = null;
        yield return new WaitForSeconds(2f);
        PlayerAttributes.single.ResetExpAfterDeath();
        pa.ChangeAnimation(PlayerAnimations.AnimationType.Idle);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _playerObject.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(.8f);
        Showing();
        _startReincarnationProcess = null;
    }

    public void Showing(bool delay = false)
    {
        StartCoroutine(ShowingProcess(delay));
    }

    public IEnumerator ShowingProcess(bool delay)
    {
        PlayerAttributes.single.SetBaseParameters();
        PlayerAttributes.single.SetBonusesToParameters();

        if (delay)
            yield return new WaitForSeconds(1f);
        _reincarnationEffect.Play("reinc");
        AudioManager.single.Play("appear");
        yield return new WaitForSeconds(.3f);
        _playerObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        GetComponent<HealthSys>()._isDead = false;
        AfterKillingEnemy();
        GameManager.single.ChangeGameStatus(GameManager.GameStatus.Playing);
        ChangeCurrentAction(Actions.Waiting);

        if (InterfaceSys.single._bossBattleButtonWasEnabled)
        {
            InterfaceSys.single.ShowBossBattleButton(true);
        }
    }
}
