using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    // The object that the force will be applied towards
    public Rigidbody satellite;

    // Rigidbody component of the object
    private Rigidbody planet;

    // Gravitational constant
    private float G = 100f;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        planet = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Apply gravity force
        GravityForce();
    }

    void GravityForce()
    {
        // Mass of the planet and satellite
        float m1 = planet.mass;
        float m2 = satellite.mass;

        // Distance between the planet and satellite
        float r = Vector3.Distance(planet.position, satellite.position);

        // Gravitational force calculation
        Vector3 forceDirection = (planet.position - satellite.position).normalized;
        float forceMagnitude = G * (m1 * m2) / (r * r);

        // Apply the gravitational force
        satellite.AddForce(forceDirection * forceMagnitude);
    }
}
