using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 0; // ����, ��������� ������

    [SerializeField]
    GameObject PalyerOwener;

    private void Start()
    {
        damage = PalyerOwener.GetComponent<PlayerController>().Damage;
    }

    void OnTriggerEnter(Collider other)
    {
        // ���������, ���� ������ - ����
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemyHealth = other.GetComponent<EnemyAI>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
