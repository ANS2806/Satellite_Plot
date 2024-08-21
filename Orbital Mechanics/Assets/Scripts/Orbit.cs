using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Orbit : MonoBehaviour
{
    // The object that the force will be applied towards
    public Rigidbody planet;

    // Rigidbody component of the object
    private Rigidbody satellite;

    // Gravitational constant
    private float G = 100f;

    // Key press to decide which orbit type
    public KeyCode Circular = KeyCode.C;
    public KeyCode Elliptical = KeyCode.E;

    // Track orbit selection
    private bool orbitCircular = false;
    private bool orbitElliptical = false;

    // Eccentricity for elliptical orbit
    public float e;

    // Semi-major axis of the elliptical orbit (for calculation purposes)
    private float a;

    private float r_p;
    private float r;

     //Display Delta V
    public TextMeshProUGUI DeltaVText;
    private float DeltaV = 0f;
    private float oldVelocity;
    private float heldTimer = 0f;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        satellite = GetComponent<Rigidbody>();
        r_p = Vector3.Distance(planet.position, satellite.position);; // This should be set based on your desired periapsis distance

        //Get Text Mesh Pro
        if(DeltaVText!=null)
        {
            RectTransform rectTransform = DeltaVText.rectTransform;
            rectTransform.anchorMin = new Vector2(0,1); //Top-left corner
            rectTransform.anchorMax = new Vector2(0,1); //Top-left corner
            rectTransform.pivot = new Vector2(0,1); //Top-left pivot

            //Set position relative to Canvas
            rectTransform.anchoredPosition = new Vector2(10,-10); //adjust as needed

            //Display start text
            DeltaVText.text = $"<color=red> Delta V = {DeltaV}";
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Distance between the planet and satellite
        float r = Vector3.Distance(planet.position, satellite.position);
        // Determine which orbit type is selected
        if (Input.GetKey(Circular))
        {
            heldTimer += Time.deltaTime;
            
            if(heldTimer>0.2)
            {
                oldVelocity = satellite.velocity.magnitude;
                orbitCircular = true;  
                orbitElliptical = false;
                heldTimer = 0f;
            }
        }
        if(Input.GetKey(Elliptical))
        {
            heldTimer += Time.deltaTime;
            
            if(heldTimer>0.2)
            {
                oldVelocity = satellite.velocity.magnitude;
                orbitElliptical = true;
                orbitCircular = false;
                heldTimer = 0f;
            }
        }
        if (orbitElliptical)
        { 
            //Debug.Log(r);
            float m1 = planet.mass;

            // Semi-major axis calculation based on current distance and eccentricity
            
            a = r_p / (1 - e); // Semi-major axis

            // Calculate the required velocity for an elliptical orbit
            float velocityMagnitude = Mathf.Sqrt(2 * ((G * m1) / r - (G * m1) / (2 * a)));

            // Set the initial velocity perpendicular to the direction towards the planet
            Vector3 velocityDirection = Vector3.Cross(Vector3.up, (satellite.position - planet.position).normalized).normalized;
            satellite.velocity = velocityDirection * velocityMagnitude;
            
            //Debug.Log("Elliptical velocity set");
            DisplayDeltaV(velocityMagnitude, oldVelocity);
            
            orbitElliptical = false;
        }
        if (orbitCircular)
        {
            float m1 = planet.mass;

            // Calculate the required tangential velocity for a circular orbit
            float velocityMagnitude = Mathf.Sqrt((G * m1) / r);

            // Set the initial velocity perpendicular to the direction towards the planet
            Vector3 velocityDirection = Vector3.Cross(Vector3.up, (satellite.position - planet.position).normalized).normalized;
            satellite.velocity = velocityDirection * velocityMagnitude;

            //Debug.Log("Circular velocity set");
            DisplayDeltaV(velocityMagnitude, oldVelocity);
            
            orbitCircular = false;
        }
    }
    void DisplayDeltaV(float newVelocity, float oldVelocity)
    {
        Debug.Log("Old velocity was: " + oldVelocity + "\n" + " New Velocity is: " + newVelocity);
        DeltaV = Mathf.Abs(newVelocity-oldVelocity);
        DeltaVText.text = $"<color=red> Delta V = {DeltaV}";
    }
}
