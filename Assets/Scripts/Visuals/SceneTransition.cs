using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [Header("Transition Settings")]    
    public Animator anim;    
    public float speed = 1;

    [Header("Loader Settings")]
    public SceneAsset scene;
    
    public void Transition()
    {
        anim.SetBool("Started", true);
        anim.speed = speed;
    }

    public void LoadScene()
    {
        if(scene != null)
            SceneManager.LoadScene(scene.name);
        else anim.SetBool("Started", false);
    }
}
