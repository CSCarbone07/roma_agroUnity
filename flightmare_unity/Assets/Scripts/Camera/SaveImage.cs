using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveImage : MonoBehaviour
{

    // camera resolution
    public int width = 1024;
    public int height = 1024;
    private int counter = 0;
     

    // Start is called before the first frame update
    void Start()
    {
        //TakeScreenshot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeScreenshot(string sub, int c)
    {
        counter = c;
        RenderTexture rt = new RenderTexture(width, height, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        //string filename = ScreenshotName(mode, field);
        
        string filename = string.Format("{0}/Dataset/" + sub + "{1}.png", Application.persistentDataPath, counter);
        System.IO.File.WriteAllBytes(filename, bytes);


        //filename = string.Format("{0}/Dataset/LocationAndRotations.png", Application.persistentDataPath);
        filename = string.Format("{0}/Dataset/cam/{1}.txt", Application.persistentDataPath, counter);

        string[] content = new string[2];
        //content[0] = "Position: " + this.transform.position.ToString();
        //content[1] = "Rotation: " + this.transform.rotation.ToString();
        content[0] = "Position " + this.transform.position.x.ToString() + " " + this.transform.position.y.ToString() + " " + this.transform.position.z.ToString();
        content[1] = "Rotation " + this.transform.eulerAngles.x.ToString() + " " + this.transform.eulerAngles.y.ToString() + " " + this.transform.eulerAngles.z.ToString();
        File.WriteAllLines(filename, content);

        //counter++;
    }

}
