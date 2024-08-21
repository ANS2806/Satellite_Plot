using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 0.004167f;

    // Rotation axis (e.g., Vector3.up for rotation around the Y-axis)
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        // Calculate the rotation amount based on rotation speed and time elapsed
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Apply the rotation to the object
        transform.Rotate(rotationAxis, rotationAmount);
    }
}

