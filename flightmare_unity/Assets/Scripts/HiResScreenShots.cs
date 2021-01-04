using UnityEngine;
using System.Collections;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 1920;
    public int resHeight = 1080;

    private bool takeHiResShot = false;
    public Camera cam;
    public float timeLeft = 10f;


    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    //public void TakeHiResShot()
    //{
    //    takeHiResShot = true;
    //}

    void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
        print("cam.aspect: " + cam.aspect);

        //cam.cullingMask
        print("cam.depth: " + cam.depth);
        //cam.fieldOfView
        //cam.

    }


    public void ResetTimer()
    {
        timeLeft = 5f;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("v");
        if (takeHiResShot)

        //timeLeft -= Time.deltaTime % 60;
        //if (timeLeft <= 0.0 )
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
            //ResetTimer();
        }
    }
}