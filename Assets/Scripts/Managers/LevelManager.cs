using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [HideInInspector]
    public Room ActualRoom;

    [SerializeField]
    private GameObject _roomsParent;
    [SerializeField]
    private List<Room> _rooms;

    private int _roomIndex = 0;

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
        SpawnLevel();
    }

    public void NextLevel()
    {
        _roomIndex++;
        ActualRoom.DetachFloor();
        Destroy(ActualRoom.gameObject);
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        if (_roomIndex >= _rooms.Count) _roomIndex = 0;
        ActualRoom = Instantiate(_rooms[_roomIndex], _roomsParent.transform);
        ActualRoom.transform.position = Vector3.zero;
    }
}
