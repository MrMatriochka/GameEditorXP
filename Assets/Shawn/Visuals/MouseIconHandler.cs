using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseIconHandler : MonoBehaviour
{
    [SerializeField] private Texture2D cursorDefault;
    [SerializeField] private Texture2D cursorPointer;
    [SerializeField] private Texture2D cursorHandOpen;
    [SerializeField] private Texture2D cursorHandHold;


    public enum CursorState
    {
        Default,
        Pointer,
        HandOpen,
        HandHold
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }

    public void ChangeCursor(GetMouseButtonEnum e)
    {
        switch (e.state)
        {
            case CursorState.Default: Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.Pointer: Cursor.SetCursor(cursorPointer, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.HandOpen: Cursor.SetCursor(cursorHandOpen, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.HandHold: Cursor.SetCursor(cursorHandHold, Vector2.zero, CursorMode.Auto);
                break;
        }
        
    }
}
