using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectCursor : MonoBehaviour
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
        else if(gameObject.name.Equals("noticias") || gameObject.name.Equals("ButtonNews"))
        {
            CursorController.Instance.SetInfoCursor();
        }
        else if(gameObject.name.Equals("close"))
        {
            CursorController.Instance.SetCloseCursor();
        }
    }

    private void OnMouseExit()
    {
        CursorController.Instance.SetMoveCursor();
    }
}
