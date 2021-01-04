using UnityEngine;

public class LeafBending : MonoBehaviour
{

    Material m_Material;
    public Vector3 UV_Adjustment = new Vector3(0, 0, 0);
    public float Bend = 0;
    public Vector3 Position_Adjustment = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        int numOfChildren = transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            
            if (child.GetComponent<Renderer>())
            {
                //print("1");

                m_Material = child.GetComponent<Renderer>().material; //= currentMats.ToArray();
            }

        }

        //Fetch the Material from the Renderer of the GameObject
        //m_Material = GetComponent<Renderer>().material;

        //print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);

        m_Material.SetVector("_plantBend", UV_Adjustment);
        m_Material.SetFloat("_bendOffset", Bend);
        m_Material.SetVector("_positionOffset", Position_Adjustment);

        print(m_Material);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
