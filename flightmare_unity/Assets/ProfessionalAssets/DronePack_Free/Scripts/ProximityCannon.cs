using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA_DronePack_Free
{
    public class ProximityCannon : MonoBehaviour
    {
        public GameObject cannonTarget;
        Transform shaft;
        Transform head;
        Transform spawn;
        public GameObject cannonBall;
        public float cannonBallLife = 5f;
        public float range = 40f;
        bool enteredRange = true;
        AudioSource[] audioSources;

        void Awake()
        {
            shaft = GameObject.Find("Shaft").transform;
            head = GameObject.Find("Head").transform;
            spawn = GameObject.Find("Spawn").transform;

            if (cannonTarget == null) { if (FindObjectOfType<PA_DroneController>().gameObject) { cannonTarget = FindObjectOfType<PA_DroneController>().gameObject; } }
            audioSources = GetComponents<AudioSource>();
            StartCoroutine("Fire");
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, cannonTarget.transform.position) < range)
            {
                Quaternion lookRot = Quaternion.LookRotation(cannonTarget.transform.position - head.transform.position);
                shaft.rotation = Quaternion.Slerp(shaft.rotation, Quaternion.Euler(0, lookRot.eulerAngles.y, 0), Time.deltaTime * 10f);
                head.rotation = Quaternion.Slerp(head.rotation, Quaternion.Euler(lookRot.eulerAngles.x, head.rotation.eulerAngles.y, 0), Time.deltaTime * 5f);
                if (enteredRange) { audioSources[0].PlayOneShot(audioSources[0].clip, 1f); }
                enteredRange = false;
            }
            else
            {
                enteredRange = true;
            }
        }

        public IEnumerator Fire()
        {
            while (true)
            {
                if (Vector3.Distance(transform.position, cannonTarget.transform.position) < range - 5)
                {
                    GameObject newProjectile = Instantiate(cannonBall, spawn.transform.position, Quaternion.identity);
                    newProjectile.GetComponent<Rigidbody>().AddForce(spawn.transform.forward * 5000 * newProjectile.GetComponent<Rigidbody>().mass);
                    audioSources[1].PlayOneShot(audioSources[1].clip, 1f);
                    Destroy(newProjectile, cannonBallLife);
                }
                yield return new WaitForSeconds(2f);
            }
        }

        public void SwapCannonBall(GameObject newPrefab)
        {
            cannonBall = newPrefab;
        }
    }
}
