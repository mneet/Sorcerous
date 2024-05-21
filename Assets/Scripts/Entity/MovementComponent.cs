using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;
using UnityEngine.EventSystems;
using System;
using UnityEditor.U2D;

public class MovementComponent : MonoBehaviour
{
    // Room
    [SerializeField] private float screenHeight = 25f;
    [SerializeField] private float screenWidth = 46f;

    // Game Systems
    private Stats stats;

    public enum MovementBehaviour {
        STRAIGHT,
        DIAGONAL,
        SENOIDAL,
        KAMIKAZE,
        TRACKPLAYER,
        ZIGZAG
    }
    private enum MovementDirection {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }

    // Movement controllers
    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private MovementDirection movementDirection;
    private Vector2 movementDirectionVector;
    private Vector3 movementDirectionVector3;

    // Kamikaze
    private Transform targetTransform = null;
    private Vector3 targetDirection = new Vector3();

    public bool fixedPosition = false;
    public Vector3 targetPosition;
    Transform cameraTransform;

    // Pick direction from enum
    private MovementDirection GetRandomDirection() {
        // Obtém todos os valores do enum como um array
        Array values = Enum.GetValues(typeof(MovementDirection));

        // Gera um índice aleatório entre 0 e o comprimento do array
        int randomIndex = UnityEngine.Random.Range(0, values.Length);

        // Retorna o valor correspondente ao índice aleatório
        return (MovementDirection)values.GetValue(randomIndex);
    }  
    public Vector3 GetVector3Direction() {
        Vector3 vec3 = new Vector3();
        switch (Perspective.Instance.perspective) {
            case PerspectiveOptions.topDown:
                vec3 = new Vector3(movementDirectionVector.x, 0, movementDirectionVector.y);
                break;

            case PerspectiveOptions.sideScroler:
                vec3 = new Vector3(0, movementDirectionVector.y, movementDirectionVector.x);
                break;
        }
        return vec3;
    }
    public void RandomizeMovementDirection() {
        switch (movementBehaviour) {
            case MovementBehaviour.DIAGONAL:
                // faz nada
                break;
            case MovementBehaviour.STRAIGHT:
                RandomizeStraightDirection();
                break;
        }
    }
    private void RandomizeStraightDirection() {
        MovementDirection direction = GetRandomDirection();
        movementDirection = direction;
        movementDirectionVector = new Vector2(0, 0);

        float screenHeightCenter = screenHeight / 2;
        float screenWidthCenter = screenWidth / 2;

        Vector2 newPosition = new Vector2(0, 0);
        switch (direction) {
            case MovementDirection.RIGHT:
                movementDirectionVector.x = 1;
                newPosition.x = UnityEngine.Random.Range(-screenWidthCenter - 20, -screenWidthCenter);
                newPosition.y = UnityEngine.Random.Range(-screenHeightCenter + 2, screenHeightCenter - 2);
                break;
            case MovementDirection.LEFT:
                movementDirectionVector.x = -1;
                newPosition.x = UnityEngine.Random.Range(screenWidthCenter, screenWidthCenter + 20);
                newPosition.y = UnityEngine.Random.Range(-screenHeightCenter + 2, screenHeightCenter - 2);
                break;
            case MovementDirection.UP:
                movementDirectionVector.y = 1;
                newPosition.x = UnityEngine.Random.Range(-screenWidthCenter + 2, screenWidthCenter - 2);
                newPosition.y = UnityEngine.Random.Range(-screenHeightCenter, -screenHeightCenter - 20);
                break;
            case MovementDirection.DOWN:
                movementDirectionVector.y = -1;
                newPosition.x = UnityEngine.Random.Range(-screenWidthCenter + 2, screenWidthCenter - 2);
                newPosition.y = UnityEngine.Random.Range(screenHeightCenter, screenHeightCenter + 20);
                break;
        }
        ApplyPosition(newPosition);
    }
    private void ApplyPosition(Vector2 newPosition) {

        Vector3 objectPosition = new Vector3();
        switch (Perspective.Instance.perspective) {
            case PerspectiveOptions.topDown:
                objectPosition.x = newPosition.x;
                objectPosition.z = newPosition.y;
                break;

            case PerspectiveOptions.sideScroler:
                objectPosition.z = newPosition.x;
                objectPosition.y = newPosition.y;
                break;
        }

        transform.position = objectPosition;
    }
    // Move entity
    private void BasicStraightMovement() {
        Vector3 movDir = new Vector3(0, 0, 0);

        switch (Perspective.Instance.perspective) {

            case PerspectiveOptions.topDown:
                movDir = new Vector3(movementDirectionVector.x, 0, movementDirectionVector.y);
                break;

            case PerspectiveOptions.sideScroler:
                movDir = new Vector3(0, movementDirectionVector.y, movementDirectionVector.x);
                break;
        }

        transform.position += movDir * stats.movementSpeed * Time.deltaTime;
    }

