using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = target.transform.position - offset;
    }
}
