using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
public class ObjectDetection 
{
   
    public static void SetMuzzlePosition(ref RawImage image,Mat m, int tolerance,int lastXPixel, int lastYPixel)
    {


        Mat col_proyection = new Mat();
        Mat row_proyection = new Mat();

        Cv2.Threshold(m, m, 102, 255, ThresholdTypes.BinaryInv);


        ApplyMorphology(ref m);

        //

        // Proyection Vertical
        Cv2.Reduce(m, col_proyection, ReduceDimension.Column, ReduceTypes.Sum, MatType.CV_32F);
        // Proyection Horizontal
        Cv2.Reduce(m, row_proyection, ReduceDimension.Row, ReduceTypes.Sum, MatType.CV_32F);

        if (Cv2.CountNonZero(col_proyection)<10 || Cv2.CountNonZero(row_proyection)<10)
        {
            GameEvent.instance.MuzzelNotDetected();
            return;
        }

        // Mean Proyection Vertical
        double col_mean = (Cv2.Sum(col_proyection).Val0) / Cv2.CountNonZero(col_proyection);
        //Mean  Proyection Horizontal
        double row_mean = (Cv2.Sum(row_proyection).Val0) / Cv2.CountNonZero(row_proyection);

        bool objectExist = col_mean > 1000 && row_mean > 1000;

       // Debug.Log($"Colmean: {col_mean} |  RowMean: {row_mean} | LastPixelX: {lastXPixel} | LastPixelX: {lastYPixel}");

        if (objectExist)
        {
           
            GameEvent.instance.MuzzelDetected(GetObjectPosition(col_proyection, row_proyection, col_mean, row_mean, tolerance,lastXPixel, lastYPixel));
        }
     
    }

    public static bool DetectLazer(ref RawImage rawImage, int threshHoldLazer,Mat m, int umbralSum)
    {


        Cv2.Threshold(m, m, threshHoldLazer, 255, ThresholdTypes.Binary);

       // rawImage.texture = OpenCvSharp.Unity.MatToTexture(m);

        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 5);
        // Cv2.MorphologyEx(m, m, MorphTypes.Close, strElem, iterations: 10);
        Cv2.Dilate(m, m, strElem, iterations: 30);
       // rawImage.texture = OpenCvSharp.Unity.MatToTexture(m);


        return (Cv2.Sum(m).Val0 > umbralSum) ? true : false;

        
          

        
    }
    static void ApplyMorphology(ref Mat m)
    {
        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));

        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 3);
    }

    static Vector2 GetObjectPosition(Mat col_proyection, Mat row_proyection, double col_mean, double row_mean, int tolerance, int lastX,int lastY)
    { 
       return new Vector2(GetXAxis(row_proyection, row_mean, row_mean / 10, tolerance, lastX),
                           GetYAxis(col_proyection, col_mean, col_mean / 10, tolerance,lastY));
     }

    static float GetYAxis(Mat col_proyection, double col_mean, double col_tolerance, int tolerance, int lastY)
    {
        for (int i = 0; i < col_proyection.Rows; i++)
        {
            float sum = col_proyection.At<float>(new int[] { i, 0 });

            if (sum > (col_mean - col_tolerance) && sum < (col_mean + col_tolerance))
            {
                if (i >= lastY + tolerance || i <= lastY - tolerance) // Tolerance => 50 or -50 ( Standard desviation)
                {
                   
                    return -1 * i;
                }

            }
        }

        return -1*lastY;
    }

    static float GetXAxis(Mat row_proyection, double row_mean, double row_tolerance, int tolerance,int lastX)
    {
        for (int i = 0; i < row_proyection.Cols; i++)
        {
            float sum = row_proyection.At<float>(new int[] { 0, i });


            if (sum > (row_mean - row_tolerance) && sum < (row_mean + row_tolerance))
            {
                if (i >= lastX + tolerance || i <= lastX - tolerance)
                {
                    return i;
                    
                }
            }
        }

        return lastX;
    }
}
