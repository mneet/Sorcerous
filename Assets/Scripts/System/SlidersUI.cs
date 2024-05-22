using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersUI : MonoBehaviour
{
    public bool isSfx;
    public Text textoFeedbackValor;
    public Slider slider;


    void Awake()
    {
        if (isSfx)
            AtualizarTextoSliderVolumeSFX(AudioController.Instance.PegarVolumeSFXSalvo());
        else
            AtualizarTextoSliderVolumeBG(AudioController.Instance.PegarVolumeBGSalvo());
    }

    public void AtualizarTextoSliderVolumeBG(float valor)
    {
        textoFeedbackValor.text = (valor * 100f).ToString("F0") + "%";
        slider.value = valor;
        AudioController.Instance.AtualizarVolumeBG(valor);
    }

    public void AtualizarTextoSliderVolumeSFX(float valor)
    {
        textoFeedbackValor.text = (valor * 100f).ToString("F0") + "%";
        slider.value = valor;
        AudioController.Instance.AtualizarVolumeSFX(valor);

    }

}
