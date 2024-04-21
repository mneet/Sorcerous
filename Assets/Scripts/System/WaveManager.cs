using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Perspective;
using static WaveManager;

public class WaveManager : MonoBehaviour
{
    // System
    [SerializeField] private Perspective perspectiveManager;
    // Round control variables
    private Round currentRound;
    private bool formationWaveSpawned = false;
    private bool runnerWaveSpawned = false;
    private List<GameObject> formationMobList = new List<GameObject>();
    private List<GameObject> runnerMobList = new List<GameObject>();

    // Generation variables
    [SerializeField] private GameObject enemyPreFab;
    private SpawnArea lastSpawnedArea;
    private int waveCountMax = 3;

    public enum waveOrientation {
        horizontal,
        vertical,
        random
    };
    [Serializable]
    public struct SpawnArea {
        public float centerAnchorX;
        public float centerAnchorY;
        public float width;
        public float height;
        public waveOrientation orientation;
    }

    // Structs
    public struct Wave {
        public bool waveSpawned;
        public List<WaveMob> waveMobs;
        public List<GameObject> mobList;

        public void InitVariables() {
            waveMobs = new List<WaveMob>();
            mobList = new List<GameObject>();
            waveSpawned = false;
        }
    }
    public struct Round {
        public Perspective.PerspectiveOptions perspective;
        public List<Wave> formationWaves;
        public List<Wave> runnerWaves;
        public int runnerWaveInd;
        public int formationWaveInd;
        public bool formationWaveFlag;
        public bool runnerWaveFlag;

        public void InitVariables() {
            formationWaves = new List<Wave>();
            runnerWaves = new List<Wave>();
            runnerWaveInd = 0;
            formationWaveInd = 0;
            formationWaveFlag = true;
            runnerWaveFlag = true;
        }
    }
    public struct WaveMob {
        public Vector3 position;
        public GameObject preFab;
        public bool formation;
        public Vector3 spawnPosition;
    }

    // Formation areas
    #region TopDown Areas
    [SerializeField] private SpawnArea topDownRight = new SpawnArea();
    [SerializeField] private SpawnArea topDownLeft = new SpawnArea();
    [SerializeField] private SpawnArea topDownCenter = new SpawnArea();
    private List<SpawnArea> topDownAreas;
    #endregion
    #region SideScroller Areas
    [SerializeField] private SpawnArea sideScrollerTop = new SpawnArea();
    [SerializeField] private SpawnArea sideScrollenCenter = new SpawnArea();
    [SerializeField] private SpawnArea sideScrollerDown = new SpawnArea();
    private List<SpawnArea> sideScrollerAreas;
    #endregion

    #region Generation Methods
    // Generates a new Round
    private Round RoundGenerator() {
        Round newRound = new Round();
        newRound.perspective = perspectiveManager.GetRandomPerspective();
        newRound.InitVariables();

        for (var i = 0; i < 3; i++) {
            newRound.formationWaves.Add(FormationWaveGenerator(newRound));
        }

        for (var i = 0; i < 3; i++) {
            newRound.runnerWaves.Add(RunnerWaveGenerator(newRound));
        }

        return newRound;
    }
    // Generates a wave with mobs in a fixed formation
    private Wave FormationWaveGenerator(Round round) {
        Wave wave = new Wave();
        wave.InitVariables();

        SpawnArea spawnArea = ChooseArea(round.perspective);

        float xOrigin = spawnArea.centerAnchorX;
        float yOrigin = spawnArea.centerAnchorY;

        for (var i = 0; i < 3; i++) {
            float mobX = 0;
            float mobY = 0;
            float lineDivision = 0f;
            float lineSize = 0f;

            switch (spawnArea.orientation) {
                case waveOrientation.horizontal:
                    lineDivision = spawnArea.width / 3;
                    mobX = xOrigin - (spawnArea.width / 3) + lineDivision * i + 1;
                    mobY = yOrigin;
                    break;

                case waveOrientation.vertical:
                    lineDivision = spawnArea.height / 3;
                    mobX = xOrigin;
                    mobY = yOrigin + (spawnArea.height / 3) - lineDivision * i + 1;
                    break;
            }
            Vector3 mobPosition = new Vector3();
            switch (round.perspective) {
                case PerspectiveOptions.topDown:
                    mobPosition = new Vector3(mobX, 0f, mobY);
                    break;
                case PerspectiveOptions.sideScroler:
                    mobPosition = new Vector3(0f, mobY, mobX);
                    break;
            }

            WaveMob mob = new WaveMob();
            mob.position = mobPosition;
            mob.spawnPosition = new Vector3(20f,20f,20f);
            mob.preFab = enemyPreFab;
            mob.formation = true;

            wave.waveMobs.Add(mob);
        }

        return wave;
    }
    // Generates a wave with mobs that run over the map
    private Wave RunnerWaveGenerator(Round round) {
        Wave wave = new Wave();
        wave.InitVariables();

        float xMin = 0f, xMax = 0f, yMin = 0f, yMax = 0f;
        switch (round.perspective) {
            case PerspectiveOptions.topDown:
                xMin = 23f;
                xMax = 30f;
                yMin = -12.5f;
                yMax = 12.5f;
                break;
            case PerspectiveOptions.sideScroler:
                xMin = 23f;
                xMax = 30f;
                yMin = -12.5f;
                yMax = 12.5f;
                break;
        }

        for (var i = 0; i < 3; i++) {
            float mobX = UnityEngine.Random.Range(xMin, xMax);
            float mobY = UnityEngine.Random.Range(yMin, yMax);

            Vector3 mobPosition = new Vector3(0,0,0);

            switch (round.perspective) {
                case PerspectiveOptions.topDown:
                    mobPosition = new Vector3(mobX, 0f, mobY);
                    break;
                case PerspectiveOptions.sideScroler:
                    mobPosition = new Vector3(0f, mobY, mobX);
                    break;
            }

            WaveMob mob = new WaveMob();
            mob.spawnPosition = mobPosition;
            mob.position = mobPosition;
            mob.preFab = enemyPreFab;
            mob.formation = false;

            wave.waveMobs.Add(mob);
        }

        return wave;
    }
    // Chooses an area for the Formation wave based on perspective
    private SpawnArea ChooseArea(PerspectiveOptions perspective) {

        SpawnArea area = new SpawnArea();
        switch (perspective) {
            case PerspectiveOptions.sideScroler:
                area = GetRandomArea(sideScrollerAreas);
                break;
            case PerspectiveOptions.topDown:
                area = GetRandomArea(topDownAreas);
                break;
        }
        lastSpawnedArea = area;
        return area;
    }
    // Get a random area from a predefined list
    private SpawnArea GetRandomArea(List<SpawnArea> list) {

        List<SpawnArea> optionsList = new List<SpawnArea>(list);

        if (optionsList.Contains(lastSpawnedArea)) {
            optionsList.Remove(lastSpawnedArea);
        }
        int randomIndex = Mathf.Max(UnityEngine.Random.Range(0, optionsList.Count()) - 1, 0);
    
        return optionsList[randomIndex];
    }

