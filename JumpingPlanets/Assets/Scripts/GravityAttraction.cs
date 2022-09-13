using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttraction : MonoBehaviour
{
    private Transform currentPlanet;
    private Rigidbody2D rBody;
    private float currentPlanetGravity = 0f;
    private float currentPlanetMass = 0f;


    void Start()
    {
        InitRigidbody();
    }

    private void FixedUpdate()
    {
        Attraction();
    }

    private void InitRigidbody()
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.gravityScale = 0f;
        rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void ChangePlanet(PlanetInfo planetInfo)
    {
        currentPlanetGravity = planetInfo.planetGravity;
        this.currentPlanet = planetInfo.planetTransform;
        currentPlanetMass = planetInfo.planetMass;
    }

    private void OnEnable()
    {
        Planet.OnPlanetEnter += ChangePlanet;
    }

    private void OnDisable()
    {
        Planet.OnPlanetEnter -= ChangePlanet;
    }

    private void Attraction()
    {
        if (currentPlanet != null)
        {
            float distance = Vector2.Distance(transform.position, currentPlanet.position);
            float gravitationalPower = currentPlanetGravity * ((rBody.mass * currentPlanetMass) / (distance * distance));
            //Debug.Log($"GravityPower: {gravitationalPower}");
            Vector2 directionToPlanet = (currentPlanet.position - transform.position).normalized;
            rBody.AddForce(directionToPlanet * Mathf.Abs(gravitationalPower));
            RotateBody(-directionToPlanet);
        }
    }

    private void RotateBody(Vector2 direction)
    {
        float angle = Vector2.Angle(transform.up, direction);
        //Check if rotation is clockwise or anticlockwise
        int clockwise = -1;
        if (Vector3.Cross(transform.up, direction).z > 0)
        {
            clockwise = 1;
        }
        angle *= clockwise;
        angle += transform.eulerAngles.z;
        angle = Mathf.Lerp(transform.eulerAngles.z, angle, 5f * Time.fixedDeltaTime);
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
