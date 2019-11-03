using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float moveSpeed = 20f;
    private Rigidbody rb;
    private Vector3 randomRotation;
    private float removePositionZ;
    // Start is called before the first frame update

    public Material targetMaterial;
    private Material baseMat;
    private Renderer[] renderers;

    public bool isGoldenAsteroid = false;

    //new
    public ParticleSystem explosion;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        randomRotation = new Vector3(Random.Range(0f, 100f), Random.Range(0f, 100f), Random.Range(0f, 100f));
        removePositionZ = Camera.main.transform.position.z;

        renderers = GetComponentsInChildren<Renderer>();
        baseMat = renderers[0].material;
    }

    public void ResetMaterial()
    {
        if (renderers == null)
            return;

        foreach(Renderer rend in renderers)
        {
            rend.material = baseMat;
        }

    }

    public void SetTargetMaterial()
    {
        if (renderers == null)
            return;

        foreach (Renderer rend in renderers)
        {
            rend.material = targetMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < removePositionZ)
        {
            AsteroidManager.Instance.aliveAsteroids.Remove(gameObject);
            Destroy(gameObject);
        }

        Vector3 movementVector = new Vector3(0f, 0f, -moveSpeed * Time.deltaTime);
        rb.velocity = movementVector;

        transform.Rotate(randomRotation * Time.deltaTime);
    }

    public void DestroyAsteroid()
    {
        if (isGoldenAsteroid && GameManager.Instance != null)
        {
            //add gold
            SaveManager.Instance.AddGold();
        }

        //remove from alive list
        AsteroidManager.Instance.OnAsteroidKill(gameObject);

        //play particall effect
        Instantiate(explosion, transform.position, Quaternion.identity);

        //disable movement

        //disable colliders

        //destroy game object with a delay
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().OnAsteroidImpact();
            DestroyAsteroid();
        }
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        moveSpeed += speedIncrease;
    }
}
