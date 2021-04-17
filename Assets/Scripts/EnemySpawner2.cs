// --------------------------------------------------------------------------
//------- Desarrolladores: -----------------------------
//-------- David Andrés Torres Betancour-------------------------------------------
//-------  Contacto : davida.torres@udea.edu.co --------------
//-------  Jenny Carolina Escobar Sozas    -----------------
//-------  Contacto:    carolina.escobar@udea.edu.co -------------------
//------- Proyecto 'Banana Gun' del Curso Procesamiento Digital de Imagenes----
//------- V1.5 Abril de 2021--------------------------------------------------
//--------------------------------------------------------------------------
using System.Collections; //Libreria de funcionalidades basicas de C#
using System.Collections.Generic; //Libreria de funcionalidades basicas de C#
using UnityEngine;// Importación de la libreria principal de Unity
using System; //Libreria de funcionalidades basicas de C#
/// <summary>
/// Maneja la instanciación cada uno de los enemigos en el round2
/// </summary>
public class EnemySpawner2 : MonoBehaviour
{
    //estructura auxiliar para guardar la posición de los enemigos que ya han sido instanciados
    struct elements
    {
        public int i; //Posición en el eje X
        public int j; // Posición en el eje Y
        public int k; //Posición en el eje Z

    }
    
    /// <summary>
    /// Contiene las propiedades basicas de un color
    /// </summary>
    [Serializable]
    public struct ColorCustom
    {
        public Color color; //Color
        public Color emissionColor; //Emisión de luz  
        public string name; //Nombre del color
    }

    //Modelo en 3d del enemigo
    public GameObject prefabEnemy;
    //Efecto especial para cuando el enemigo aparece
    public GameObject effectShowup;

    //Maximo número de enemigos que va a tener el nivel
    public  int maxNumEnemys = 20;
    //Maximo numero de enemigos que se van a  generar en cada eje
    public Vector3 dimensions;
    //Lista con los colores que va a tener el round
    public List<ColorCustom> randomColors = new List<ColorCustom>();
    //Lista con la posición de todos los enemigos
    Vector3[,,] enemysPositions;

    //Lista con los indices de X de los enemigos que se van a generar
    List<int> idxsX = new List<int>();
    //Lista con los indices de Y de los enemigos que se van a generar
    List<int> idxsY = new List<int>();
    //Lista con los indices de Z de los enemigos que se van a generar
    List<int> idxsZ = new List<int>();

    //Lista de las posibles posiciones  de X de los enemigos que se van a generar
    List<int> randomDimX = new List<int>();
    //Lista de las posibles posiciones  de Y de los enemigos que se van a generar
    List<int> randomDimY = new List<int>();
    //Lista de las posibles posiciones  de Z de los enemigos que se van a generar
    List<int> randomDimZ = new List<int>();
    //Lista con las posiciones de los enemigos que ya fueron generadas
    List<elements> elementos = new List<elements>();

    //Auxiliar para saber los indices de color que ya han sido creados
    List<int> idxsColorUsed = new List<int>();
    //El color ideal de cada enemigo
    public static  ColorCustom actualColor;

  


    /// <summary>
    /// Metodo por defecto de Unity que se corre segundos antes de cargar la escena
    /// </summary>
    private void Awake()
    {
       //Se generan las posiciones aleatorias de los enemigos
        SetEnemyPositions();
        //Se escogen solo algunos enemigos que se van a mostrar
        SetGoldIndexs();
        //Se aplica mas aleatorización a la listas
       SetRandomStacks();
    }



    /// <summary>
    /// Metodo que se correr por defecto en Unity al momento de Iniciar el juego
    /// </summary>
    void Start()
    {
        //Se suscribe al evento para saber cuando se inicia el juego
      GameEvent.instance.OnStartGame += StartSpawn;


    }
    /// <summary>
    /// Mezcla las listas para que queden en orden aleatorio
    /// </summary>

