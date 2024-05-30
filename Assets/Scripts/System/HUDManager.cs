using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Text score;
    [SerializeField] private Text playerHP;

    public void UpdateScore(int newScore) {
        score.text = $"PONTOS: {newScore}";
    }
    public void UpdatePlayerHP(int hp) {
        playerHP.text = $"VIDA: {hp}";
    }

    public void ActivateVictory() {
        victoryPanel.SetActive(true);
    }

    public void ActivateDefeat() {
        defeatPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
