using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Tooltip("The swords schould be in the same order as the EDirecion enum")]
    [SerializeField] private GameObject[] swords;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Image moveUI;
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite arrowSprite;
    private MetronomeManager metronomeManager;
    private EPlayerTickOptions tickOption;
    private SpriteRenderer spriteRenderer;
    private bool firstInput;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        metronomeManager = MetronomeManager.Instance;
        metronomeManager.MetronomeTickEvent += OnMetronomeTick;
    }
    private void OnDisable()
    {
        metronomeManager.MetronomeTickEvent -= OnMetronomeTick;
    }
    private void Update()
    {
        GetPlayerInput();
    }
    private void GetPlayerInput()
    {
        SetTickOption(KeyCode.W, EPlayerTickOptions.WalkUp);
        SetTickOption(KeyCode.A, EPlayerTickOptions.WalkLeft);
        SetTickOption(KeyCode.S, EPlayerTickOptions.WalkDown);
        SetTickOption(KeyCode.D, EPlayerTickOptions.WalkRight);
        SetTickOption(KeyCode.I, EPlayerTickOptions.AttackUp);
        SetTickOption(KeyCode.J, EPlayerTickOptions.AttackLeft);
        SetTickOption(KeyCode.K, EPlayerTickOptions.AttackDown);
        SetTickOption(KeyCode.L, EPlayerTickOptions.AttackRight);
    }
    private void SetTickOption(KeyCode key, EPlayerTickOptions option)
    {
        if (Input.GetKeyDown(key))
        {
            tickOption = option;
            if (!firstInput)
            {
                firstInput = true;
                moveUI.color = new Color(1, 1, 1, 1);
            }
            switch (tickOption)
            {
                case EPlayerTickOptions.WalkUp:
                    moveUI.sprite = arrowSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case EPlayerTickOptions.WalkLeft:
                    moveUI.sprite = arrowSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case EPlayerTickOptions.WalkDown:
                    moveUI.sprite = arrowSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case EPlayerTickOptions.WalkRight:
                    moveUI.sprite = arrowSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case EPlayerTickOptions.AttackUp:
                    moveUI.sprite = swordSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case EPlayerTickOptions.AttackLeft:
                    moveUI.sprite = swordSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case EPlayerTickOptions.AttackDown:
                    moveUI.sprite = swordSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case EPlayerTickOptions.AttackRight:
                    moveUI.sprite = swordSprite;
                    moveUI.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                default:
                    break;
            }
        }
    }
    private void OnMetronomeTick(EMetronomeTick tick)
    {

        if (tick != EMetronomeTick.Player)
            return;
        switch (tickOption)
        {
            case EPlayerTickOptions.WalkUp:
                MovePlayer(Vector2.up);
                spriteRenderer.sprite = backSprite;
                spriteRenderer.flipX = true;
                break;

            case EPlayerTickOptions.WalkLeft:
                MovePlayer(Vector2.left);
                spriteRenderer.sprite = frontSprite;
                spriteRenderer.flipX = true;
                break;

            case EPlayerTickOptions.WalkDown:
                MovePlayer(Vector2.down);
                spriteRenderer.sprite = frontSprite;
                break;

            case EPlayerTickOptions.WalkRight:
                MovePlayer(Vector2.right);
                spriteRenderer.sprite = frontSprite;
                spriteRenderer.flipX = false;
                break;
            case EPlayerTickOptions.AttackUp:
                Attack(0, Vector2.up);
                spriteRenderer.sprite = backSprite;
                spriteRenderer.flipX = true;
                break;

            case EPlayerTickOptions.AttackLeft:
                Attack(1, Vector2.left);
                spriteRenderer.sprite = frontSprite;
                spriteRenderer.flipX = true;
                break;

            case EPlayerTickOptions.AttackDown:
                Attack(2, Vector2.down);
                spriteRenderer.sprite = frontSprite;
                break;

            case EPlayerTickOptions.AttackRight:
                Attack(3, Vector2.right);
                spriteRenderer.sprite = frontSprite;
                spriteRenderer.flipX = false;
                break;

            case EPlayerTickOptions.None:
                break;
        }
    }
    private void MovePlayer(Vector2 direction)
    {
        if (!Physics2D.Raycast(transform.position, direction, 1f, wallLayer))
        {
            transform.Translate(direction);
        }
    }
    private void Attack(int swordIndex, Vector2 direction)
    {
        swords[swordIndex].SetActive(true);
        StartCoroutine(DeactivateGameObjectAfterDelay(swords[swordIndex], .2f));

        RaycastHit2D result = Physics2D.Raycast(transform.position, direction, 1f, ~LayerMask.GetMask("Player"));
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
   
}
