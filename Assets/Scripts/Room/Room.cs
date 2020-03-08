using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    [SerializeField]
    private GameObject _wallsFrontParent;
    private RoomTriggerCamera _triggerCam;
    private NavMeshSurface _surface;

    protected virtual void Start()
    {
        _triggerCam = GetComponentInChildren<RoomTriggerCamera>();
        _surface = GetComponentInChildren<NavMeshSurface>();
        _surface.BuildNavMesh();
    }

    public void EnterRoom(Vector3 camPos)
    {
        _wallsFrontParent.transform.DOMoveY(_wallsFrontParent.transform.position.y - 10f, 1f).SetEase(Ease.InOutBack, 1.2f, 0);
        Camera.main.GetComponent<CustomCamera>().GoToRoomSpot(camPos);
    }

    public void ExitRoom()
    {
        _wallsFrontParent.transform.DOMoveY(_wallsFrontParent.transform.position.y + 10f, .5f);
        Camera.main.GetComponent<CustomCamera>().FollowPlayer();
    }
}
