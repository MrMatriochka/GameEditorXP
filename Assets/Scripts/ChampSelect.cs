using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampSelect : MonoBehaviour
{
    public PrefabInButton button;
    public GameObject player;
    public BuildingManager manager;

    public void ChoosePrefab(GameObject prefab)
    {
        button.prefab = prefab;

        if(player != null)
        {
            GameObject newPlayer = Instantiate(prefab, player.transform.position, player.transform.rotation);
            manager.placedObject.Add(newPlayer);

            int index = manager.placedObject.IndexOf(player);
            manager.placedObject.RemoveAt(index);
            Destroy(player);
        }
        
    }
}
