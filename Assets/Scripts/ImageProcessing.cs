using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
public class ImageProcessing : MonoBehaviour
{
    public RawImage rawImageCamera;

    public int toleranceMuzzle;

    int umbralSum = 50000;

   public bool findMuzzle;
    public  bool findLazer;

    Pointer pointer;
    WebCamTexture webCamTexture;


    private void Start()
    {
        UIControls.mouseController = false;
            pointer = Pointer.instance;

        if (UIControls.mouseController)
        {           
            gameObject.SetActive(false);
           
        }
        else
        {
            OpenCvTools.SetCameraTexture(ref webCamTexture);

            rawImageCamera.texture = webCamTexture;
         
            GameEvent.instance.OnRoundEnds += StopCamera;
            GameEvent.instance.OnLoadingNextScene += StopCamera;
        }
    }

    private void OnDestroy()
    {
        StopCamera();
        GameEvent.instance.OnRoundEnds -= StopCamera;
    }

    private void OnApplicationQuit()
    {
        StopCamera();
    }

    private void Update()
    {
        if (webCamTexture.isPlaying)
        {
            Mat mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2Lab);

            if (findMuzzle)
            {
                ObjectDetection.SetMuzzlePosition(ref rawImageCamera, mat.ExtractChannel(2).Flip(FlipMode.Y),
                                                toleranceMuzzle,
                                                (int)pointer.rect.anchoredPosition.x,
                                               -(int)pointer.rect.anchoredPosition.y);

            }
            if (findLazer)
            {
                if (ObjectDetection.DetectLazer(ref rawImageCamera, mat.ExtractChannel(1).Flip(FlipMode.Y), umbralSum))
                {
                    
                    GameEvent.instance.StartShoot(new Vector2(pointer.rect.anchoredPosition.x, 720 + pointer.rect.anchoredPosition.y));
                }
            }


         
        }
    }

    void StopCamera()
    {
        if (webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
        }
    }

   
}
