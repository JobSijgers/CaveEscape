using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour, IDamageable
{
    [SerializeField] private SpriteRenderer leftDoor;
    [SerializeField] private SpriteRenderer rightDoor;

    [SerializeField] private Sprite[] leftDoorStates;
    [SerializeField] private Sprite[] rightDoorStates;
    private int currentState;

    private void Start()
    {
        currentState = leftDoorStates.Length - 1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState <= 0)
            return;
        currentState-= damage;
        if (currentState == 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            SetSpriteState();
            return;
        }
        SetSpriteState();

    }

    private void SetSpriteState()
    {
        leftDoor.sprite = leftDoorStates[currentState];
        rightDoor.sprite = rightDoorStates[currentState];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState <= 0)
        {
            GameManager.instance.WonGame();
        }
    }
}
