using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualCameraShoot : MonoBehaviour
{
    public int resWidth = 2550;
    public int resHeight = 3300;

    private bool takingHiResShot = false;
    public float samplingTime = 2f;

    private float probability = 30f;
    void Start()
    {

        StartCoroutine(setTakeShoot(samplingTime));

    }


    IEnumerator setTakeShoot(float time)
    {
        print("###################################################Taking screen");

        yield return new WaitForSeconds(time);
        TakeHiResShot();
        StartCoroutine(setTakeShoot(time));

    }



    public static string ScreenShotName(int width, int height, bool myRGB)
    {
        if(myRGB)
        {
            return string.Format("{0}/Screenshots/screenRGB_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }
        else
        {
            return string.Format("{0}/Screenshots/screenGT_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }



    }

    public void TakeHiResShot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 16);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        for (int y = 0; y < screenShot.height; y++)
        {
            for (int x = 0; x < screenShot.width; x++)
            {

                //Color color = ((x & y) != 0 ? Color.white : Color.gray);

                if(probability / 100f >= Random.Range(0.0f, 1.0f))
                {
                    if (screenShot.GetPixel(x, y).Equals(Color.green))
                    {
                        screenShot.SetPixel(x, y, Color.red);
                    }
                    else if (screenShot.GetPixel(x, y).Equals(Color.red))
                    {
                        screenShot.SetPixel(x, y, Color.green);
                    }
                }


            }
        }

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight, true);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        //takeHiResShot = false;

        /*
        if (transform.GetChild(0))
        {
            rt = new RenderTexture(resWidth, resHeight, 16);
            transform.GetChild(0).GetComponent<Camera>().targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            transform.GetChild(0).GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            transform.GetChild(0).GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();
            filename = ScreenShotName(resWidth, resHeight, false);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            //takeHiResShot = false;

            //transform.GetChild(i).gameObject;
        }
        */
    }

    void LateUpdate()
    {
        //takingHiResShot |= Input.GetKeyDown("k");



    }
}