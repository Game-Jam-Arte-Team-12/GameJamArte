using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected delegate void DoAction();

    protected virtual void Action() { }

    void OnMouseDown()
    {
        Action();
    }

    void OnMouseOver()
    {
        GameManager.Instance.ChangeCursor(CursorTypes.Interact);
    }

    void OnMouseExit()
    {
        GameManager.Instance.ChangeCursor(CursorTypes.Normal);
    }
}
