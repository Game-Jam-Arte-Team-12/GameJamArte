using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private GameObject _wallsFrontParent;
    [SerializeField]
    private GameObject _floorExit;
    private RoomTriggerCamera _triggerCam;

    private void Start()
    {
        _triggerCam = GetComponentInChildren<RoomTriggerCamera>();
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

    public void DetachFloor()
    {
        _floorExit.transform.SetParent(null);
    }


}