    void SetRandomStacks() {

        //Initializa cada una de las listas segun su dimensión
        InitializeList(ref randomDimX,(int)dimensions.x);
        InitializeList(ref randomDimY,(int)dimensions.y);
        InitializeList(ref randomDimZ,(int)dimensions.z);

        //Cambia las posiciones de cada lista de forma aleatoria
        randomDimX.Shuffle();
        randomDimY.Shuffle();
        randomDimZ.Shuffle();
    }
    /// <summary>
    /// Metodo por defecto de Unity que se ejecuta cuando el objetro se destruye
    /// </summary>
    private void OnDestroy()
    {
        //Cancela la suscripción para cuando se inicia el juego
        GameEvent.instance.OnStartGame -= StartSpawn;
    }
    /// <summary>
    /// Se inicializa las lista segun la dimension
    /// </summary>
    /// <param name="list">Lista con los indices</param>
    /// <param name="dim">tamaño maximo de la lista</param>
    void InitializeList(ref List<int> list, int dim)
    {
        //Se itera hasta el valor de dim añadiendo cada valor de i
        for (int i = 0; i < dim; i++)
        {
            //Añade "i" a la lista
            list.Add(i);
        }
    }
   /// <summary>
    /// Genera en escena los enemigos
    /// </summary>
    void StartSpawn()
    {
       //Inicia coroutina para generar  los enemigos cada cierto tiempo
        StartCoroutine(SpawnEnemys());
    }

    /// <summary>
    /// Instancia cada uno de los enemigos en escena
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemys()
    {
        //Total de enemigos generados
        int spawnedEnemies = 0;

        //Itera hasta que logre el maximo numero de enemigos definidos en el nivel
        while (spawnedEnemies <=maxNumEnemys)
            {
            //Va a guardar las posiciones x,y,z del elemento a generar
            elements elem;
            //Guarda la posicion en X
            elem.i = UnityEngine.Random.Range(0, (int)dimensions.x);
            //Guarda la posicion en Y
            elem.j = UnityEngine.Random.Range(0, (int)dimensions.y);
            //Guarda la posicion en Z
            elem.k = UnityEngine.Random.Range(0, (int)dimensions.z);

            //Si esa posicion ya existe debe continuar buscando otra
            if (elementos.Contains(elem)) continue;
            //Añade la nueva posicion a la lista de elementos ya generados
            elementos.Add(elem);
            //Si esa posicion no existe en la lista con los indices aleatoriso debe continuar
            if (!randomDimX.Contains(elem.i) && !randomDimY.Contains(elem.j) && !randomDimZ.Contains(elem.k)) continue;
            //Si los indices de X contienen ese elemento
            if (idxsX.Contains(elem.i) )
            {
                //Se borra de la lista de indices aleatorios
                randomDimX.Remove(elem.i);
            }
            //Si los indices de Y contienen ese elemento

            if (idxsY.Contains(elem.j))
            {
                //Se borra de la lista de indices aleatorios
                randomDimY.Remove(elem.j);
            }
            //Si los indices de Z contienen ese elemento

            if (idxsZ.Contains(elem.k))
            {
                //Se borra de la lista de indices aleatorios
                randomDimZ.Remove(elem.k);
            }
            //Cada 10 enemigos cambia el color al cual hay que disparar
            if (spawnedEnemies % 10 == 0)
            {
                 // si ya pasó por todo los posiles colores debe acabar el ciclo
                if (randomColors.Count == 0) break;
                //Valor del indice del color seleccionado
                int idxC;
               
                while (true)
                {  //Elige de forma aleatoria aun color
                    idxC = UnityEngine.Random.Range(0, randomColors.Count);
                    //Si la lista no lo contiene continua el ciclo
                    if (!idxsColorUsed.Contains(idxC))
                    {
                       //Cuando encuentra uno que no habia escogido antes termina el ciclo
                        idxsColorUsed.Add(idxC);
                        break;
                    }

                }

                //Notiifca a los observadores del evento cambiar enemigo de color el nuevo color seleccionado
                GameEvent.instance.ChangeEnemyColor(randomColors[idxC].color, randomColors[idxC].name);
                //Define el nuevo color seleccionado
                actualColor = randomColors[idxC];
                
              


            }
            //Toma indice aleatorio de la lista de colores
            int randomValue = UnityEngine.Random.Range(0, randomColors.Count);
            //Inicia coroutina para esperar por 3 segundos
            yield return new WaitForSeconds(3f);
            //Genera efecto de aparición
             GameObject effect = Instantiate(effectShowup, this.transform.position + enemysPositions[elem.i, elem.j, elem.k], Quaternion.identity);
            //Espera otro momento para generar ya los enemigos 
            yield return new WaitForSeconds(0.5f);
            //destruye el efecto
            Destroy(effect, 1);
            //Genera el enemigo  en escena en la posicion aleatoria y color aleatorio    
            GameObject enemy = Instantiate(prefabEnemy, this.transform.position + enemysPositions[elem.i, elem.j, elem.k], Quaternion.identity);
            Material matEnemy = enemy.GetComponent<Renderer>().material;
            matEnemy.SetColor("_Color", randomColors[randomValue] .color);
            matEnemy.SetColor("_EmissionColor", randomColors[randomValue].emissionColor);
            enemy.GetComponent<EnemyShape>().enemyColor = randomColors[randomValue].name;
            //Pone al enemigo como hijo de la clase principal
            enemy.transform.parent = gameObject.transform;
            //aumenta el numero de enemigos generados en uno
            spawnedEnemies += 1;

         


           
        }


       
        //Espera por un momento para darle tiempo de destruirlo
        yield return new WaitForSeconds(2);
    //Notifica a los suscriptores que el round ha terminado
        GameEvent.instance.RoundEnds();
    }

