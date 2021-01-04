using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PA_DronePack_Free
{
    public class NightLights : MonoBehaviour
    {
        public List<Material> materials;
        public List<Light> lights;
        private List<PA_DroneController> drones = new List<PA_DroneController>();

        void Awake()
        {
            foreach (Material mat in materials) { mat.DisableKeyword("_EMISSION"); }
            foreach (Light light in FindObjectsOfType<Light>())
            {
                if (light.transform.name.Contains("Light"))
                {
                    lights.Add(light);
                }
            }
            drones.AddRange(FindObjectsOfType<PA_DroneController>());
        }

        public void LightsOn()
        {
            foreach (PA_DroneController drone in drones)
            {
                drone.UseEmission();
            }
            foreach (Material mat in materials) { mat.EnableKeyword("_EMISSION"); }
            foreach (Light light in lights) { light.enabled = true; }
        }

        public void LightsOff()
        {
            foreach (Material mat in materials) { mat.DisableKeyword("_EMISSION"); }
            foreach (Light light in lights) { light.enabled = false; }
        }

        void OnApplicationQuit()
        {
            foreach (Material mat in materials) { mat.DisableKeyword("_EMISSION"); }
            foreach (Light light in lights) { light.enabled = false; }
        }

        public void ReLoadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}
