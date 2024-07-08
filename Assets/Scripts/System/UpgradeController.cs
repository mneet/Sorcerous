using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeController : MonoBehaviour
{
    [SerializeField] private Button fireButton;
    [SerializeField] private Button waterButton;
    [SerializeField] private Button earthButton;
    [SerializeField] private Button windButton;

    [SerializeField] private TextMeshProUGUI fireButtonText;
    [SerializeField] private TextMeshProUGUI waterButtonText;
    [SerializeField] private TextMeshProUGUI earthButtonText;
    [SerializeField] private TextMeshProUGUI windButtonText;

    [SerializeField] private GameObject player;

    [SerializeField] public int fireLevel = 0;
    [SerializeField] public int waterLevel = 0;
    [SerializeField] public int earthLevel = 0;
    [SerializeField] public int windLevel = 0;

    [SerializeField] public int fireLevelMax = 3;
    [SerializeField] public int waterLevelMax = 3;
    [SerializeField] public int earthLevelMax = 3;
    [SerializeField] public int windLevelMax = 3;

    public void upgradeFire() {
        Stats playerStats = player.GetComponent<Stats>();

        if (playerStats != null && fireLevel < fireLevelMax) {
            fireLevel++;
            playerStats.fireRate -= playerStats.fireRate * 0.15f;
            returnToGame();
        }
    }

    public void upgradeWater() {
        Stats playerStats = player.GetComponent<Stats>();
        HealthComponent playerHealthComponent = player.GetComponent<HealthComponent>();

        if (playerStats != null && waterLevel < waterLevelMax) {
            Debug.Log(playerStats.maxHealth * 0.25f);
            playerHealthComponent.TakeHeal(playerStats.maxHealth * 0.25f);
            waterLevel++;
            returnToGame();
        }     
    }

    public void upgradeEarth() {
        Stats playerStats = player.GetComponent<Stats>();

        if (playerStats != null && earthLevel < earthLevelMax) {
            earthLevel++;
            playerStats.armor += 5;
            returnToGame();
        }
    }

    public void upgradeWind() {
        Stats playerStats = player.GetComponent<Stats>();

        if (playerStats != null && windLevel < windLevelMax) {
            windLevel++;
            playerStats.movementSpeed += playerStats.movementSpeed * 0.20f;
            returnToGame();
        }
    }

    private void updateCards() {
        if (fireLevel >= fireLevelMax) {
            fireButtonText.text = "NÍVEL MÁXIMO ALCANÇADO";
        }
        if (earthLevel >= earthLevelMax) {
            earthButtonText.text = "NÍVEL MÁXIMO ALCANÇADO";
        }
        if (windLevel >= windLevelMax) {
            windButtonText.text = "NÍVEL MÁXIMO ALCANÇADO";
        }
        if (waterLevel >= waterLevelMax) {
            waterButtonText.text = "ESGOTADO";
        }
        
    }

    private void returnToGame() {        
        GameManager.Instance.PauseGame();
        updateCards();
        gameObject.SetActive(false);
    }
}
