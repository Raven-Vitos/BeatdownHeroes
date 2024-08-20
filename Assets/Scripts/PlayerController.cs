using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField]
    private MobileJoystick joystick;

    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private float Health = 100.0f;
    [SerializeField]
    private float MaxHealth = 100.0f;

    [SerializeField]
    private int damage = 20;

    [SerializeField]
    private Transform Skeleton;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    private RectTransform healthBarFill;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        healthBarFill = GameObject.Find("HealthBarFill").GetComponent<RectTransform>();
    }

    public int Damage 
    {
        get {
            return damage;
        }
        set {
            damage = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(Vector3.down);

        // Получаем информацию о текущем состоянии анимации на первом слое
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Проверяем, текущая ли анимация атаки ногой 
        if (stateInfo.IsName("Kick_Attack"))
        {
            return;
        }

        // Получаем горизонтальные и вертикальные значения джойстика
        float moveX = joystick.Horizontal();
        float moveY = joystick.Vertical();

        if (moveX != 0.0f)
            Skeleton.localScale = new Vector3(Skeleton.localScale.x, Skeleton.localScale.y, moveX < 0.0f ? -1 : 1);

        // Направление движения
        Vector3 direction = new Vector3(moveX, 0, moveY);
        if (direction.magnitude > 0f)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        if (direction.magnitude > 1f)
        {
            direction = direction.normalized;
        }

        // Преобразуем направление относительно камеры
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0f;

        moveDirection = direction * moveSpeed;
        // Движение персонажа
        characterController.Move(moveDirection * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Space)) StartAttack();
    }

    public void StartAttack()
    {
        animator.SetTrigger("isKickAttack");
    }

    public void TakeDamage(int damage)
    {
        Health = (Health - damage) < 0 ? 0 : Health - damage;

        if (Health == 0)
        {
            Die();
        }

        float healthPercentage = Health / MaxHealth;
        healthBarFill.localScale = new Vector3(healthPercentage, 1, 1);

    }

    public void Die()
    {
        SceneManager.LoadScene("HeroDie");
    }
}
