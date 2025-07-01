using System.Collections.Generic;
using UnityEngine;

public class SolarSystemController : MonoBehaviour
{
    [Header("References")]
    /// <summary>
    /// Transform of the Sun object (center of the solar system).
    /// </summary>
    [Tooltip("Transform of the Sun object (center of the system)")]
    public Transform sunTransform;

    /// <summary>
    /// Name of the planet to control.
    /// </summary>
    [Tooltip("Name of the planet to control")]
    public string planetName;

    /// <summary>
    /// If true, shows the planet individually without orbiting the Sun (planet only rotates in place).
    /// </summary>
    [Tooltip("If true, show the planet individually without orbiting the Sun")]
    public bool isIndividualPreview = false;

    [Header("Settings")]
    /// <summary>
    /// Time scale multiplier to speed up orbit and rotation for AR visibility.
    /// Acts like a global time multiplier to fast-forward or slow down planet motion.
    /// </summary>
    [Tooltip("Time scale multiplier to speed up orbit and rotation for AR visibility")]
    private float timeScale = 1f;

    // Constant scale for planets when shown individually without orbiting.
    private const float individualPlanetScale = 1f;

    // Current orbit angle in degrees for this planet.
    private float currentOrbitAngle = 0f;

    // Dictionary mapping planet names to their orbital and rotational data.
    private Dictionary<string, PlanetData> planetDataMap;

    /// <summary>
    /// Unity Awake lifecycle method. Initializes the planet data dictionary.
    /// </summary>
    private void Awake()
    {
        InitializePlanetData();
    }

    /// <summary>
    /// Unity Update lifecycle method. Called once per frame to update orbit and rotation.
    /// If in individual preview mode, only rotates the planet without orbiting.
    /// </summary>
    private void Update()
    {
        if (!isIndividualPreview && sunTransform == null)
        {
            Debug.LogWarning("Sun Transform not assigned.");
            return;
        }

        if (!planetDataMap.TryGetValue(planetName, out PlanetData data))
        {
            Debug.LogWarning("Planet name not found.");
            return;
        }

        if (isIndividualPreview)
        {
            // Just rotate planet in place; scale fixed; no orbit.
            transform.localScale = Vector3.one * individualPlanetScale;
            UpdateRotation(data);
        }
        else
        {
            UpdateOrbit(data);
            UpdateRotation(data);
        }
    }

    /// <summary>
    /// Updates the planet's orbit position around the Sun based on elapsed time and the timeScale multiplier.
    /// </summary>
    /// <param name="data">The planet's orbital and rotational data.</param>
    private void UpdateOrbit(PlanetData data)
    {
        currentOrbitAngle = (currentOrbitAngle + data.orbitSpeedDegPerSec * timeScale * Time.deltaTime) % 360f;
        Vector3 orbitPos = Quaternion.AngleAxis(currentOrbitAngle, data.orbitAxis) * Vector3.right * data.planetDistanceFromSun;
        transform.position = sunTransform.position + orbitPos;
        transform.localScale = Vector3.one * data.planetSizeScale;
    }

    /// <summary>
    /// Rotates the planet around its own axis based on elapsed time and the timeScale multiplier.
    /// </summary>
    /// <param name="data">The planet's rotational data.</param>
    private void UpdateRotation(PlanetData data)
    {
        transform.Rotate(data.rotationAxis.normalized, data.rotationSpeedDegPerSec * timeScale * Time.deltaTime);
    }

