using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawner : MonoBehaviour
{
    public GameObject Prefab;

    public int numParticles = 10; // Number of blood particles to spawn

    public float spawnRadius = 0.5f; // Radius for spawning around the enemy

    public float fillProbability = 0.9f; // Probability of spawning a particle

    public Gradient ParticleGradient; // Gradient for blood particle color

    public float ColorSpeed = 1f; // Speed of color change

    // Options for force enhancement (choose one or combine)
    public float initialForceMultiplier = 1.0f; // Base force multiplier
    public float minForceMultiplier = 0.5f; // Minimum force multiplier (random range)
    public float maxForceMultiplier = 1.5f; // Maximum force multiplier (random range)
    public float minSpeed = 2.0f; // Minimum speed for setting velocity
    public float maxSpeed = 5.0f; // Maximum speed for setting velocity

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnBloodParticles() // Renamed for clarity
    {
        Debug.Log("Blood should spawn");
        for (int i = 0; i < numParticles; i++)
        {
            if (Random.value < fillProbability) // Spawn based on probability
            {
                // Random position around the enemy
                Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPoint.y = transform.position.y + 1f; // Keep Y position consistent

                // Random direction
                Vector3 randomDirection = Random.insideUnitSphere;

                // Choose Force Enhancement Method (comment out unused sections)

                // Option 1: Modify initialForceMultiplier
                // float randomForceMultiplier = initialForceMultiplier; // Use base value

                // Option 2: Random Force Multiplier
                float randomForceMultiplier = Random.Range(minForceMultiplier, maxForceMultiplier);

                // Option 3: Set Velocity
                // Vector3 randomVelocity = direction * Random.Range(minSpeed, maxSpeed);

                // Spawn particle with force/velocity in random direction and delayed destroy
                StartCoroutine(SpawnAndDestroyParticle(spawnPoint, randomDirection, randomForceMultiplier)); // Pass chosen multiplier
            }
        }
    }

    IEnumerator SpawnAndDestroyParticle(Vector3 pos, Vector3 direction, float chosenForceMultiplier) // Receive chosen multiplier
    {
        GameObject particle = Instantiate(Prefab, pos, Quaternion.identity); // Don't use random rotation for direction
        particle.transform.localScale *= Random.Range(0.1f, 1f); // Random scaling

        Color color = ParticleGradient.Evaluate(Mathf.Sin(Time.time * ColorSpeed) / 2 + 0.5f); // Option 2: Use Mathf.Sin
        particle.GetComponent<MeshRenderer>().material.color = color;

        // Apply force/velocity in the random direction with chosen multiplier
        Rigidbody rb = particle.GetComponent<Rigidbody>();
        if (rb != null) // Check if particle has a rigidbody component
        {
            // Option 1 & 2: Apply Force with multiplier
            // rb.AddForce(direction * chosenForceMultiplier, ForceMode.Impulse);

            // Option 3: Set Velocity
            rb.velocity = direction * Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            Debug.LogWarning("Blood particle prefab missing Rigidbody component. Physics won't work.");
        }

        // Wait for 2 seconds and then destroy the particle
        yield return new WaitForSeconds(2.0f);
        Destroy(particle);
    }
}