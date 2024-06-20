using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool pause = false;
    public int score = 0;
    [SerializeField] private int scoreWin = 200;

    public static GameManager Instance;

    public bool gameEnded = false;

    private HUDManager hudManager;

    public void ScorePoint(int point) {
        score += point;
        hudManager.UpdateScore(score);

        
        if (score >= scoreWin) {
            hudManager.ActivateVictory();
            gameEnded = true;
            Time.timeScale = 0f;
        }
        
    }

    public void UpdateHealthUI(float health) {
        hudManager.UpdatePlayerHP(health);
        if (health <= 0) {
            hudManager.ActivateDefeat();
            gameEnded = true;
            Time.timeScale = 0f;
        }
    }

    public void RestartGame() {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void PauseGame() {
        pause = !pause;
        if (pause) {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    private void Awake() {
        hudManager = gameObject.GetComponent<HUDManager>();
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }   
       
    }
    private void Start()
    {
        GameObject AudioMan = GameObject.Find("AudioManager");

        AudioMan.GetComponent<AudioController>().TocarBGMusic(1);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }
}
 
