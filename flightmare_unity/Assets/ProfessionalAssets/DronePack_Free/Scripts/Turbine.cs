using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA_DronePack_Free
{
    public class Turbine : MonoBehaviour
    {
        public bool active;
        private GameObject fanSwitch;
        private GameObject motor;
        private GameObject fan;
        private ParticleSystem particles;

        void Start()
        {
            fanSwitch = GameObject.Find("FanSwitch");
            motor = GameObject.Find("Motor");
            fan = GameObject.Find("Fan");
            particles = motor.GetComponent<ParticleSystem>();
        }

        void Update()
        {
            fan.GetComponent<HingeJoint>().useMotor = active;

            if (active) { if (!particles.isPlaying) { particles.Play(); } }
            if (!active) { if (particles.isPlaying) { particles.Stop(); } }

            if (fanSwitch.transform.localRotation.eulerAngles.y > 70) { fanSwitch.GetComponent<MeshRenderer>().material.color = Color.red; fanSwitch.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Vector4(0.5f, 0, 0, 1)); active = false; }
            if (fanSwitch.transform.localRotation.eulerAngles.y < 10) { fanSwitch.GetComponent<MeshRenderer>().material.color = Color.green; fanSwitch.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Vector4(0, 0.5f, 0, 1)); active = true; }
        }

        void OnTriggerStay(Collider newObject)
        {
            if (active)
            {
                Rigidbody hasRigidbody = newObject.GetComponent<Rigidbody>();
                if (hasRigidbody)
                {
                    newObject.GetComponent<Rigidbody>().AddForce(transform.forward * (400 / Vector3.Distance(transform.position, newObject.transform.position)));
                }
            }
        }
    }
}
