using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
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

    public void Interact()
    {
        Action();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.Player.AddInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Player.RemoveInteractable(this);
        }
    }
}
