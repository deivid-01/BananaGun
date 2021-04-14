using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using System.IO;
using System.Linq;
public class CreatingMask : MonoBehaviour
{
   public  AnimationCurve animationCurve;
    
     WebCamTexture webCamTexture;

    public RectTransform canvas;
    public Texture2D texture;

    public Image target;

    public RawImage image;
    Mat snapShot = new Mat();
    Mat mat = new Mat();


    int lasti = 0;

    public bool cameraEnabled = true;

    public bool testUpdate = false;

    public int umbral;
    public int iterations;
    public float speedPointer;
    public int toleranceTarget=55;

    int umbralSum= 50000;
    OpenCvTools openCvTools;
    Shooting shotting;
    public static Vector2 positionObj = new Vector2();
    private void Start()
    {
        openCvTools = OpenCvTools.instance;
        shotting = Shooting.intance;

        if (UIControls.mouseController)
        {
            image.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        else if (cameraEnabled)
        {
            OpenCvTools.SetCameraTexture(ref webCamTexture);
            
           image.texture = webCamTexture;

        }

        GameEvent.instance.OnRoundEnds += StopCamera;



    }

    private void OnDestroy()
    {
        GameEvent.instance.OnRoundEnds -= StopCamera;
    }

    private void Update()
    {
        if (webCamTexture.isPlaying)
        {
            mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2Lab);

            FindMuzzel(mat.ExtractChannel(1).Flip(FlipMode.Y));
            FindLazer(mat.ExtractChannel(1).Flip(FlipMode.Y));
        }
       
    }

    void StopCamera()
    {
        if (webCamTexture.isPlaying)
        {
           webCamTexture.Stop();
        }
    }
    void FindLazer(Mat m)
    {

        //Morfo : Cleaning
        // Cv2.Threshold(m, m, 111, 255, ThresholdTypes.BinaryInv);
        Cv2.Threshold(m, m, 150, 255, ThresholdTypes.Binary);


        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));


         Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 5);
       // Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 3);
        //Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 1);

        //image.texture = OpenCvSharp.Unity.MatToTexture(m);

        //print(Cv2.Sum(m).Val0);
        if (Cv2.Sum(m).Val0 > umbralSum)
        {
           
            shotting.Shoot(new Vector2(positionObj.x, 720 + positionObj.y));

        }
    }
    void FindMuzzel( Mat m)
    {
        //113 -> Light on | Night
        Cv2.Threshold(m, m, 113, 255, ThresholdTypes.BinaryInv);
        // Cv2.Threshold(m, m, 170, 255, ThresholdTypes.Binary);//ideal ->180
      
        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
       // OpenCvTools.rawImage = image;
        //Cv2.MorphologyEx(m, m, MorphTypes.Close, strElem, iterations: 15);
        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 3);
         //image.texture = OpenCvSharp.Unity.MatToTexture(m);



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
                    if (i >= lasti + toleranceTarget || i <= lasti- toleranceTarget) // Tolerance => 50 or -50 ( Standard desviation)
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
            // target.transform.position = positionObj; 

        }
        else
        {
            target.gameObject.SetActive(false);
        }
        

    }


}
