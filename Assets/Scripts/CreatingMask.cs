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

    bool done = false;
    int cont = 0;

    public bool cameraEnabled = true;

    public bool testUpdate = false;

    public int umbral;
    public int iterations;

    int umbralSum= 50000;
    OpenCvTools openCvTools;
    Shooting shotting;
    Vector2 positionObj = new Vector2();
    private void Start()
    {
        openCvTools = OpenCvTools.instance;
        shotting = Shooting.intance;

        if (cameraEnabled)
        {
            OpenCvTools.SetCameraTexture(ref webCamTexture);
            
           image.texture = webCamTexture;

        }

        else {
         

            //texture.Resize((int)canvas.rect.width, (int)canvas.rect.height, texture.format,false);
     


          

            /*
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
                  print(positionObj);
                  target.gameObject.SetActive(true);
                target.GetComponent<RectTransform>().anchoredPosition = positionObj;
                // target.transform.position = positionObj; 

              }
              else
              {
                  target.gameObject.SetActive(false);
              }

              */

        }



    }

    private void Update()
    {

        mat = OpenCvSharp.Unity.TextureToMat(webCamTexture);
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2Lab);

       FindMuzzel(mat.ExtractChannel(1));
        FindLazer(mat.ExtractChannel(1));




    }

    void FindLazer(Mat m)
    {

        //Morfo : Cleaning
        // Cv2.Threshold(m, m, 111, 255, ThresholdTypes.BinaryInv);
        Cv2.Threshold(m, m, 167, 255, ThresholdTypes.Binary);


        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));


        // Cv2.MorphologyEx(m, m, MorphTypes.Close, strElem, iterations: 4);
        //Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 10);
        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 1);

      //  image.texture = OpenCvSharp.Unity.MatToTexture(m);

        //print(Cv2.Sum(m).Val0);
        if (Cv2.Sum(m).Val0 > umbralSum)
        {
            shotting.Shoot(positionObj);

        }
    }
    void FindMuzzel(Mat m)
    {

        Cv2.Threshold(m, m, 111, 255, ThresholdTypes.BinaryInv);
        // Cv2.Threshold(m, m, 170, 255, ThresholdTypes.Binary);//ideal ->180

         

        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        OpenCvTools.rawImage = image;
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
            // target.transform.position = positionObj; 

        }
        else
        {
            target.gameObject.SetActive(false);
        }

    }

    void Shoot()
    {
        Debug.Log(" Piu piu bu bum buuum");
    }


}
