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
    public static WaveManager Instance;

    // Round control variables
    private Round currentRound;
    private bool formationWaveSpawned = false;
    private bool runnerWaveSpawned = false;
    private List<GameObject> formationMobList = new List<GameObject>();
    private List<GameObject> runnerMobList = new List<GameObject>();

    private float timerStartDelayMax = 2f;
    private float timerStartDelay = 2f;
    private bool waveStartDelay = true;

    // Generation variables
    [SerializeField] private GameObject enemyPreFab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private List<GameObject> TopDownFixedMob;
    [SerializeField] private List<GameObject> TopDownMoveMob;
    [SerializeField] private List<GameObject> SideScrollFixedMob;
    [SerializeField] private List<GameObject> SideScrollMoveMob;

    private SpawnArea lastSpawnedArea;
    private int waveCountMax = 3;
    public int waveCount = 0;

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

        public bool roundFlag;

        public void InitVariables() {
            formationWaves = new List<Wave>();
            runnerWaves = new List<Wave>();
            runnerWaveInd = 0;
            formationWaveInd = 0;
            formationWaveFlag = true;
            runnerWaveFlag = true;
            roundFlag = false;
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
        newRound.perspective = Perspective.Instance.GetRandomPerspective();
        newRound.InitVariables();

        for (var i = 0; i < UnityEngine.Random.Range(1,5); i++) {
            newRound.formationWaves.Add(FormationWaveGenerator(newRound));
        }

        for (var i = 0; i < UnityEngine.Random.Range(1, 5); i++) {
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

        for (var i = 0; i < UnityEngine.Random.Range(2, 4); i++) {
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
            switch (round.perspective) {
                case PerspectiveOptions.sideScroler:
                    mob.preFab = PickRandomList(SideScrollFixedMob);
                    break;
                case PerspectiveOptions.topDown:
                    mob.preFab = PickRandomList(TopDownFixedMob);
                    break;
            }
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

        for (var i = 0; i < UnityEngine.Random.Range(1, 3); i++) {
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
            switch (round.perspective) {
                case PerspectiveOptions.sideScroler:
                    mob.preFab = PickRandomList(SideScrollMoveMob);
                    break;
                case PerspectiveOptions.topDown:
                    mob.preFab = PickRandomList(TopDownMoveMob);
                    break;
            }
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
    private GameObject PickRandomList(List<GameObject> list) {

        int randomIndex = Mathf.Max(UnityEngine.Random.Range(0, list.Count()));
        return list[randomIndex];
    }

    #endregion

    private Round BossRound() {
        Round bossRound = new Round();
        bossRound.InitVariables();

        Wave bossWave = new Wave();
        bossWave.InitVariables();

        bossRound.perspective = Perspective.PerspectiveOptions.topDown;

        WaveMob bossMob = new WaveMob();
        bossMob.preFab = bossPrefab;
        bossMob.spawnPosition = new Vector3(0f, 0f, 20f);
        bossMob.formation = false;
        bossMob.position = new Vector3(0f, 0f, 0f);

        bossWave.waveMobs.Add(bossMob);
        bossRound.formationWaves.Add(bossWave);
        bossRound.runnerWaveFlag = false;

        Debug.Log(bossRound);

        return bossRound;
    }

    #region Gameplay Management Methods
    // Spawn a given mob list and return the GameObject list
    private List<GameObject> SpawnMobList(List<WaveMob> mobList, bool fixedPosition) {
        List<GameObject> list = new List<GameObject>();
        
        for (var i = 0; i < mobList.Count(); i++) {
            Debug.Log(mobList.Count());
            WaveMob mobStruct = mobList[i];
            GameObject mob = Instantiate(mobStruct.preFab, mobStruct.spawnPosition, new Quaternion(0f,1f,0f,0f));
            MovementComponent movementComponent = mob.GetComponent<MovementComponent>();
            if (movementComponent != null) {
                movementComponent.fixedPosition = fixedPosition;
                movementComponent.targetPosition = mobStruct.position;
                movementComponent.RandomizeMovementDirection();
            }
            list.Add(mob);

            if (mob.GetComponent<ItemDrop>() != null) {
                mob.GetComponent<ItemDrop>().spawner = gameObject;
            }

        }
        return list;
    }
    // Control Wave spawn and call SpawnMobList
    private List<GameObject> SpawnWave(Wave wave, bool fixedPosition) {
        List<GameObject> mobList = new List<GameObject>();
        if (!wave.waveSpawned) { 
            mobList = SpawnMobList(wave.waveMobs, fixedPosition);
            wave.waveSpawned = true;
        }
        return mobList;
    }

    // Call SpawnWave for Formation and Runner Waves
    private void RoundManager() {

        if (!waveStartDelay) {
                if (!formationWaveSpawned && currentRound.formationWaveFlag && currentRound.formationWaves.Count > 0) {
                    formationMobList = SpawnWave(currentRound.formationWaves[currentRound.formationWaveInd], true);
                    formationWaveSpawned = true;
                }
                if (!runnerWaveSpawned && currentRound.runnerWaveFlag && currentRound.runnerWaves.Count > 0) {
                    runnerMobList = SpawnWave(currentRound.runnerWaves[currentRound.runnerWaveInd], false);
                    runnerWaveSpawned = true;
                }
        }
        else {
            timerStartDelay -= Time.deltaTime;
            if (timerStartDelay <= 0) {
                waveStartDelay = false;
                timerStartDelay = timerStartDelayMax;
            }
        }

        if (!currentRound.runnerWaveFlag && !currentRound.formationWaveFlag) {
            if ((waveCount + 1) % 5 == 0) {
                Debug.Log("Boss Wave");
                currentRound = BossRound();
                Perspective.Instance.SwitchPerspective(currentRound.perspective);
            }
            else {
                Debug.Log("Generating new Round");
                currentRound = RoundGenerator();
                Perspective.Instance.SwitchPerspective(currentRound.perspective);
                waveStartDelay = true;
            }

            if (!currentRound.roundFlag) {
                Debug.Log($" Wave: {waveCount}");
                waveCount += 1;
                HUDManager.Instance.UpdateWave(waveCount);
                currentRound.roundFlag = true;
            }
        }
    }
    // Check if given list contains the given mob and remove it from the list, returns false if the list is empty
    private bool CheckWaveList(GameObject mob, List<GameObject> mobList) {
      
        bool waveInProgress = true;
        if (mobList.Contains(mob)) {
            mobList.Remove(mob);
            Debug.Log("List checked");
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

    public void clearRound() {
        Debug.Log($"Runner Waves: {currentRound.runnerWaves.Count}");
        if (currentRound.runnerWaves.Count > 0 && runnerMobList.Count > 0){
            for (var i = 0; i < runnerMobList.Count; i++){
                Destroy(runnerMobList[i]);
            }
            currentRound.runnerWaveFlag=false;
            runnerMobList = new List<GameObject>();
        }

        Debug.Log($"Formation Waves: {currentRound.formationWaves.Count}");
        if (currentRound.formationWaves.Count > 0 && formationMobList.Count > 0){
            for (var i = 0; i < formationMobList.Count; i++){
                Destroy(formationMobList[i]);
            }
            
            currentRound.formationWaveFlag = false;
            formationMobList = new List<GameObject>();
        }

        formationWaveSpawned = false;
        runnerWaveSpawned = false;
    }
    public void skipToBoss() {
        clearRound();
        waveCount += (5 - (waveCount % 5)) -1;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        sideScrollerAreas = new List<SpawnArea> { sideScrollerTop, sideScrollenCenter, sideScrollerDown };
        topDownAreas = new List<SpawnArea> { topDownRight, topDownLeft, topDownCenter };
    }

    private void Start() {
        currentRound = BossRound();
        Perspective.Instance.SwitchPerspective(currentRound.perspective);
    }

    void Update()
    {
        if (!GameManager.Instance.gameEnded) RoundManager();

        // Debugs
        if (Input.GetKeyDown(KeyCode.F1)) {
            Debug.Log("Clearing wave");
            clearRound();
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            Debug.Log("Skipping to boss wave");
            skipToBoss();
        }
    }
}
