using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeedsPattern : MonoBehaviour
{
    private PlantDistribution weedDistribution;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Wait for parameter script to load parameters        
        yield return new WaitUntil(() => gameObject.GetComponent<LoadParameters>().IsLoaded);

        // Get the spawn parameters
        weedDistribution = gameObject.GetComponent<LoadParameters>().weedDistribution;
        
        // Debug.Log(weedDistribution.name);
        // Debug.Log(weedDistribution.distribution_type);  

        spawnWeeds();
    }

    void spawnWeeds() 
    {
        for(int i = 0; i < weedDistribution.n_plants; i++)
        {
            Vector2 weed_position = weedDistribution.distribution.getNextPosition();
            // Debug.Log("Spawning weed number " + i + " at position " + weed_position);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(weed_position[0], 0, weed_position[1]);
        }
    }
}
