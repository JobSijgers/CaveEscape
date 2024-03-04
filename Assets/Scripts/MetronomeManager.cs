using UnityEngine;
public class MetronomeManager : MonoBehaviour
{
    public static MetronomeManager Instance { get; private set; }

    public delegate void MetronomeTick(EMetronomeTick tick);
    public event MetronomeTick MetronomeTickEvent;

    [SerializeField] private float baseMetronomeInterval;
    [SerializeField] private Transform pendulum;
    [SerializeField] private AnimationCurve pendulumCurve;

    private EMetronomeTick currentTick;

    private float timer;
    private float metronomeInterval;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        metronomeInterval = baseMetronomeInterval;
    }
    private void Update()
    {
        

        if (GameManager.instance.currentState == EGameSate.Playing)
        {
            timer += Time.deltaTime;
            MovePendulum();
            if (timer < metronomeInterval)
                return;

            MetronomeTickEvent?.Invoke(currentTick);

            AudioManager.Instance.Play("Tik");
            if (currentTick == EMetronomeTick.Player)
            {
                currentTick = EMetronomeTick.Enemy;
            }
            else
            {
                currentTick = EMetronomeTick.Player;
            }
            timer = 0;
        }
        
    }

    private void MovePendulum()
    {
        if (currentTick == EMetronomeTick.Player)
        {
            float t = pendulumCurve.Evaluate(timer);
            pendulum.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, 45), t / metronomeInterval);
        }
        else
        {
            float t = pendulumCurve.Evaluate(timer);
            pendulum.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 45), Quaternion.Euler(0, 0, -45), t / metronomeInterval);
        }
    }
}
