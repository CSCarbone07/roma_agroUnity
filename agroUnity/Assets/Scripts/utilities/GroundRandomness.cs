using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRandomness : MonoBehaviour
{

    Material m_Material;
    //Vector2 randomness = new Vector;//(0.5f, 0.5f, 0, 0);//, Random.RandomRange(0, 1));

    //Vector2 randomness = new Vector2(Random.RandomRange(0, 1), Random.RandomRange(0, 1));


    // Start is called before the first frame update
    void Start()
    {

        //Vector2 randomness = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        float randomness = Random.Range(0.0f, 1.0f);
        //Vector2 randomness = new Vector2(0.5f, 0.5f);


        //GameObject child = transform.GetChild(i).gameObject;

        m_Material = GetComponent<Renderer>().material; //= currentMats.ToArray();

        //Fetch the Material from the Renderer of the GameObject
        //m_Material = GetComponent<Renderer>().material;

        //print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);

        //m_Material.SetVector("Vector2_DC43CA85", randomness);
        m_Material.SetFloat("_NoiseOffset", randomness);

        //print(m_Material);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