    private void KamikazeMovement()
    {
        if (targetTransform == null) {
            targetTransform = GameObject.Find("Player").transform;
            targetPosition = targetTransform.position;

            movementDirectionVector3 = targetTransform.position - transform.position;
            movementDirectionVector3.Normalize(); // normalize a direcao para que o tamanho do vetor seja 1 mas a direcao e sentido se mantenham
            transform.rotation = Quaternion.LookRotation(movementDirectionVector3, Vector3.up); // faz o objeto olhar para a direcao que esta
        }
        transform.position += movementDirectionVector3 * stats.movementSpeed * Time.deltaTime;     
    }

    private void SenoidalMovement()
    {

    }

    public Vector3 direcaoPadrao;
    public Vector3 direcaoInvertida;
    public float posParaInverter;
    public bool inverteu;


    private void InverterDirecao()
    {
        if (inverteu == false)
        {
            transform.Translate(direcaoPadrao * stats.movementSpeed * Time.deltaTime);
            if (transform.position.z < (cameraTransform.position.z + posParaInverter))
            {
                inverteu = true;
                transform.rotation = Quaternion.LookRotation(direcaoInvertida);
            }
        }
        else
        {
            //inverti
            transform.Translate(direcaoInvertida * stats.movementSpeed * Time.deltaTime, Space.World);
        }

    }
    private void TrackPlayerMovement()
    {
        Transform target = GameObject.Find("Player").transform;

        Vector3 dirTarget = (transform.position - target.position); // direção entre o alvo e o player~
        dirTarget.x = 0f;
        dirTarget.y = 0f;

        dirTarget.Normalize(); // normalize a direcao para que o tamanho do vetor seja 1 mas a direcao e sentido se mantenham

        if (Vector3.Distance(target.position, transform.position) > 3f)
        {
            transform.Translate(dirTarget * Time.deltaTime * stats.movementSpeed);
        }
        transform.rotation = Quaternion.LookRotation(-dirTarget); // faz o objeto olhar para a direcao que esta
    }

    private void MoveToFormation() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (stats.movementSpeed * 3) * Time.deltaTime);
    }

    // Check if entity left boundaries 
    private void CheckOutOfBorder() {
        bool xLimit = transform.position.x < -screenWidth || transform.position.x > screenWidth;
        bool yLimit = transform.position.y < -screenHeight || transform.position.y > screenHeight;
        bool zLimit = transform.position.z < -screenWidth || transform.position.z > screenWidth;

        if (xLimit || yLimit || zLimit) RandomizeMovementDirection();
    }
   

    private void Awake() {

        stats = gameObject.GetComponent<Stats>();

    }

    void Start() {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (fixedPosition) {
            MoveToFormation();
        }
        else {
            switch(movementBehaviour)
            {
                case MovementBehaviour.STRAIGHT: BasicStraightMovement();
                    break;
                case MovementBehaviour.KAMIKAZE: KamikazeMovement();    
                    break;
                case MovementBehaviour.SENOIDAL: SenoidalMovement();
                    break;
                case MovementBehaviour.TRACKPLAYER: TrackPlayerMovement(); 
                    break;
                case MovementBehaviour.ZIGZAG: InverterDirecao();
                    break;           
            }
            CheckOutOfBorder();
        }
    }
}
