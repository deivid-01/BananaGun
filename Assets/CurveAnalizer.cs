using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveAnalizer : MonoBehaviour
{
    public AnimationCurve curve;

    float mean = 0;
    float std = 0;
    void Start()
    {
        mean = Mean(curve);
        std = Std(curve);
        print($"Mean: {mean}");
        print($"Std: {std}");
    }

    // Update is called once per frame
    float Mean(AnimationCurve curve)
    {

        float sum = 0;
        for (int i = 0; i < curve.keys.Length; i++)
        {
            sum += curve.keys[i].value;
        }


        return sum /(float) curve.keys.Length;
    }

    float Std(AnimationCurve curve)
    {

        float sum = 0;
        for (int i = 0; i < curve.keys.Length; i++)
        {
            sum += ((curve.keys[i].value -mean)* (curve.keys[i].value - mean));
        }


        return Mathf.Sqrt(sum/curve.keys.Length);
    }
}
