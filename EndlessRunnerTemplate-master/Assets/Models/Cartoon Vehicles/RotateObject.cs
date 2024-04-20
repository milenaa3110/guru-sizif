using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Speed of rotation
    public float rotationSpeed = 50.0f;

    void Update()
    {
        // Rotate the object around its Y axis at the specified speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

