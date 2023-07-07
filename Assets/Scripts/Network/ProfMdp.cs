using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProfMdp : MonoBehaviour
{
    public Image image;
    public string password;

    private void Start()
    {
        if (PlayerPrefs.HasKey("isProf"))
        {
            image.color = Color.green;
        }
    }
    public void CheckMdp(string Mdp)
    {
        if(Mdp == password)
        {
            image.color = Color.green;
            PlayerPrefs.SetInt("isProf", 1);
            GetComponent<TMP_InputField>().text = "";
        }
        else
        {
            GetComponent<TMP_InputField>().text = "";
        }
    }
}
