using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvent.instance.OnRoundEnds += RoundEnds;
    }

    public void RoundEnds()
    {
        print("Round ended");
    }


}
