using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [HideInInspector]
    public Room ActualRoom;

    [SerializeField]
    private GameObject _levelDesignPrefab;
    private GameObject _levelDesign;

    private List<Room> _rooms;

    //private int _roomIndex = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Init()
    {
        _rooms = new List<Room>();

        _levelDesign = Instantiate(_levelDesignPrefab, Vector3.zero, Quaternion.identity);
        _levelDesign.transform.position += new Vector3(0, -10, 0);
        _levelDesign.transform.DOMoveY(0, 2f).OnComplete(() =>
        {
            foreach (Room room in _levelDesign.GetComponentsInChildren<Room>())
            {
                room.RefreshNavMesh();
            }
            GameManager.Instance.Player.ActiveAgent();
        });
        foreach (Room room in _levelDesign.GetComponentsInChildren<Room>())
        {
            _rooms.Add(room);
        }
        AccessNewRoom();

    }

    public void NextLevel()
    {
        _rooms.Remove(ActualRoom);
        Destroy(ActualRoom.gameObject);
        AccessNewRoom();
    }

    private void AccessNewRoom()
    {
        ActualRoom = _rooms[0];
    }
}
