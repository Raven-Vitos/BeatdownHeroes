using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int damage = 0; // Урон, наносимый врагам

    [SerializeField]
    GameObject EnemyOwener;

    private void Start()
    {
        damage = EnemyOwener.GetComponent<EnemyAI>().Damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (EnemyOwener.GetComponent<EnemyAI>().isDie)
        {
            gameObject.SetActive(false);
            return;
        }

        // Проверяем, если объект - Герой
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }


}
