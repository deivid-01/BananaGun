// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2011--------------------------------------------------
//--------------------------------------------------------------------------
using UnityEngine;  // Importación de la libreria principal de Unity
using OpenCvSharp; // Importación de la Libreria de OpenCv

/// <summary>
/// Esta clase contiene los metodos para detectar  el led y la bolita
/// </summary>
public class ObjectDetection 
{
   /// <summary>
   /// Este metodo se encarga de encontrar la posición de la bolita
   /// </summary>
   /// <param name="m">Matriz con los pixeles de la imagen </param>
   /// <param name="tolerance"> Tolerancia de movimiento</param>
   /// <param name="lastXPixel"> Última posición encontrada en el eje X</param>
   /// <param name="lastYPixel"> Ultima posición encontrada en el eje Y</param>
    public static void SetMuzzlePosition(Mat m, int tolerance,int lastXPixel, int lastYPixel)
    {

        // Se crea matrices vacias que van a contener las proyecciones
        Mat col_proyection = new Mat();
        Mat row_proyection = new Mat();

        //Se aplica umbralización binaria inversa, con un umbral de 102
        Cv2.Threshold(m, m, 102, 255, ThresholdTypes.BinaryInv);

        // Se aplica Morfologia
        ApplyMorphologyBall(ref m);

       

        // Se obtiene la  Proyection Vertical
        Cv2.Reduce(m, col_proyection, ReduceDimension.Column, ReduceTypes.Sum, MatType.CV_32F);
        // Se obtiene la  Proyection Horizontal
        Cv2.Reduce(m, row_proyection, ReduceDimension.Row, ReduceTypes.Sum, MatType.CV_32F);

        // Si la suma de los elementos de la proyección esta vacia retorna ya que el elemento no existe
        if (Cv2.CountNonZero(col_proyection)<10 || Cv2.CountNonZero(row_proyection)<10)
        {
            //Se le avisa a todos los elementos que estan suscritos a esta acción de que el elemento no fue detectado
            GameEvent.instance.MuzzelNotDetected();
            return;
        }

        // Se calcula la media de la  Proyection Vertical y Horizontal
        double col_mean = (Cv2.Sum(col_proyection).Val0) / Cv2.CountNonZero(col_proyection);
        double row_mean = (Cv2.Sum(row_proyection).Val0) / Cv2.CountNonZero(row_proyection);

        // Condición para asegurar que la media del objeto es lo suficientemente grande 
        bool objectExist = col_mean > 1000 && row_mean > 1000;

    
        if (objectExist)
        {
            //Si el objeto existe se procede a encontrar la posicion del objeto y luego se notifica a todos los objetos suscritos a esta acción de que la bolita fue detectada en esa posición
            GameEvent.instance.MuzzelDetected(GetObjectPosition(col_proyection, row_proyection, col_mean, row_mean, tolerance,lastXPixel, lastYPixel));
        }
     
    }

    /// <summary>
    /// Este metodo se encarga de detectar el led
    /// </summary>
    /// <param name="m"> Matriz con los pixeles de la imagen </param>
    /// <param name="umbralSum"> Sumatoria minima que debe tener el elemento para detectarlo</param>
    /// <returns></returns>

