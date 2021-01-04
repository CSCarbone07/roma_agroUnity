

using UnityEngine;

public class Example : MonoBehaviour
{
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Draws a wireframe sphere in the Scene view, fully enclosing
    // the object.
    void OnDrawGizmosSelected()
    {
        // A sphere that fully encloses the bounding box.
        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(center, radius);
    }
}

