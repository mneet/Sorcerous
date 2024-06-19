using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueData : MonoBehaviour
{
    public static ValueData Instance;

    public int scoreRecord;
    public int bossRecord;
    public int waveRecord;

    private void Awake() {

        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

    }
}