    #endregion

    #region Gameplay Management Methods
    // Spawn a given mob list and return the GameObject list
    private List<GameObject> SpawnMobList(List<WaveMob> mobList, bool fixedPosition) {
        List<GameObject> list = new List<GameObject>();
        for (var i = 0; i < mobList.Count(); i++) {
            WaveMob mobStruct = mobList[i];
            GameObject mob = Instantiate(mobStruct.preFab, mobStruct.spawnPosition, Quaternion.identity);
            MovementComponent movementComponent = mob.GetComponent<MovementComponent>();
            movementComponent.fixedPosition = fixedPosition;
            movementComponent.targetPosition = mobStruct.position;
            list.Add(mob);

            mob.GetComponent<Entity>().spawner = gameObject;
        }
        return list;
    }
    // Control Wave spawn and call SpawnMobList
    private List<GameObject> SpawnWave(Wave wave, bool fixedPosition) {
        List<GameObject> mobList = new List<GameObject>();
        if (!wave.waveSpawned) {
            perspectiveManager.SwitchPerspective(currentRound.perspective);
            mobList = SpawnMobList(wave.waveMobs, fixedPosition);
            wave.waveSpawned = true;
        }
        return mobList;
    }
    // Call SpawnWave for Formation and Runner Waves
    private void RoundManager() {
        if (!formationWaveSpawned && currentRound.formationWaveFlag) {
            formationMobList = SpawnWave(currentRound.formationWaves[currentRound.formationWaveInd], true);
            formationWaveSpawned = true;
        }
        if (!runnerWaveSpawned && currentRound.runnerWaveFlag) {
            runnerMobList = SpawnWave(currentRound.runnerWaves[currentRound.runnerWaveInd], false);
            runnerWaveSpawned = true;
        }

        if (!currentRound.runnerWaveFlag && !currentRound.formationWaveFlag) {
            currentRound = RoundGenerator();
        }
    }
    // Check if given list contains the given mob and remove it from the list, returns false if the list is empty
    private bool CheckWaveList(GameObject mob, List<GameObject> mobList) {
        bool waveInProgress = true;
        if (mobList.Contains(mob)) {
            mobList.Remove(mob);

            if (mobList.Count() <= 0) {
                waveInProgress = false;
            }
        }

        return waveInProgress;
    }
    //Call CheckWaveList when an enemy object is destroyed 
    public void MobDestroyed(GameObject mob) {
        bool formationWave = CheckWaveList(mob, formationMobList);
        if (!formationWave) {
            currentRound.formationWaveInd++;
            formationWaveSpawned = false;
            if (currentRound.formationWaveInd >= currentRound.formationWaves.Count()) {
                currentRound.formationWaveInd = currentRound.formationWaves.Count() - 1;
                currentRound.formationWaveFlag = false;
            }
        }
        bool runnerWave = CheckWaveList(mob, runnerMobList);
        if (!runnerWave) {
            currentRound.runnerWaveInd++;
            runnerWaveSpawned = false;
            if (currentRound.runnerWaveInd >= currentRound.runnerWaves.Count()) {
                currentRound.runnerWaveInd = currentRound.runnerWaves.Count() - 1;
                currentRound.runnerWaveFlag = false;
            }
        }
    }
    #endregion 

    private void Awake() {
        sideScrollerAreas = new List<SpawnArea> { sideScrollerTop, sideScrollenCenter, sideScrollerDown };
        topDownAreas = new List<SpawnArea> { topDownRight, topDownLeft, topDownCenter };

        currentRound = RoundGenerator();
    }
    void Update()
    {
        RoundManager();
    }
}
