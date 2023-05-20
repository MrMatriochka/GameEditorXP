using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public SceneAsset scene;
    public Animator anim;
    
    public void Transition()
    {
        anim.SetBool("Started", true);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(scene.name);
    }
}
