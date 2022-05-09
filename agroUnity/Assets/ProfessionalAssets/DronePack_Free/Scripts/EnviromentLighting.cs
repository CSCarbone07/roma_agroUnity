using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA_DronePack_Free
{
    public class EnviromentLighting : MonoBehaviour
    {
        public Material skybox;
        [Range(0, 8)]
        public float brightness;
        public bool fog;
        public Color fogColor = Color.white;

        void OnEnable()
        {
            RenderSettings.skybox = skybox;
            RenderSettings.ambientIntensity = brightness;
            RenderSettings.fogColor = fogColor;
            DynamicGI.UpdateEnvironment();
        }
    }
}