    public static bool DetectLazer(Mat m, int umbralSum)
    {
        //Se aplica umbralización binaria con un umbral de 145
        Cv2.Threshold(m, m, 145, 255, ThresholdTypes.Binary);

        //Se aplica morfologia para eliminar el ruido
        ApplyMorphologyLed(ref m);

        // Si la sumatoria de los elementos de la matriz es mayor al umbral minimo quiere decir que el objeto existe
        return (Cv2.Sum(m).Val0 > umbralSum) ? true : false;   
    }
    /// <summary>
    /// Aplica morfologia a la matriz con la bolita
    /// </summary>
    /// <param name="m">Matriz con los pixeles de la imagen</param>
    static void ApplyMorphologyBall(ref Mat m)
    {
       // Se obtiene el elemento estructural que tiene forma de rectangulo con forma de 3x3 pixeles
        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));

        // Se aplica Open para quitar pequeños punticos en la imagen
        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 3);
    }
    /// <summary>
    /// / Aplica morfologia a la matriz con la bolita
    /// </summary>
    /// <param name="m"> Matriz con los pixeles de la imagen</param>
    static void ApplyMorphologyLed(ref Mat m)
    {
        Mat strElem = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        Cv2.MorphologyEx(m, m, MorphTypes.Open, strElem, iterations: 5);
        Cv2.Dilate(m, m, strElem, iterations: 30);
    }

    /// <summary>
    /// Obtiene la posición en el eje X y Y del objeto detectado
    /// </summary>
    /// <param name="col_proyection"> Matriz con la proyeccion vertical</param>
    /// <param name="row_proyection">Matriz con la proyecction horizontal</param>
    /// <param name="col_mean">Media de la proyeccion vertical</param>
    /// <param name="row_mean">Media de la proyección horizontal</param>
    /// <param name="tolerance">Tolerancia de movimiento</param>
    /// <param name="lastXPixel"> Última posición encontrada en el eje X</param>
    /// <param name="lastYPixel"> Ultima posición encontrada en el eje Y</param>
    /// <returns>Retorna la posición del Objeto detectado</returns>
    static Vector2 GetObjectPosition(Mat col_proyection, Mat row_proyection, double col_mean, double row_mean, int tolerance, int lastX,int lastY)
    { 
       return new Vector2(GetXAxis(row_proyection, row_mean, row_mean / 10, tolerance, lastX), //Obtiene posición en el eje X
                           GetYAxis(col_proyection, col_mean, col_mean / 10, tolerance,lastY)); //Obtiene posición en e eje Y 
     }

    /// <summary>
    /// Obtiene la posición del objeto en el eje Y
    /// </summary>
    /// <param name="col_proyection">Matriz con la proyección vertical</param>
    /// <param name="col_mean">Media de la matriz con la proyeccion vertical</param>
    /// <param name="col_tolerance">Tolerancia de la media  en el eje Y</param>
    /// <param name="tolerance"> Tolerancia de la emdia en el eje Y</param>
    /// <param name="lastY">Ultima posición encontrada en el eje Y </param>
    /// <returns>EL valor invertido de la posicion en el eje Y</returns>
    static float GetYAxis(Mat col_proyection, double col_mean, double col_tolerance, int tolerance, int lastY)
    {
        //Se itera por cada fila de la matriz
        for (int i = 0; i < col_proyection.Rows; i++)
        {
            //Se obtiene el valor
            float sum = col_proyection.At<float>(new int[] { i, 0 });

            //Si la posición esta cerca a la media += cierta tolerancia significa que nos estamos acercando al centro del objeto
            if (sum > (col_mean - col_tolerance) && sum < (col_mean + col_tolerance))
            {
                //Si la posición actual esta cerca a la posición anterior += la tolerancia se debe actualizar la posicion
                if (i >= lastY + tolerance || i <= lastY - tolerance) 
                {   
                    //Se retorna el valor invertido de la posición en el eje Y
                    return -1 * i;
                }

            }
        }

        return -1*lastY; // Si no sufrió un movimiento sustancial sigue retornando la posicion anterior
    }
    /// <summary>
    /// Obtiene la posición en el eje X del objeto
    /// </summary>
    /// <param name="row_proyection">Matriz con la proyección horizontal</param>
    /// <param name="row_mean">Media de la matriz con la proyeccion horizontal</param>
    /// <param name="row_tolerance">Tolerancia de la media  en el eje X</param>
    /// <param name="tolerance">Tolerancia de la emdia en el eje X </param>
    /// <param name="lastX"> Ultima posición encontrada en el eje X  </param>
    /// <returns> Retorna la posición en el eje X</returns>
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

        return lastX; // Si no sufrió un movimiento sustancial sigue retornando la posicion anterior
    }
}
