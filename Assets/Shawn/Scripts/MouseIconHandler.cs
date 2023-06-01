using System;
using UnityEngine;

namespace Shawn.Scripts
{
    public class MouseIconHandler : MonoBehaviour
    {
        public static MouseIconHandler Instance { get; private set; }

        [SerializeField] private Texture2D cursorDefault;
        [SerializeField] private Texture2D cursorPointer;
        [SerializeField] private Texture2D cursorHandOpen;
        [SerializeField] private Texture2D cursorHandHold;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = true;
            SetCursorDefault();
        }

        public void SetCursorDefault()
        {
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        }
        
        public void SetCursorPointer()
        {
            Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
        }
        
        public void SetCursorHandOpen()
        {
            Cursor.SetCursor(cursorHandOpen, Vector2.zero, CursorMode.Auto);
        }
        
        public void SetCursorHandHold()
        {
            Cursor.SetCursor(cursorHandHold, Vector2.zero, CursorMode.Auto);
        }
    }
}
