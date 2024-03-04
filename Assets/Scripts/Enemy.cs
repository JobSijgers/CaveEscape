using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject[] swords;
    private List<Vector2> path = new();
    private List<Vector2> walkableTiles;
    private List<Vector2> nonWalkableTiles;
    private MetronomeManager metronomeManager;
    private bool chasing;

    void Start()
    {
        metronomeManager = MetronomeManager.Instance;
        walkableTiles = FindObjectOfType<NodeManager>().floorPositions;
        nonWalkableTiles = FindObjectOfType<NodeManager>().wallPositions;
        metronomeManager.MetronomeTickEvent += OnMetronomeTick;
    }
    private void OnDisable()
    {
        metronomeManager.MetronomeTickEvent -= OnMetronomeTick;
    }

    #region Pathfinding
    private void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Queue<Vector2> queue = new Queue<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();
        Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>();
        queue.Enqueue(startPos);

        while (queue.Count > 0)
        {
            Vector2 currentTile = queue.Dequeue();
            visited.Add(currentTile);

            if (currentTile == targetPos)
            {
                // Path found, reconstruct and return it
                path = RetracePath(startPos, targetPos, parentMap);

                return;
            }

            List<Vector2> neighbors = GetNeighbors(currentTile);
            foreach (Vector2 neighbor in neighbors)
            {
                if (visited.Contains(neighbor) || queue.Contains(neighbor))
                    continue;

                queue.Enqueue(neighbor);
                parentMap[neighbor] = currentTile;
            }
        }

        Debug.Log("Path not found!");
    }

    private List<Vector2> RetracePath(Vector2 startTile, Vector2 endTile, Dictionary<Vector2, Vector2> parentMap)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            if (parentMap.ContainsKey(currentTile))
            {
                currentTile = parentMap[currentTile];
            }
            else
            {
                Debug.LogError("Path is broken");
                break;
            }
        }

        path.Reverse();
        return path;
    }

    private List<Vector2> GetNeighbors(Vector2 tile)
    {
        List<Vector2> neighbors = new List<Vector2>();
        Vector2[] possibleNeighbors = new Vector2[]
        {
            new Vector2(tile.x + 1, tile.y),
            new Vector2(tile.x - 1, tile.y),
            new Vector2(tile.x, tile.y + 1),
            new Vector2(tile.x, tile.y - 1)
        };

        foreach (Vector2 neighbor in possibleNeighbors)
        {
            if (walkableTiles.Contains(neighbor) && !nonWalkableTiles.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
    #endregion
    private void OnMetronomeTick(EMetronomeTick tick)
    {
        if (tick != EMetronomeTick.Enemy)
            return;
        if (!chasing)
        {
            if (RoomManager.instance.GetRoomWithPosition(transform.position) != RoomManager.instance.GetRoomWithPosition(target.position))
                return;
        }
        chasing = true;
        path.Clear();
        FindPath(transform.position, target.position);
        if (path.Count > 15)
        {
            path.Clear();
            return;
        }
        else if (path.Count > 1)
        {
            transform.position = path[0];
        }
        else
        {
            AttackPlayer();
        }

    }
    private void AttackPlayer()
    {
        //attack up
        if (target.position.y > transform.position.y)
        {
            Attack(0, Vector2.up);
        }
        //attack left
        else if (target.position.x < transform.position.x)
        {
            Attack(1, Vector2.left);
        }
        //attack down
        else if (target.transform.position.y < transform.position.y)
        {
            Attack(2, Vector2.down);
        }
        //attack right
        else
        {
            Attack(3, Vector2.right);
        }
    }
    private void Attack(int swordIndex, Vector2 direction)
    {
        swords[swordIndex].SetActive(true);
        StartCoroutine(DeactivateGameObjectAfterDelay(swords[swordIndex], .2f));

        RaycastHit2D result = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Player"));
        if (result.transform == null)
        {
            return;
        }
        if (result.transform.TryGetComponent(out IDamageable damageble))
        {
            damageble?.TakeDamage(1);
        }
    }
    private IEnumerator DeactivateGameObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        foreach (var tile in path)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(tile, new Vector2(.4f, .4f));
        }
    }

}
