using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoClicavel : MonoBehaviour
{
    public void AoClickar()
    {
        AudioController.Instance.TocarSFX(2);
    }

}
