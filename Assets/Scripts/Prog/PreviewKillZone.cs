using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewKillZone : MonoBehaviour
{
    public BlocCodeCheck codeCheck;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            codeCheck.ResetPreview();
        }
        if(other.CompareTag("Player"))
        {
            codeCheck.playerIsDead = true;
        }
    }
}
