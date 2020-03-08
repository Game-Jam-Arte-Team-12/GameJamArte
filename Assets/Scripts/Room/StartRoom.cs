using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LevelManager.Instance.ActualRoom.StartRoom();
        }
    }
}
