using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCheckpoint : MonoBehaviour
{
    public GameObject winScreen;
    public BlocCodeCheck codeCheck;
    [HideInInspector]public int nbOfPassage;
    public int nbOfPassageNeeded;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            nbOfPassage++;

            if (nbOfPassage == nbOfPassageNeeded)
            {
                if (codeCheck.player != null)
                {
                    if (codeCheck.playerIsDead)
                        winScreen.SetActive(true);
                }
                else
                {
                    winScreen.SetActive(true);
                }


            }
            
        }
    }
}
