using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab; // ������ ����� ��������

    private GameObject healthBar;
    private RectTransform healthBarFill;
    private EnemyAI enemyHealth; // ������ �� ��������� �������� �����
    private Camera mainCamera;

    void Start()
    {
        // ������� ����� ��������
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity, transform);
        healthBar.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        healthBarFill = healthBar.transform.Find("HealthBarBackground").transform.Find("HealthBarFill").GetComponent<RectTransform>();
        enemyHealth = GetComponent<EnemyAI>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // ������� � ������� ������
        healthBar.transform.LookAt(healthBar.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);

        // ���������� ����� ��������
        float healthPercentage = enemyHealth.GetHealth() / 100.0f;
        healthBarFill.localScale = new Vector3(healthPercentage, 1, 1);

        if (healthBar.activeSelf && healthPercentage == 0)
        {
            healthBar.SetActive(false);
        }
    }
}
