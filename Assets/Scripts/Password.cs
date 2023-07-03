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
            lerp.StartCoroutine(lerp.FadeOut());
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void AlreadyDone()
    {
        if (done)
        {
            lerp.StartCoroutine(lerp.FadeOut());
            if(transform.childCount>0) transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Loading()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            return;
        }
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (PlayerPrefs.HasKey("ProgLvl"))
            {
                if (PlayerPrefs.GetInt("ProgLvl") < SceneManager.sceneCountInBuildSettings+1)
                {
                    SceneManager.LoadScene(PlayerPrefs.GetInt("ProgLvl"));
                }
                else
                    SceneManager.LoadScene(PlayerPrefs.GetInt("ProgLvl") - 1);

            }
            else
            {
                SceneManager.LoadScene(1 + SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("ProgLvl", sceneIndex + 1);
        if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
            SceneManager.LoadScene(1);
    }
}
