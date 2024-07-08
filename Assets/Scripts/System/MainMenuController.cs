using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject helpMenu;
    [SerializeField] private GameObject creditMenu;
    [SerializeField] private GameObject mainMenu;

    private bool credit = false;
    private bool help = false;
    private bool pause = false;
    private void Start() {
        AudioController.Instance.TocarBGMusic(0);
    }

    public void CarregarCena(int idCena) {
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

    public void HelpMenu(bool active) {
        helpMenu.SetActive(active);
        mainMenu.SetActive(!active);
    }

    public void CreditMenu(bool active) {
        creditMenu.SetActive(active);
        mainMenu.SetActive(!active);
    }

    public void ExitGame() {
        Application.Quit();
    }
    //DEBUG
    private void ScreenShoot() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ScreenCapture.CaptureScreenshot("SomeLevel.png");
            Debug.Log("Screenshoot taken");
        }
    }

    private void Update() {
        ScreenShoot();
    }
}
