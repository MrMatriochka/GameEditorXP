using System;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.Scripts
{
     [RequireComponent(typeof(Animator))]
     public class EventOnAnimEnd : MonoBehaviour
     {
          public UnityEvent onAnimationEnd;
          private Animator anim;
          private int poofIndexID;

          private void Start()
          {    
               anim = GetComponent<Animator>();
               poofIndexID = Animator.StringToHash("PoofIndex");
               
               anim.SetInteger(poofIndexID, 1);
          }

          private void Update()
          {
               if (anim.GetInteger(poofIndexID) > 0 && anim.playbackTime > 0.99f)
               {
                    onAnimationEnd.Invoke();
               }
          }
     }
}
