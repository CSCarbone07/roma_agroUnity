using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class material_lerping : MonoBehaviour
{
    
    public Texture2D image;

    public List<Material> MaterialList;
    public Material material1;
    public Material material2;
    public float lerp_intensity = 0;

    public float duration = 2.0f;

    private Renderer rend;
    public bool updateLerp = false;


    // Start is called before the first frame update
    void Start()
    {
       print("lerping materials");
       //lerpMaterials(); 
      rend = GetComponent<Renderer> ();
      rend.material = material1;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.IsPlaying(this) && updateLerp)
        {
            //if (useSeed)
            //{ Random.seed = seed; }

	    lerpMaterials();

            updateLerp = false;
        }
        
	float lerp = Mathf.PingPong(Time.time, duration) / duration;
	print("lerping " + lerp);
        rend.material.Lerp(material1, material2, lerp);       
	Vector3 startPosition = new Vector3(0,0,0);
	Vector3 finalPosition = new Vector3(1,1,1);
	//transform.position = Vector2.Lerp(startPosition, finalPosition, lerp);
    }


    void lerpMaterials()
    {                
      rend = GetComponent<Renderer> ();
      rend.material = material1;
      rend.material.Lerp(material1, material2, lerp_intensity);

    }
   
    void rotateTexture()
    { 
      Color32[] pixels = image.GetPixels32();
      pixels = RotateMatrix(pixels, image.width);
      image.SetPixels32(pixels); 

    }
   
    static Color32[] RotateMatrix(Color32[] matrix, int n)
    {
	   Color32[] ret = new Color32[n * n];
	   
	   for (int i = 0; i < n; ++i) {
	       for (int j = 0; j < n; ++j) {
		   ret[i*n + j] = matrix[(n - j - 1) * n + i];
	       }
	   }
	   
	   return ret;
    }

}
