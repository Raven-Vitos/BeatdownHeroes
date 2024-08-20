using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, ICharacter
{
    private NavMeshAgent agent; // Ссылка на NavMeshAgent
    private Transform player;   // Ссылка на игрока
    private Animator animator;  // Ссылка на аниматор врага

    [SerializeField]
    private float attackRange = 1.0f;  // Дистанция атаки
    [SerializeField]
    private float attackCooldown = 1.0f; // Время между атаками
    [SerializeField]
    private float rotationSpeed = 5.0f; // Скорость поворота
    [SerializeField]
    private float Health = 100.0f;
    [SerializeField]
    private float dealayRespawn = 10.0f;
    [SerializeField]
    private int damage = 2;


    private bool isAttacking = false;   
    public bool isDie { get; set; }    

    public delegate void EventEnemyDied();
    public EventEnemyDied eventEnemyDied;
    public EventEnemyDied eventEnemyDestroy;

    void Start()
    {
        player = GameObject.Find("Hero").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    void Update()
    {
        if (isDie) return;

        // Поворот в сторону игрока
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (isAttacking) return;

        // Дистанция до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            // Если враг вне диапазона атаки, двигаться к игроку
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isWalk", true);
        }
        else
        {
            // Если враг в пределах диапазона атаки, атаковать
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }

    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetBool("isWalk", false);
        animator.SetTrigger("isAttack");

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;        
    }

    public void TakeDamage(int damage)
    {
        Health = (Health - damage) < 0 ? 0 : Health - damage;

        if (Health == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        StopCoroutine(AttackPlayer());

        isDie = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        animator.SetBool("isDie", true);

        CapsuleCollider colleder = GetComponent<CapsuleCollider>();
        colleder.enabled = false;

        eventEnemyDied();

        Invoke("Respawn", dealayRespawn);
    }

    public float GetHealth()
    {
        return Health;
    }

    public void Respawn()
    {
        eventEnemyDestroy();
        Destroy(gameObject);
    }
}
