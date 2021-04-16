using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    public static int actualRound = 0;
    void Start()
    {
        GameEvent.instance.OnRoundEnds += RoundEnds;
    }

    public void RoundEnds()
    {
        actualRound += 1;
    }


}
