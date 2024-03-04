using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private Color color;

    public bool CheckPositionInRoom(Vector2 position)
    {
        Vector2 halfSize = size /2;
        bool InRoomFromTop = position.y < transform.position.y + halfSize.y;
        bool inRoomFromBottom = position.y > transform.position.y - halfSize.y;
        bool inRoomFromLeft = position.x > transform.position.x - halfSize.x;
        bool inRoomFromRight = position.x < transform.position.x + halfSize.x;
        if (InRoomFromTop && inRoomFromBottom && inRoomFromLeft && inRoomFromRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, size);
    }
}
