using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Password : MonoBehaviour
{
    public string password;
    public bool done;
    public LerpCurve lerp;
    private void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "password"))
            done = true;
    }
    public void CheckPassword(string text)
    {
        if(password == text)
        {
            done = true;
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "password", 1);
            lerp.StartCoroutine(lerp.Lerp());
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void AlreadyDone()
    {
        if (done)
        {
            lerp.StartCoroutine(lerp.Lerp());
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Loading()
    {
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(i);
    }
}
