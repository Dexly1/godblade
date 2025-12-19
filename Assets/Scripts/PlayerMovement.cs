using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float _moveSpeed;
    public float _rotatingSpeed;
    public GameObject _object;
    public GameObject _currentTarget;

    public MovementStatus _movementStatus;

    public enum MovementStatus
    {
        Free,
        Locked
    }

    void Start()
    {
        ChangeMovementStatus(MovementStatus.Free);
    }

    public void Update()
    {
        if (GameManager.single._gameStatus == GameManager.GameStatus.Playing || GameManager.single._gameStatus == GameManager.GameStatus.BossBattle)
        {
            if (_currentTarget != null)
            {
                var lookPos = _currentTarget.transform.position - _object.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);

                _object.transform.rotation = Quaternion.Lerp(_object.transform.rotation, rotation, _rotatingSpeed * Time.deltaTime);

                if (_movementStatus == MovementStatus.Free)
                    transform.position = Vector3.MoveTowards(transform.position, _currentTarget.transform.position, Time.deltaTime * _moveSpeed);
            }
        }
    }

    public void ChangeMovementStatus(MovementStatus mt)
    {
        _movementStatus = mt;

        switch(_movementStatus)
        {
            case MovementStatus.Free:
                break;
            case MovementStatus.Locked:
                break;
        }
    }

    public void SetCurrentTarget(GameObject target)
    {
        _currentTarget = target;
    }
}
