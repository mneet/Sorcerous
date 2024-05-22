using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private void Start()
    {
        AudioController.Instance.TocarBGMusic(0);
    }
    public void AbrirConfiguracoes(){

    }
    public void CarregarCena(int idCena){
         SceneManager.LoadScene(idCena);
    }
}
