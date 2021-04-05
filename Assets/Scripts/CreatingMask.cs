using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using System.IO;
public class CreatingMask : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public Texture2D texture;

    public Image target;

    public RawImage image;
    Mat snapShot = new Mat();
    Mat mat = new Mat();

    bool done = false;
    int cont = 0;

    public bool cameraEnabled = true;

    OpenCvTools openCvTools;
    private void Start()
    {
        openCvTools = OpenCvTools.instance;


        if (cameraEnabled)
        {
            OpenCvTools.SetCameraTexture(ref webCamTexture);

            
            image.texture = webCamTexture;
           
        }
       
            

    }

    private void Update()
    {
       

        mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2Lab);
        //Morfo : Cleaning
        mat = mat.ExtractChannel(2);
        Cv2.Threshold(mat, mat, 170, 255, ThresholdTypes.Binary);//ideal ->180

        //image.texture = OpenCvSharp.Unity.MatToTexture(mat);

   
        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        OpenCvTools.rawImage = image;
        Cv2.MorphologyEx(mat, mat, MorphTypes.Close, strElem, iterations: 15);
        Mat col_sum = new Mat();
        Mat row_sum = new Mat();

        // Proyection Vertical
        Cv2.Reduce(mat, col_sum, ReduceDimension.Column, ReduceTypes.Sum, MatType.CV_32F);
        // Proyection Horizontal
        Cv2.Reduce(mat, row_sum, ReduceDimension.Row, ReduceTypes.Sum, MatType.CV_32F);


        double col_mean = (Cv2.Sum(col_sum).Val0) / Cv2.CountNonZero(col_sum);
        double row_mean = (Cv2.Sum(row_sum).Val0) / Cv2.CountNonZero(row_sum);

        Vector2 positionObj = new Vector2();

        if (col_mean > 100 && row_mean > 100)
        {
            double col_tolerance = col_mean / 10;
            double row_tolerance = row_mean / 10;

            for (int i = 0; i < col_sum.Rows; i++)
            {
                float sum = col_sum.At<float>(new int[] { i, 0 });

                if (sum > (col_mean - col_tolerance) && sum < (col_mean + col_tolerance))
                {
                    positionObj.y = -1 * i;
                    break;
                }
            }

            for (int i = 0; i < row_sum.Cols; i++)
            {
                float sum = row_sum.At<float>(new int[] { 0, i });


                if (sum > (row_mean - row_tolerance) && sum < (row_mean + row_tolerance))
                {
                    positionObj.x = i;
                    break;
                }
            }

            target.gameObject.SetActive(true);
            target.GetComponent<RectTransform>().anchoredPosition = positionObj;

        }
        else
        {
            target.gameObject.SetActive(false);
        }

      
   
    }

}
