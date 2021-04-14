using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
public class MaskMain : MonoBehaviour
{
    public RawImage cameraImage;

    WebCamTexture webCamTexture;
    public int toleranceTarget = 55;
    public float speedPointer=20;
    int lasti = 0;
    public Image target;
    public static Vector2 positionObj;

    void Start()
    {
        GameEvent.instance.OnOptionSelected += StopCamera;


        if (UIControls.mouseController)
        {
            this.gameObject.SetActive(false);
            target.gameObject.SetActive(false);
            return;
        }
            OpenCvTools.SetCameraTexture(ref webCamTexture);

            cameraImage.texture = webCamTexture;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (webCamTexture.isPlaying)
        {
            Mat mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
            mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2Lab);
            FindMuzzel(mat.ExtractChannel(1).Flip(FlipMode.Y));

        }


    }

    void StopCamera()
    {
        if (webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
        }
    }

    void FindMuzzel(Mat m)
    {
        //113 -> Light on | Night
        Cv2.Threshold(m, m, 113, 255, ThresholdTypes.BinaryInv);
        // Cv2.Threshold(m, m, 170, 255, ThresholdTypes.Binary);//ideal ->180

        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        // OpenCvTools.rawImage = image;
        //Cv2.MorphologyEx(m, m, MorphTypes.Close, strElem, iterations: 15);
        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 3);
        
        //cameraImage.texture = OpenCvSharp.Unity.MatToTexture(m);

        
        

        Mat col_sum = new Mat();
        Mat row_sum = new Mat();

        // Proyection Vertical
        Cv2.Reduce(m, col_sum, ReduceDimension.Column, ReduceTypes.Sum, MatType.CV_32F);
        // Proyection Horizontal
        Cv2.Reduce(m, row_sum, ReduceDimension.Row, ReduceTypes.Sum, MatType.CV_32F);
        

        double col_mean = (Cv2.Sum(col_sum).Val0) / Cv2.CountNonZero(col_sum);
        double row_mean = (Cv2.Sum(row_sum).Val0) / Cv2.CountNonZero(row_sum);





        if (col_mean > 100 && row_mean > 100)
        {
            double col_tolerance = col_mean / 10;
            double row_tolerance = row_mean / 10;



            for (int i = 0; i < col_sum.Rows; i++)
            {
                float sum = col_sum.At<float>(new int[] { i, 0 });

                if (sum > (col_mean - col_tolerance) && sum < (col_mean + col_tolerance))
                {
                    if (i >= lasti + toleranceTarget || i <= lasti - toleranceTarget) // Tolerance => 50 or -50 ( Standard desviation)
                    {
                        lasti = i;
                        positionObj.y = -1 * i;
                        break;
                    }

                }
            }

            for (int i = 0; i < row_sum.Cols; i++)
            {
                float sum = row_sum.At<float>(new int[] { 0, i });


                if (sum > (row_mean - row_tolerance) && sum < (row_mean + row_tolerance))
                {
                    if (i >= positionObj.x + toleranceTarget || i <= positionObj.x - toleranceTarget)
                    {
                        positionObj.x = i;
                        break;
                    }
                }
            }



            target.gameObject.SetActive(true);
            target.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(target.GetComponent<RectTransform>().anchoredPosition, positionObj, speedPointer * Time.deltaTime);
            

        }
        else
        {
            target.gameObject.SetActive(false);
        }
        

    }
}
