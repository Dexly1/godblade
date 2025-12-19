using System.Collections;
using UnityEngine;

public class DetectorSys : MonoBehaviour
{
    public float _radius;
    public GameObject _neariestEnemy;
    public LayerMask _enemyLayer;
    public Coroutine _detectingProcess;

    void Start()
    {
        DetectingStatus(true);
    }

    public void DetectingStatus(bool value)
    {
        if (value)
        {
            _detectingProcess = StartCoroutine(DetectingProcess());
        }
        else
        {
            StopCoroutine(_detectingProcess);
        }
    }

    public IEnumerator DetectingProcess()
    {
        while(true)
        {
            if (_neariestEnemy == null)
                FindEnemy();

            yield return null;
        }
    }

    public void FindEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, Mathf.Infinity, _enemyLayer);

        if (enemies.Length > 0)
        {
            GameObject target = null;

            for (int i = 0; i < enemies.Length; i++)
            {
                if (!enemies[i].GetComponent<HealthSys>()._isDead)
                {
                    if (target == null)
                    {
                        target = enemies[i].gameObject;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(transform.position, enemies[i].gameObject.transform.position))
                        {
                            target = enemies[i].gameObject;
                        }
                    }
                }
            }

            if (target != null)
                _neariestEnemy = target;
        }
    }
}
