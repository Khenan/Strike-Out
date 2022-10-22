using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfficheScreen : MonoBehaviour
{

    [SerializeField] private string path;

    [SerializeField] [Range(1, 5)] private int size = 1;

    // Update is called once per frame
    void Start()
    {
        path += "screenshot";
        path += System.Guid.NewGuid().ToString() + ".png";
        ScreenCapture.CaptureScreenshot(path, size);
    }
}

