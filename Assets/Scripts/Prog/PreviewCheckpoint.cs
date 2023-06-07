using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewCheckpoint : MonoBehaviour
{
    public GameObject winScreen;
    public BlocCodeCheck codeCheck;
    public float enemyTotal;
    [HideInInspector] public float enemyCount;

    public float checkpointTotal;
    [HideInInspector] public float checkpointCount;

    private void Start()
    {
        enemyCount = enemyTotal;
        checkpointCount = checkpointTotal;
    }
    private void Update()
    {
        if (enemyCount == 0 && checkpointCount == 0)
        {
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("ProgLvl", sceneIndex + 1);
        yield return new WaitForSeconds(2f);
        winScreen.SetActive(true);
        yield return null;
    }
}
