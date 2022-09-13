using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static event Action<PlanetInfo> OnPlanetEnter;

    //Play with some values to achieve different states
    [Header("Planet Values")]
    [Tooltip("Gravity of the planet")]
    [SerializeField] [Range(3f, 20f)] private float planetGravity;
    [Tooltip("Mass of the planet")]
    [SerializeField] [Range(10f, 100f)] private float planetMass;

    private PlanetInfo planetInfo;

    private void Start()
    {
        planetInfo = new PlanetInfo();
        planetInfo.planetTransform = this.transform;
        planetInfo.planetGravity = planetGravity;
        planetInfo.planetMass = planetMass;      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (OnPlanetEnter != null)
                OnPlanetEnter(planetInfo);
        }
    }
}
public struct PlanetInfo
{
    public Transform planetTransform;
    public float planetGravity, planetMass;

}
