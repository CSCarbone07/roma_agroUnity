using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA_DronePack_Free
{
    public class WebGLHideMouse : MonoBehaviour
    {

        void Start()
        {
            Invoke("HideMouse", 0.5f);
        }

        void OnGUI()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnHideMouse();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Invoke("HideMouse", 0.5f);
            }

        }

        void HideMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void UnHideMouse()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
