using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : Room
{
    [SerializeField]
    private GameObject a;

    protected override void Start()
    {
        base.Start();
    }

    public override void StartRoom()
    {
        base.StartRoom();
        GameManager.Instance.Player.Room2();

    }

}
