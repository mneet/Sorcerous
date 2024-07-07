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

    [SerializeField] private int fireLevel = 0;
    [SerializeField] private int waterLevel = 0;
    [SerializeField] private int earthLevel = 0;
    [SerializeField] private int windLevel = 0;

    [SerializeField] private int fireLevelMax = 3;
    [SerializeField] private int waterLevelMax = 3;
    [SerializeField] private int earthLevelMax = 3;
    [SerializeField] private int windLevelMax = 3;

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
            playerHealthComponent.TakeHeal(playerStats.health += playerStats.maxHealth * 0.25f);
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
            earthLevel++;
            playerStats.movementSpeed += playerStats.movementSpeed * 0.20f;
            returnToGame();
        }
    }

    private void updateCards() {
        if (fireLevel >= fireLevelMax) {
            fireButtonText.text = "NÍVEL MÁXIMO ALCANÇADO";
        }
        if (earthLevel >= earthLevelMax) {
            fireButtonText.text = "NÍVEL MÁXIMO ALCANÇADO";
        }
        if (windLevel >= windLevelMax) {
            fireButtonText.text = "NÍVEL MÁXIMO ALCANÇADO";
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
