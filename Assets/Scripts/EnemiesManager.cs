using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager single;
    public EnemyPack[] _enemies;
    public enum EnemyType
    {
        Common,
        Boss
    }

    private void Awake()
    {
        single = this;
    }
}

[System.Serializable]
public class EnemyPack
{
    public Enemy[] enemy;
}

[System.Serializable]
public class Enemy
{
    public string name;
    public GameObject prefab;
    public Vector2 health;
    public float originDamage;
    public EnemiesManager.EnemyType type;
}
