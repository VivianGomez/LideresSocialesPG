using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private void OnMouseEnter()
    {
        CursorController.Instance.SetClickableCursor();
    }

    private void OnMouseExit()
    {
        CursorController.Instance.SetMoveCursor();
    }
}
