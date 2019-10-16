using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float moveSpeed = 400f;
    private Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > AsteroidManager.Instance.asteroidSpawnDistance)
        {
            Destroy(gameObject);
        }
        rb.velocity = new Vector3(0f, 0f, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            other.gameObject.GetComponent<AsteroidController>().DestroyAsteroid();
            Destroy(gameObject);
        }
    }

}
