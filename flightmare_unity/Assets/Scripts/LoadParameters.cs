using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadParameters : MonoBehaviour
{
    public bool IsLoaded { get; private set;}
    public PlantDistribution weedDistribution { get; private set;}
    public PlantDistribution potatoDistribution { get; private set;}


    private string weed_distribution_path = System.IO.Path.Combine(Application.streamingAssetsPath, "weed_spawning.json");
    private string potato_distribution_path = System.IO.Path.Combine(Application.streamingAssetsPath, "potato_spawning.json");

    // Start is called before the first frame update
    void Start() {
        weedDistribution = LoadDistributionFromJson(weed_distribution_path);
        potatoDistribution = LoadDistributionFromJson(potato_distribution_path);

        IsLoaded = true;
    }

    PlantDistribution LoadDistributionFromJson(string json_file)
    {
        string data = System.IO.File.ReadAllText(json_file);
        ParsedDistributionFromJSON plant_object = JsonUtility.FromJson<ParsedDistributionFromJSON>(data);

        // If seed is 0, set seed to null (random seed)
        int? seed = (plant_object.seed == 0)? null : (int?)plant_object.seed;

        // Create distributions
        BaseDistribution distribution;
        switch(plant_object.distribution_type)
        {
            case "gaussian":
                distribution = new GaussianDistribution(
                    plant_object.gaussian_position_mean, 
                    plant_object.gaussian_position_std,
                    seed
                );
                break;
            case "patch_gaussian":
                distribution = new PatchGaussianDistribution(
                    plant_object.patch_size_mean, 
                    plant_object.patch_size_std, 
                    plant_object.patch_position_std_mean, 
                    plant_object.patch_position_std_std,
                    seed
                );
                break;
            case "random":
                distribution = new RandomDistribution(
                    seed
                );
                break;
            default:
                Debug.Log("Distribution type of " + plant_object.name + " unknown: " 
                             + plant_object.distribution_type + "! Using random distribution...");
                distribution = new RandomDistribution();
                break;
        }

        // Create distribution object
        PlantDistribution plant_distribution = new PlantDistribution
        {
            name = plant_object.name,
            distribution_type = plant_object.distribution_type,
            distribution = distribution,
            n_plants = plant_object.n_plants
        };

        return plant_distribution;
    }
}
  
[System.Serializable]
public class PlantDistribution{
    public string name;
    public string distribution_type;
    public BaseDistribution distribution;
    public int n_plants;
}


[System.Serializable]
public class ParsedDistributionFromJSON{
    // Variables that are common
    public string name;
    public string distribution_type;
    public int n_plants;
    public int seed;

    // Variables for the patch Gaussian distribution
    public int patch_size_mean;
    public int patch_size_std;
    public float patch_position_std_mean; 
    public float patch_position_std_std; 

    // Variables for the Gaussian distribution
    public float gaussian_position_mean;
    public float gaussian_position_std;
}