    /// <summary>
    /// Genera las posiciones Aleatorias
    /// </summary>
    void SetEnemyPositions()
    {
        //Crea matriz con las dimensiones seleccionadas
        enemysPositions = new Vector3[(int)dimensions.x, (int)dimensions.y,(int)dimensions.z];
        
        //Itera por todas las dimensiones de la matriz
        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                for (int k = 0; k < dimensions.z; k++)
                {
                    //Genera posición aleatoria 
                    enemysPositions[i,j,k] = GeneratePosition(i,j,k);
                }
            }
            
            
        }
    }
    /// <summary>
    /// Genera posicion aleatoria
    /// </summary>
    /// <param name="i">Valor de la posicion donde se a guardar la posicion aleatoria generada</param>
    /// <param name="j">Valor de la posicion donde se a guardar la posicion aleatoria generada</param>
    /// <param name="k">Valor de la posicion donde se a guardar la posicion aleatoria generada</param>
    /// <returns>Posicion aleatoria segun los valores de i,j,k</returns>
    Vector3 GeneratePosition(int i,int j, int k)
    {
        return new Vector3( i * EnemyShape.maximumSize.x, j * EnemyShape.maximumSize.y, k * EnemyShape.maximumSize.z);
    }
    /// <summary>
    /// Seleccionada los enemigos que se van a mostrar
    /// </summary>

    void SetGoldIndexs()
    {
     // A cada lista adiciona los enemigos que se van a seleccionar por cada eje
        AddIndexs(ref idxsX, (int)dimensions.x);
        AddIndexs(ref idxsY, (int)dimensions.y);
        AddIndexs(ref idxsZ, (int)dimensions.z);
    }

    /// <summary>
    /// Adiciona y genera el indice aleatorio que se va a escoger
    /// </summary>
    /// <param name="idxs">Lista con los indices</param>
    /// <param name="maxSize">Maximo número aleatorio a generar</param>
    void AddIndexs(ref List<int> idxs, int maxSize)
    {
        //Itera hasta la mitad de los elementos
        while (idxs.Count < maxSize/2)
        {
            //Genera indices aleatorios
            int idx = UnityEngine.Random.Range(0, maxSize);
            //Verifica si el elemento no esta en la lista
            if (!idxs.Contains(idx))
            {
                
                idxs.Add(idx);
            }
        }
    }

}
