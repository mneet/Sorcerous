using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int score = 0;

    public static GameManager Instance;

    public bool gameEnded = false;

    private HUDManager hudManager;
    public void ScorePoint(int point) {
        score += point;
        hudManager.UpdateScore(score);

        if (score >= 30) {
            hudManager.ActivateVictory();
            gameEnded = true;
        }
    }

    public void UpdateHealthUI(int health) {
        hudManager.UpdatePlayerHP(health);
        if (health <= 0) {
            hudManager.ActivateDefeat();
            gameEnded = true;
        }
    }

    public void RestartGame() {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(0);
    }

    private void Awake() {
        hudManager = gameObject.GetComponent<HUDManager>();
        Instance = this;
    }
}
