using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick input;
    public float moveSpeed = 10f;
    public float maxRotation = 25f;

    private Rigidbody rb;
    private float minX, maxX, minY, maxY;

    public int maxHealth = 4;
    private int currentHealth;
    public InGameManager inGameManager;

    public Transform[] missleSpawnPoints;
    public GameObject rocketPrefab;
    public float fireInterval = 2f;
    private bool canFire = true;

    private Vector3 raycastDirection = new Vector3(0f, 0f, 1f);
    public float raycastDst = 100f;
    int layerMask;

    private List<GameObject> previousTargets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetUpBoundries();
        currentHealth = maxHealth;
        layerMask = LayerMask.GetMask("EnemyRaycastLayer");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();

        CalculateBoundries();

        RaycastForAsteroids();
    }

    private void RaycastForAsteroids()
    {
        List<GameObject> currentTargets = new List<GameObject>();

        foreach(Transform missleSpawnPoint in missleSpawnPoints)
        {
            RaycastHit hit;
            Ray ray = new Ray(missleSpawnPoint.position, raycastDirection);
            if(Physics.Raycast(ray, out hit, raycastDst, layerMask))
            {
                GameObject target = hit.transform.gameObject;
                currentTargets.Add(target);
            }
        }

        bool listsChanged = false;
        //check if the previous and current targets are the same
        if(currentTargets.Count != previousTargets.Count)
        {
            listsChanged = true;
        }
        else
        {
            for(int i = 0; i < currentTargets.Count; ++i)
            {
                if(currentTargets[i] != previousTargets[i])
                {
                    listsChanged = true;
                }
            }
        }

        if(listsChanged == true)
        {
            //update asteroids
            AsteroidManager.Instance.UpdateAsteroids(currentTargets);

            previousTargets = currentTargets;
        }

    }

    public void FireRockets()
    {
        if (canFire)
        {
            //fire rocktes
            foreach(Transform t in missleSpawnPoints)
            {
                Instantiate(rocketPrefab, t.position, Quaternion.identity);
            }

            canFire = false;

            StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ReloadDelay()
    {
        //play reload sound 

        yield return new WaitForSeconds(fireInterval);

        canFire = true;
    }

    private void RotatePlayer()
    {
        float currentX = transform.position.x;
        float newRotatinZ;

        if(currentX < 0)
        {
            //rotate negative
            newRotatinZ = Mathf.Lerp(0f, -maxRotation, currentX / minX);
        }
        else
        {
            //rotate positive
            newRotatinZ = Mathf.Lerp(0f, maxRotation, currentX / maxX);
        }

        Vector3 currentRotationVector3 = new Vector3(0f, 0f, newRotatinZ);
        Quaternion newRotation = Quaternion.Euler(currentRotationVector3);
        transform.localRotation = newRotation;
    }

    private void CalculateBoundries()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        transform.position = currentPosition;
    }

    private void SetUpBoundries()
    {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorners = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, camDistance));
        Vector2 topCorners = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, camDistance));

        //calculate the size of the gameobject
        Bounds gameObjectBouds = GetComponent<Collider>().bounds;
        float objectWidth = gameObjectBouds.size.x;
        float objectHeight = gameObjectBouds.size.y;
        


        minX = bottomCorners.x + objectWidth;
        maxX = topCorners.x - objectWidth;

        minY = bottomCorners.y + objectHeight;
        maxY = topCorners.y - objectHeight;

        //set up the asteroid manager
        AsteroidManager.Instance.maxX = maxX;
        AsteroidManager.Instance.minX = minX;
        AsteroidManager.Instance.minY = minY;
        AsteroidManager.Instance.maxY = maxY;
    }

    private void MovePlayer()
    {
        float horizontalMovement = input.Horizontal;
        float verticalMovement = input.Vertical;

        Vector3 movementVector = new Vector3(horizontalMovement, verticalMovement, 0f);

        rb.velocity = movementVector * moveSpeed;
    }

    public void OnAsteroidImpact()
    {
        currentHealth--;

        //change health bar
        inGameManager.ChangeHealthbar(maxHealth, currentHealth);

        if (currentHealth == 0) //called once
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        //play animation

        Debug.Log("Player Died");
    }
}
