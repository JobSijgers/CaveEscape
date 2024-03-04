using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private GameManager manager;
    [SerializeField] private Vector2 deathScreenPosition;
    [SerializeField] private Vector2 winScreenPosition;

    private Vector3 velocity = Vector3.zero;
    private Vector2 menuPosition;
    private bool movingToMenu = false;
    private bool movingToDeathScreen;
    private bool movingToWinScreen;
    private void Start()
    {
        menuPosition = transform.position;
    }

    private void Update()
    {
        if (movingToMenu)
            return;
        if (movingToDeathScreen)
            return;
        if (movingToWinScreen)
            return;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, target.position, ref velocity, speed);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
    public IEnumerator MoveToMenu()
    {
        movingToMenu = true;
        while (Vector2.Distance(transform.position, menuPosition) > 0.01f)
        {
            Vector3 newPos = Vector3.SmoothDamp(transform.position, menuPosition, ref velocity, speed);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            yield return null;
            if (Vector2.Distance(transform.position, menuPosition) < 0.3f)
            {
                manager.EnableMenu();
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public IEnumerator MoveToDeathScreen()
    {
        movingToDeathScreen = true;
        while (Vector2.Distance(transform.position, deathScreenPosition) > 0.01f)
        {
            Vector3 newPos = Vector3.SmoothDamp(transform.position, deathScreenPosition, ref velocity, speed);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            yield return null;
            if (Vector2.Distance(transform.position, deathScreenPosition) < 0.4f)
            {
                manager.EnableDeathScreen();
            }
        }
    }

    public IEnumerator MoveToWinScreen()
    {
        movingToWinScreen = true;
        while (Vector2.Distance(transform.position, winScreenPosition) > 0.01f)
        {
            Vector3 newPos = Vector3.SmoothDamp(transform.position, winScreenPosition, ref velocity, speed);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            yield return null;
            if (Vector2.Distance(transform.position, winScreenPosition) < 0.3f)
            {
                manager.EnableWinscreen();
            }
        }
    }
    
}
