using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
public class OpenCvTools : MonoBehaviour
{

    public static RawImage rawImage;

    #region Singlenton
    public static OpenCvTools instance;

    private void Awake()
    {
        instance = this;
    }

   public static  void  SetCameraTexture(ref WebCamTexture webCamTexture)
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        webCamTexture  =  new WebCamTexture(devices[0].name);

        webCamTexture.Play();
    }

    #endregion
    public IEnumerator Dilate(Mat mat, Mat strElem, int numDilations, int actual = 0)
    {


        yield return new WaitForSeconds(0.1f);
        Cv2.Dilate(mat, mat, strElem, iterations: 1);

        rawImage.texture = OpenCvSharp.Unity.MatToTexture(mat);

        if (actual < numDilations)
        {
            StartCoroutine(Dilate(mat, strElem, numDilations, actual + 1));
        }

    }

    public IEnumerator Erode(Mat mat, Mat strElem, int numDilations, int actual = 0)
    {


        yield return new WaitForSeconds(0.1f);

        Cv2.Erode(mat, mat, strElem, iterations: 1);

        rawImage.texture = OpenCvSharp.Unity.MatToTexture(mat);

        if (actual < numDilations)
        {
            StartCoroutine(Erode(mat, strElem, numDilations, actual + 1));
        }


    }


}
