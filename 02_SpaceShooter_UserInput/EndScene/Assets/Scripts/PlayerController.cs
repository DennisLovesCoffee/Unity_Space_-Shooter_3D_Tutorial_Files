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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetUpBoundries();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();

        CalculateBoundries();
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
    }

    private void MovePlayer()
    {
        float horizontalMovement = input.Horizontal;
        float verticalMovement = input.Vertical;

        Vector3 movementVector = new Vector3(horizontalMovement, verticalMovement, 0f);

        rb.velocity = movementVector * moveSpeed;
    }
}
