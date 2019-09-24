using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if(gameObject.name.Equals("Nevera"))
        {
            CursorController.Instance.SetNeveraCursor();
        }
        else if (gameObject.name.Equals("mom"))
        {
            CursorController.Instance.SetMomCursor();
        }
        else if(gameObject.name.Equals("camaClick"))
        {
            CursorController.Instance.SetDormirCursor();
        }
    }

    private void OnMouseExit()
    {
        CursorController.Instance.SetMoveCursor();
    }
}
