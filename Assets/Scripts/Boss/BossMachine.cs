using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMachine : MonoBehaviour
{
    // State
    
    public enum BossStates {
        IDLE,
        SIDE_TO_SIDE_SHOOTING,
        HEAL,
        SPINNING_N_SHOOTING,
        FOUR_CORNERS
    }
    [Header("State")]
    public BossStates state = BossStates.IDLE;
    private List<BossStates> stateList = new List<BossStates>();
    private bool stateSet = false;

    [SerializeField] private float stateTimer = 4f;
    [SerializeField] private float healSpawnTimer = 1f;
    private float healSpawnTimerMax;
    private float stateTimerMax;

    [SerializeField] private Slider healthBar;

    private float hp;
    private float maxHp;
    
    [Header("Movement")]
    // Movement
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxX;
    [SerializeField] private float minX;
    [SerializeField] private float maxZ;
    [SerializeField] private float minZ;
    private bool invertDirection;

    [SerializeField] private Vector3 moveDirVec = new Vector3(1, 0, 0);
    [SerializeField] private MovementComponent.MovementDirection direction = MovementComponent.MovementDirection.RIGHT;

    [Header("Positions")]
    // Positions
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 topCenterPosition;

    [Header("Boss parts")]
    // Boss parts
    [SerializeField] private GameObject rightArm;
    [SerializeField] private GameObject leftArm;
    [SerializeField] private GameObject bossCore;
    [SerializeField] private GameObject innerShield;
    [SerializeField] private GameObject outerShield;

    [SerializeField] private GameObject healerMob;


    private List<BossStates> defineStateQueue()
    {
        stateList = new List<BossStates>();
        Array enumValues = Enum.GetValues(typeof(BossStates));

        for (var i = 1; i < enumValues.Length; i++)
        {
            stateList.Add((BossStates)enumValues.GetValue(i));
        }

        // Shuffle
        var count = stateList.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = stateList[i];
            stateList[i] = stateList[r];
            stateList[r] = tmp;
        }

        return stateList;

    }

    private BossStates pickStateFromList()
    {
        BossStates new_state = stateList[0];

        stateList.RemoveAt(0);
        stateList.Add(new_state);

        return new_state;
    }

    private void slideToPosition(Vector3 destPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, destPos, movementSpeed * Time.deltaTime);

    }


    private void toggleShooting(bool flag) {

        // Parent
        ShootComponent coreShoot = gameObject.GetComponent<ShootComponent>();
        //coreShoot.activateShoot = flag;

        // RightArm
        ShootComponent rightArmShoot = rightArm.GetComponent<ShootComponent>();
       // rightArmShoot.activateShoot = flag;

        // LeftArm
        ShootComponent leftArmShoot = leftArm.GetComponent<ShootComponent>();
        //leftArmShoot.activateShoot = flag;
    }

    private float getTotalHp() {

        float totalHp = 0;
        // Parent
        Stats parentHp = gameObject.GetComponent<Stats>();
        if (parentHp.isActiveAndEnabled) totalHp += parentHp.health;


        // RightArm
        Stats rightArmHp = rightArm.GetComponent<Stats>();
        if (rightArmHp.isActiveAndEnabled) totalHp += rightArmHp.health;

        // LeftArm
        Stats leftArmHp = leftArm.GetComponent<Stats>();
        if (leftArmHp.isActiveAndEnabled) totalHp += leftArmHp.health;

        // InnerShield
        Stats innerShieldHp = innerShield.GetComponent<Stats>();
        if (innerShieldHp.isActiveAndEnabled) totalHp += innerShieldHp.health;

        // InnerShield
        Stats outerShieldHp = outerShield.GetComponent<Stats>();
        if (outerShieldHp.isActiveAndEnabled) totalHp += outerShieldHp.health;

        return totalHp;
    }


    #region State Methods
    private void idleState()
    {
        if (!stateSet) {
            // Se move para posi��o incial
            slideToPosition(topCenterPosition);
            toggleShooting(false);
            if (transform.position == topCenterPosition) {
                stateSet = true;
            }
        }
        else {
            // Condi��o de troca de estado
            if (transform.position == topCenterPosition) {
                    state = pickStateFromList();
                    stateTimer = stateTimerMax;
                    stateSet = false;
            }
        }
    }    
    
    private void healState()
    {
        if (!stateSet) {
            slideToPosition(topCenterPosition);
            toggleShooting(false);
            if (transform.position == topCenterPosition) {             
                stateSet = true;
            }
        }
        else {

            healSpawnTimer -= Time.deltaTime;
            if (healSpawnTimer <= 0) {
                healSpawnTimer = healSpawnTimerMax;
                //Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(minX, maxX), 0f, UnityEngine.Random.Range(-8f, -7.7f));
                //GameObject mob = Instantiate(healerMob, spawnPos, Quaternion.identity);
                //mob.GetComponent<HealEnemy>().boss = gameObject;
            }

            // Condi��o de troca de estado
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0 && direction == MovementComponent.MovementDirection.RIGHT) {
                state = pickStateFromList();
                stateTimer = stateTimerMax;
                stateSet = false;
            }
        }
    }

    private void sideToSideShootingState()
    {
        if (!stateSet) {
            slideToPosition(topCenterPosition);

            if (transform.position == topCenterPosition) {
                toggleShooting(true);
                stateSet = true;
            }
        }
        else {
            // Aplicando movimento
            transform.position += moveDirVec * (movementSpeed / 2) * Time.deltaTime;

            if (transform.position.x >= maxX || transform.position.x <= minX) {
                moveDirVec.x *= -1;
            }

            // Condi��o de troca de estado
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0) {
                state = pickStateFromList();
                stateTimer = stateTimerMax;
                stateSet = false;
            }
        }
    }

    private void fourCornersState() {
        if (!stateSet) {
            slideToPosition(topCenterPosition);

            if (transform.position == topCenterPosition) {
                moveDirVec = new Vector3(1, 0, 0);
                toggleShooting(true);
                stateSet = true;
            }
        }
        else {
            // Aplicando movimento
            transform.position += moveDirVec * (movementSpeed / 2) * Time.deltaTime;

            if (transform.position.x >= maxX) {
                moveDirVec = new Vector3(0,0,-1);
            }

            switch (direction) {
                case MovementComponent.MovementDirection.RIGHT:
                    if (transform.position.x >= maxX) {
                        moveDirVec = new Vector3(0, 0, -1);
                        direction = MovementComponent.MovementDirection.DOWN;
                    }
                    break;
                case MovementComponent.MovementDirection.DOWN:
                    if (transform.position.z <= minZ) {
                        moveDirVec = new Vector3(-1, 0, 0);
                        direction = MovementComponent.MovementDirection.LEFT;
                    }
                    break;
                case MovementComponent.MovementDirection.LEFT:
                    if (transform.position.x <= minX) {
                        moveDirVec = new Vector3(0, 0, 1);
                        direction = MovementComponent.MovementDirection.UP;

                    }
                    break;
                case MovementComponent.MovementDirection.UP:
                    if (transform.position.z >= maxZ + 0.1f) {
                        moveDirVec = new Vector3(1, 0, 0);
                        direction = MovementComponent.MovementDirection.RIGHT;
                    }
                    break;
            }
            transform.LookAt(new Vector3(0f,0f,0f));

            // Condi��o de troca de estado
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0 && direction == MovementComponent.MovementDirection.RIGHT) {
                state = pickStateFromList();
                stateTimer = stateTimerMax;
                stateSet = false;

            }
        }
    }

    private void spinningNShootingState()
    {
        if (!stateSet) {
            toggleShooting(true);
            stateSet = true;
        }

        slideToPosition(centerPosition);

        // Condi��o de troca de estado
        if (transform.position == centerPosition)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0)
            {
                state = pickStateFromList();
                stateTimer = stateTimerMax;
                stateSet = false;
            }
        }
    }
    #endregion
   
    private void stateMachineControl()
    {
        switch (state)
        {
            case BossStates.IDLE:
                idleState();
                break;

            case BossStates.SPINNING_N_SHOOTING:
                spinningNShootingState();
                break;

            case BossStates.SIDE_TO_SIDE_SHOOTING:
                sideToSideShootingState();
                break;

            case BossStates.HEAL:
                healState();
                break;

            case BossStates.FOUR_CORNERS:
                fourCornersState();
                break;
        }
    }

    private void Awake()
    {
        defineStateQueue();
        stateTimerMax = stateTimer;
        healSpawnTimerMax = healSpawnTimer;
    }

    private void Start() {
        hp = getTotalHp();
        maxHp = getTotalHp();
    }

    private void Update()
    {
        stateMachineControl();
        
        if (getTotalHp() != hp) {
            hp = getTotalHp();
            float hpPercent = hp / maxHp;
            Debug.Log(hpPercent);
            healthBar.value = hpPercent;
        }
    }
}
