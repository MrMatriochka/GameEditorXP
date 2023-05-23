using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Visuals
{
    static class Easings // Should probably be exported...
    {
        public enum Type
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut
        }
        public static float EaseIn(float t) { return t * t; }
        public static float Flip(float t) { return 1 - t; }
        public static float EaseOut(float t) { return Flip(Mathf.Pow(Flip(t), 2)); }
        public static float EaseInOut(float t) { return Mathf.Lerp(EaseIn(t), EaseOut(t), t); }
    }
    
    public class SceneTransition : MonoBehaviour
    {
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
    
        [Header("Transition Settings")] 
        [SerializeField] private Direction direction;
        [SerializeField] private float duration = 1;
        [SerializeField] private Easings.Type easing;
        [SerializeField] private bool reverseTransitionOut = true;
        [SerializeField] private bool reverseEasingOut = true;

        //public Animator anim;    

        [Header("Loader Settings")]
        [SerializeField] private SceneAsset scene;

        private bool inPlaying = false;
        private bool outPlaying = true;
        private RectTransform imageTransform;
        private Vector2 basePosition;
        private Vector2 basePivot;
        private float lerpElapsedTime;

        private void Start()
        {
            lerpElapsedTime = duration;
            imageTransform = GetComponent<RectTransform>();
            if (!reverseTransitionOut) InvertDirection();
            if (!reverseEasingOut) InvertEasing();
            else InitTargetPositions();
        }

        private void InitTargetPositions()
        {
            switch(direction)
            {
                case Direction.Left:
                    basePosition = new Vector2(Screen.width / 2, 0);
                    basePivot = new Vector2(0, 0.5f);
                    break;
                case Direction.Right:
                    basePosition = new Vector2(0 - Screen.width / 2, 0);
                    basePivot = new Vector2(1, 0.5f);
                    break;
                case Direction.Up:
                    basePosition = new Vector2(0, 0 - Screen.height/2);
                    basePivot = new Vector2(0.5f, 1);
                    break;
                case Direction.Down:
                    basePosition = new Vector2(0, Screen.height/2);
                    basePivot = new Vector2(0.5f, 0);
                    break;
            }
        }

        public void Transition()
        {
            InitTargetPositions();
            imageTransform.pivot = basePivot;
            imageTransform.localPosition = basePosition;
            
            outPlaying = false;
            inPlaying = true;
        }

        private float GetLerpInterpolation()
        {
            return easing switch
            {
                Easings.Type.Linear => lerpElapsedTime / duration,
                Easings.Type.EaseIn => Easings.EaseIn(lerpElapsedTime / duration),
                Easings.Type.EaseOut => Easings.EaseOut(lerpElapsedTime / duration),
                Easings.Type.EaseInOut => Easings.EaseInOut(lerpElapsedTime / duration),
                _ => lerpElapsedTime / duration
            };
        }
        
        private void Update()
        {
            float lerpInterpolation = GetLerpInterpolation();
            
            if (!inPlaying && !outPlaying) return;
            imageTransform.pivot = Vector2.Lerp(basePivot, new Vector2(0.5f, 0.5f), lerpInterpolation);
            imageTransform.localPosition = Vector2.Lerp(basePosition, new Vector2(0, 0), lerpInterpolation);
                
            if (inPlaying)
            {
                if (lerpElapsedTime < 0) lerpElapsedTime = 0;
                lerpElapsedTime += Time.deltaTime;
                if (lerpElapsedTime > duration) LoadScene();
            } else if (outPlaying)
            {
                if (lerpElapsedTime > duration) lerpElapsedTime = duration;
                lerpElapsedTime -= Time.deltaTime;
                if (lerpElapsedTime <= 0)
                {
                    if (!reverseTransitionOut) InvertDirection();
                    if (!reverseEasingOut) InvertEasing();
                    imageTransform.pivot = basePivot;
                    imageTransform.localPosition = basePosition;
                    outPlaying = false;
                }
            }
        }

        private void InvertDirection()
        {
            direction = direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                _ => direction
            };
            InitTargetPositions();
        }
        
        private void InvertEasing()
        {
            easing = easing switch
            {
                Easings.Type.EaseIn => Easings.Type.EaseOut,
                Easings.Type.EaseOut => Easings.Type.EaseIn,
                _ => easing
            };
        }

        private void LoadScene()
        {
            inPlaying = false;
            
            if (scene != null)
                SceneManager.LoadScene(scene.name);
            else
            {
                if (!reverseTransitionOut) InvertDirection();
                if (!reverseEasingOut) InvertEasing();
                outPlaying = true;
                lerpElapsedTime = duration;
            }
        }
    }
}
