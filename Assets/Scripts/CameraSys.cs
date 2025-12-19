using UnityEngine;

public class CameraSys : MonoBehaviour
{
    public GameObject _player;
    public float _speed;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //if (GameManager.single._gameState != GameManager.GameState.LevelCompleted)
            transform.position = Vector3.Lerp(transform.position, new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z - 12f), _speed * Time.deltaTime);
    }
}
