using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocFunction : MonoBehaviour
{
    public enum Function
    {
        Avancer,
        Flip,
        PertePV,
        IfObstacle,
        IfToucheLeJoueur,
        EndIf
    }

    public Function function;
}
