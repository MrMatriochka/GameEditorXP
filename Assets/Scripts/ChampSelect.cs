using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampSelect : MonoBehaviour
{
    public PrefabInButton button;

    public void ChoosePrefab(GameObject prefab)
    {
        button.prefab = prefab;
    }
}
