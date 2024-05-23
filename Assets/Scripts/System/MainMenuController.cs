using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    private bool pause = false;
    private void Start()
    {
        AudioController.Instance.TocarBGMusic(0);
    }
    public void AbrirConfiguracoes(){

    }
    public void CarregarCena(int idCena){
         SceneManager.LoadScene(idCena);
         AudioController.Instance.TocarBGMusic(1);
    }

    public void PauseGame() {
        pause = !pause;
        if (pause) {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
