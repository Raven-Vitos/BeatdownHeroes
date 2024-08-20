using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCutSceneDead : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject btn;

    [SerializeField]
    private GameObject score;

    public void StartCutScene()
    {
        animator.SetBool("isDead", true);
    }

    public void ShowData()
    {
        btn.SetActive(true);
        score.SetActive(true);
        score.GetComponent<Text>().text += GameData.playerScore.ToString();
    }
}
