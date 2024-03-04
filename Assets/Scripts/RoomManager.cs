using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    [SerializeField] private Room[] rooms;

    private void Awake()
    {
        instance = this;
    }
    public Room GetRoomWithPosition(Vector2 position)
    {
        foreach (Room room in rooms)
        {
            if (room.CheckPositionInRoom(position))
            {
                return room;
            }
        }
        return null;
    }
}

