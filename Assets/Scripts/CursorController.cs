using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Texture2D momCursor;
    [SerializeField]
    private Texture2D neveraCursor;
    [SerializeField]
    private Texture2D moveCursor;

    public static CursorController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if(hitInfo.collider.GetComponent<ClickableObject>() != null)
            {
                SetClickableCursor();
            }
            else
            {
                SetMoveCursor();
            }
        }
    }

    public void SetMoveCursor()
    {
        Cursor.SetCursor(moveCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetMomCursor()
    {
        Cursor.SetCursor(momCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetNeveraCursor()
    {
        Cursor.SetCursor(neveraCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetClickableCursor()
    {
        Cursor.SetCursor(momCursor, Vector2.zero, CursorMode.Auto);
    }

}
