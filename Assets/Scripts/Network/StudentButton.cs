using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StudentButton : MonoBehaviour
{
     public ProfNetworkManager managerRef;
    [HideInInspector] public int id;
    public TMP_Text username;
    private void Start()
    {
        username.text = managerRef.groupData[id].username;
    }

    public void LoadButton()
    {
        managerRef.LoadLevel(id);
    }
}
