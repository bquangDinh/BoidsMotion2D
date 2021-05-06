using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidGenerator : MonoBehaviour
{
    public GameObject boid;

    public int numberOfBoids = 1;

    public float spawnRadius = 1.0f;

    public bool drawSpawnRadius = true;

    // Start is called before the first frame update
    void Start()
    {
        Constants.numberOfBoids = numberOfBoids;

        for (int i = 0; i < numberOfBoids; ++i)
        {
            Vector3 pos = Random.insideUnitSphere * spawnRadius;
            pos.z = 0;
            Vector3 rotation = new Vector3(0, 0, Random.Range(0.0f, 360.0f));

            GameObject b = Instantiate(boid, transform.position + pos, Quaternion.Euler(rotation));
            Boid _b = b.GetComponent<Boid>();
            _b.ID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (drawSpawnRadius)
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
