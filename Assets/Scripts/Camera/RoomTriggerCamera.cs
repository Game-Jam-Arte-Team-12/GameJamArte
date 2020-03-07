using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomTriggerCamera : MonoBehaviour
{
    public Color GizColor = Color.blue;

    [HideInInspector]
    public Transform RoomSpot;
    private Room Room;

    private void Start()
    {
        RoomSpot = transform.GetChild(0);
        Room = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Room.EnterRoom(RoomSpot.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Room.ExitRoom();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizColor;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
