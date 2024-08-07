using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    public Text txtScore;
    int score = 0;
    void updateScore()
    {
        score += 1;
        txtScore.text = "Score: " + score.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            SpawnStar.Instance.StartCoroutineRoutine();
            updateScore();
        }
    }
}
