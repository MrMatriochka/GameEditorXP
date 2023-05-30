using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindPlayer : MonoBehaviour
{
    public ChampSelect champSelect;
    public Image button;
    public GameObject prefab;
    void Start()
    {
        champSelect.player = gameObject;
        button.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        button.gameObject.GetComponent<PrefabInButton>().prefab = prefab.GetComponent<PrefabInButton>().prefab;
    }

}
