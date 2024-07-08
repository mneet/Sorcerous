using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
   

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI waveCounter;
    [SerializeField] private Slider hpBar;
    public bool endgame = false;
    [SerializeField] private GameObject hud;

    [Header("ENDGAME")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI scoreValueRun;
    [SerializeField] private TextMeshProUGUI waveValueRun;
    [SerializeField] private TextMeshProUGUI bossValueRun;

    [SerializeField] private TextMeshProUGUI scoreValueRecord;
    [SerializeField] private TextMeshProUGUI waveValueRecord;
    [SerializeField] private TextMeshProUGUI bossValueRecord;


    public static HUDManager Instance;

    public void UpdateScore(int newScore) {
        score.text = $"{newScore}";

        if (newScore > ValueData.Instance.scoreRecord) {
            ValueData.Instance.scoreRecord = newScore;
        }
    }
    public void UpdateWave(int newWave) {
        waveCounter.text = $"{newWave}";

        if (newWave > ValueData.Instance.waveRecord) {
            ValueData.Instance.waveRecord = newWave;
        }
    }
    public void UpdatePlayerHP(float value) {
        hpBar.value = value;
    }

    private void ActivateEndGame() {
        endGamePanel.SetActive(true);
        hud.SetActive(false);

        scoreValueRun.text = GameManager.Instance.score.ToString();
        waveValueRun.text = WaveManager.Instance.waveCount.ToString();
        bossValueRun.text = WaveManager.Instance.waveCount.ToString();

        scoreValueRecord.text = ValueData.Instance.scoreRecord.ToString();
        waveValueRecord.text = ValueData.Instance.waveRecord.ToString();
        bossValueRecord.text = ValueData.Instance.bossRecord.ToString();

        endgame = true;
    }

    public void ActivateVictory() {
        ActivateEndGame();
    }

    public void ActivateDefeat() {
        ActivateEndGame();
    }

    public void RestartGame() {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void MainMenu() {
        Debug.Log("MENU LOAD");
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

    }
}
