using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    #region Singleton
    public static AsteroidManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public GameObject[] asteroidPrefabs;
    public float asteroidSpawnDistance = 50f;

    public float goldenAsteroidSpawnChange = 0.2f;
    public GameObject goldenAsteroidPrefab;

    public float spawnTime = 2f;
    private float timer = 0f;

    public int asteroidsToFinish = 3;
    public InGameManager inGameManager;
    public float speedIncrease = 5f;
    private int asteroidCtr = 0;

    [HideInInspector]
    public float minX, maxX, minY, maxY;

    [HideInInspector]
    public List<GameObject> aliveAsteroids = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTime;
        inGameManager.ChangeAsteroidKillCount(asteroidsToFinish);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnTime)
        {
            //spawn asteroids
            SpawnNewAsteroid();
            timer = 0f;
        }
    }

    public void OnAsteroidKill(GameObject asteroid)
    {
        asteroidsToFinish--;
        aliveAsteroids.Remove(asteroid);

        if(asteroidsToFinish <= 0)
        {
            if(GameManager.Instance != null)
            {
                int thisLevelIdx = GameManager.Instance.currentLevelIdx;
                int lastLevelCompleted = SaveManager.Instance.GetLevelsCompleted();
                if (thisLevelIdx >= lastLevelCompleted)
                {
                    SaveManager.Instance.CompletedNextLevel();
                }

                inGameManager.OpenLevelCompleteMenu();
            }
        }
        else
        {
            inGameManager.ChangeAsteroidKillCount(asteroidsToFinish);
        }

    }

    private void SpawnNewAsteroid()
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minX, maxY);

        Vector3 spawPos = new Vector3(newX, newY, asteroidSpawnDistance);

        float randomNr = Random.Range(0f, 1f);

        GameObject GO = null;
        if (randomNr < goldenAsteroidSpawnChange)
        {
            // spawn golden asteroid
            GO = Instantiate(goldenAsteroidPrefab, spawPos, Quaternion.identity);
        }
        else
        {
            //spawn normal asteroid
            GO = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawPos, Quaternion.identity);
        }

        GO.GetComponent<AsteroidController>().IncreaseSpeed(asteroidCtr * speedIncrease);
        asteroidCtr++;

        aliveAsteroids.Add(GO);
    }

    public void UpdateAsteroids(List<GameObject> targetedAsteroids)
    {
        foreach(GameObject asterid in aliveAsteroids)
        {
            if (targetedAsteroids.Contains(asterid))
            {
                //red
                asterid.GetComponent<AsteroidController>().SetTargetMaterial();
            }
            else
            {
                //reset it
                asterid.GetComponent<AsteroidController>().ResetMaterial();
            }
        }
    }


}
