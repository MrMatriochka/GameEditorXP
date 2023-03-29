using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocFunction : MonoBehaviour
{
    public enum Function
    {
        Avancer,
        Flip,
        IfPasJoueur,
        Boucle,
        EndIf
    }

    public Function function;
}