    /// <summary>
    /// Initializes the dictionary storing orbital and rotational parameters for each planet.
    /// Data includes orbit speed, orbit axis, rotation speed, rotation axis, distance from the sun, and planet scale.
    /// </summary>
    private void InitializePlanetData()
    {
        planetDataMap = new Dictionary<string, PlanetData>()
        {
            { "Sun",     new PlanetData(0f, Vector3.up, 14.5f,  Vector3.up, 0f,    2f) },
            { "Mercury", new PlanetData(4.74f, Vector3.up, 0.71f, Vector3.up, 1.5f,   0.02f) },
            { "Venus",   new PlanetData(1.85f, Vector3.up, -0.17f, Vector3.up, 2.5f,   0.04f) },
            { "Earth",   new PlanetData(1.14f, Vector3.up, 4.17f, Vector3.up, 3.5f,   0.045f) },
            { "Mars",    new PlanetData(0.59f, Vector3.up, 4.06f, Vector3.up, 4.7f,   0.025f) },
            { "AsteroidBelt", new PlanetData(0f, Vector3.up, 4.5f, new Vector3(0, 0, 1), 0f,  0.38f) },
            { "Jupiter", new PlanetData(0.096f, Vector3.up, 10.1f,  Vector3.up, 8.5f,   0.1f) },
            { "Saturn",  new PlanetData(0.039f, Vector3.up, 9.35f, Vector3.up, 11.5f,   0.08f) },
            { "Uranus",  new PlanetData(0.013f, Vector3.up, -5.8f, Vector3.up, 14.5f,   0.04f) },
            { "Neptune", new PlanetData(0.0069f, Vector3.up, 6.22f, Vector3.up, 17.5f,  0.04f) },
        };
    }

    /// <summary>
    /// Manually updates the position and scale of the given planet transform.
    /// Respects individual preview mode by setting scale to constant and no orbiting.
    /// </summary>
    /// <param name="planetTransform">The Transform of the planet GameObject to update.</param>
    public void UpdatePlanetTransforms(Transform planetTransform)
    {
        if (planetTransform == null)
        {
            Debug.LogWarning("Planet transform is null.");
            return;
        }

        if (!planetDataMap.TryGetValue(planetName, out PlanetData data))
        {
            Debug.LogWarning($"Planet '{planetName}' not found in data map.");
            return;
        }

        if (isIndividualPreview)
        {
            planetTransform.localScale = Vector3.one * individualPlanetScale;
            // Position remains unchanged since no orbiting or sun reference in individual preview
        }
        else
        {
            if (sunTransform == null)
            {
                Debug.LogWarning("Sun Transform not assigned.");
                return;
            }
            planetTransform.localScale = Vector3.one * data.planetSizeScale;
            Vector3 orbitPos = Quaternion.AngleAxis(currentOrbitAngle, data.orbitAxis) * Vector3.right * data.planetDistanceFromSun;
            planetTransform.position = sunTransform.position + orbitPos;
        }
    }

    /// <summary>
    /// Internal data structure for storing orbital and rotational parameters of a planet.
    /// </summary>
    private class PlanetData
    {
        /// <summary>Degrees per second around the Sun.</summary>
        public float orbitSpeedDegPerSec;

        /// <summary>Axis of orbit (usually Vector3.up).</summary>
        public Vector3 orbitAxis;

        /// <summary>Planet's self-rotation speed in degrees per second.</summary>
        public float rotationSpeedDegPerSec;

        /// <summary>Axis of planet's own rotation.</summary>
        public Vector3 rotationAxis;

        /// <summary>Distance from the Sun in Unity units.</summary>
        public float planetDistanceFromSun;

        /// <summary>Scale factor for the planet's size.</summary>
        public float planetSizeScale;

        /// <summary>
        /// Constructs a new PlanetData instance.
        /// </summary>
        /// <param name="orbitSpeedDegPerSec">Orbit speed in degrees per second.</param>
        /// <param name="orbitAxis">Axis of orbit.</param>
        /// <param name="rotationSpeedDegPerSec">Rotation speed in degrees per second.</param>
        /// <param name="rotationAxis">Axis of rotation.</param>
        /// <param name="planetDistanceFromSun">Distance from the Sun.</param>
        /// <param name="planetSizeScale">Planet size scale factor.</param>
        public PlanetData(float orbitSpeedDegPerSec, Vector3 orbitAxis, float rotationSpeedDegPerSec, Vector3 rotationAxis, float planetDistanceFromSun, float planetSizeScale)
        {
            this.orbitSpeedDegPerSec = orbitSpeedDegPerSec;
            this.orbitAxis = orbitAxis;
            this.rotationSpeedDegPerSec = rotationSpeedDegPerSec;
            this.rotationAxis = rotationAxis;
            this.planetDistanceFromSun = planetDistanceFromSun;
            this.planetSizeScale = planetSizeScale;
        }
    }
}
