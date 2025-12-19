using System.Collections;
using UnityEngine;

public class EnemySys : MonoBehaviour
{
    public EnemyCurrentAction _enemyCurrentAction;
    //public EnemyAggression _enemyAggression;

    public int _index;
    public GameObject _player;
    public GameObject _target;
    public GameObject _enemyObject;
    public Animation _animation;
    //public Vector2 _vagrancyDelay;
    //public Vector3 _enemyStartPos;
    public float _moveSpeed;
    public float _rotatingSpeed;
    public float _distanceForAttack;
    //public float _randomX;
    //public float _randomZ;
    //public float _radiusForCheckPlayerPos = 10f;
    public float _attackDelay;
    public Coroutine _attackProcess;
    public EnemiesManager.EnemyType _type;
    //public Coroutine _checkVagrancyPointProcess;
    public GameObject _deathEffect;

    public pAttribute[] _attributes;

    //GameObject _currentVagrancyPoint;
    //Coroutine _vagrancyProcess;
    //Coroutine _checkPlayerPos;
    bool _isStunned;
    float _stunDuration;
    HealthSys hs;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _target = _player;
        hs = GetComponent<HealthSys>();
        //_enemyStartPos = transform.position;
    }

    public enum EnemyCurrentAction
    {
        Idle,
        Movement,
        Attacking
    }

    //public enum EnemyAggression
    //{
    //    Neutral,
    //    Aggressive
    //}

    //public WhenAggressionStarting _whenAggressionStarting;
    //public enum WhenAggressionStarting
    //{
    //    WhenTakingDamage,
    //    WhenPlayerSeen
    //}

    void Start()
    {
        //StartCoroutine(GetStunProcess());
        _attackProcess = StartCoroutine(AttackProcess());
        _moveSpeed = _attributes[2].value;
        //ChangeAggressionStatus(EnemyAggression.Neutral);
        //WhenAggressionStartingAction(_whenAggressionStarting);
    }

    public void Update()
    {
        if (!hs._isDead && (GameManager.single._gameStatus == GameManager.GameStatus.Playing || GameManager.single._gameStatus == GameManager.GameStatus.BossBattle))
        {
            if (_target != null)
            {
                if (_enemyCurrentAction != EnemyCurrentAction.Attacking)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _moveSpeed);
                    ChangeCurrentAction(EnemyCurrentAction.Movement);
                }

                var lookPos = _target.transform.position - _enemyObject.transform.position;

                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);

                _enemyObject.transform.rotation = Quaternion.Lerp(_enemyObject.transform.rotation, rotation, _rotatingSpeed * Time.deltaTime);
            }
        }
    }

    public void ShowDeathEffect()
    {
        GameObject eff = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(eff, .2f);
    }

    public void ChangeCurrentAction(EnemyCurrentAction eca)
    {
        _enemyCurrentAction = eca;
        if (_animation != null)
        {
            switch (_enemyCurrentAction)
            {
                case EnemyCurrentAction.Idle:
                    //_enemyAnimations.SetTrigger("Idle");
                    break;
                case EnemyCurrentAction.Movement:
                    _animation.Play("Armature|Run");
                    break;
                case EnemyCurrentAction.Attacking:
                    _animation.Play("Armature|Attack");
                    break;
            }
        }
    }

    //public void CheckAvailableOfMovement()
    //{
    //    if (_isStunned)
    //    {
    //        //_moveSpeed = 0f
    //    }
    //    else
    //    {
    //        //_moveSpeed = GetComponent<EnemyParameters>()._moveSpeed;

    //    }
    //}

    //public void WhenAggressionStartingAction(WhenAggressionStarting was)
    //{
    //    switch (was)
    //    {
    //        case WhenAggressionStarting.WhenTakingDamage:
    //            break;
    //        case WhenAggressionStarting.WhenPlayerSeen:
    //            _checkPlayerPos = StartCoroutine(CheckPlayerPos());
    //            break;
    //    }
    //}

    //public void ChangeAggressionStatus(EnemyAggression ea)
    //{
    //    _enemyAggression = ea;

    //    switch (_enemyAggression)
    //    {
    //        case EnemyAggression.Neutral:
    //            _vagrancyProcess = StartCoroutine(VagrancyProcess());
    //            break;

    //        case EnemyAggression.Aggressive:
    //            if (_vagrancyProcess != null)
    //            {
    //                StopCoroutine(_vagrancyProcess);
    //                Destroy(_currentVagrancyPoint);
    //                _currentVagrancyPoint = null;
    //            }

    //            if (_attackProcess == null)
    //                _attackProcess = StartCoroutine(AttackProcess());

    //            _target = _player;
    //            break;
    //    }
    //}

    //public void GetStun(float stun)
    //{
    //    _stunDuration = stun;
    //}

    //public IEnumerator VagrancyProcess()
    //{
    //    while (true)
    //    {
    //        if (_enemyAggression == EnemyAggression.Neutral)
    //        {
    //            float addX = Random.Range(_randomX, -_randomX);
    //            float addZ = Random.Range(_randomZ, -_randomZ);

    //            Vector3 position = new Vector3(_enemyStartPos.x + addX, _enemyStartPos.y, _enemyStartPos.z + addZ);
    //            GameObject point = new GameObject();
    //            point.transform.position = position;
    //            _currentVagrancyPoint = point;
    //            _target = _currentVagrancyPoint;
    //            ChangeCurrentAction(EnemyCurrentAction.Movement);
    //            if (_checkVagrancyPointProcess == null)
    //                _checkVagrancyPointProcess = StartCoroutine(CheckVagrancyPointProcess());
    //        }

    //        yield return new WaitForSeconds(Random.Range(_vagrancyDelay.x, _vagrancyDelay.y));

    //        Destroy(_currentVagrancyPoint);
    //        _currentVagrancyPoint = null;
    //    }
    //}

    //public IEnumerator CheckVagrancyPointProcess()
    //{
    //    while(true)
    //    {
    //        if (_enemyAggression == EnemyAggression.Neutral)
    //        {
    //            if (Vector3.Distance(_target.transform.position, transform.position) < .2f)
    //            {
    //                ChangeCurrentAction(EnemyCurrentAction.Idle);
    //                _target = null;
    //                break;
    //            }
    //        }
    //        else
    //        {
    //            _target = null;
    //            break;
    //        }

    //        yield return null;
    //    }

    //    _checkVagrancyPointProcess = null;
    //}

    //public IEnumerator CheckPlayerPos()
    //{
    //    bool playerSeen = false;
    //    while (!playerSeen)
    //    {
    //        if (Vector3.Distance(_player.transform.position, transform.position) < _radiusForCheckPlayerPos)
    //            playerSeen = true;
    //        yield return null;
    //    }

    //    ChangeAggressionStatus(EnemyAggression.Aggressive);
    //    _checkPlayerPos = null;
    //}

    public IEnumerator AttackProcess()
    {
        while (!hs._isDead)
        {
            if (GameManager.single._gameStatus == GameManager.GameStatus.Playing || GameManager.single._gameStatus == GameManager.GameStatus.BossBattle)
            {
                if (Vector3.Distance(_player.transform.position, transform.position) <= _distanceForAttack)
                {
                    ChangeCurrentAction(EnemyCurrentAction.Attacking);
                }
                else
                {
                    ChangeCurrentAction(EnemyCurrentAction.Movement);
                }

                if (_enemyCurrentAction == EnemyCurrentAction.Attacking)
                {
                    yield return new WaitForSeconds(_attackDelay / 2);
                    if (!GetComponent<HealthSys>()._isDead)
                    {
                        _player.GetComponent<HealthSys>().ChangeHealth(-_attributes[1].value);
                        AudioManager.single.Play("slap");
                    }
                    yield return new WaitForSeconds(_attackDelay / 2);
                }
            }
            yield return null;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (_whenAggressionStarting == WhenAggressionStarting.WhenPlayerSeen)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(transform.position, _radiusForCheckPlayerPos);
    //    }
    //}

    //public IEnumerator GetStunProcess()
    //{
    //    while (true)
    //    {
    //        if (_stunDuration <= 0)
    //        {
    //            _isStunned = false;
    //            _stunDuration = 0;
    //        }
    //        else
    //        {
    //            _isStunned = true;
    //            _nma.isStopped = true;
    //            _stunDuration -= Time.deltaTime;
    //        }

    //        yield return null;
    //    }
    //}

}
