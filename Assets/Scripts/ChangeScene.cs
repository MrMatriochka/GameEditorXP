using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToScene()
    {
        if(PlayerPrefs.HasKey("ProgLvl"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("ProgLvl"));
        }
        else
        {
            SceneManager.LoadScene(1 + SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartScene()
    {
        SceneManager.LoadScene(1 + SceneManager.GetActiveScene().buildIndex);
    }
}
