using UnityEngine;

public sealed class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Shake Feel")]
    [SerializeField] private float traumaDecay = 2.5f;
    [SerializeField] private float maxPositionOffset = 0.12f;
    [SerializeField] private float maxRotationDegrees = 1.2f;
    [SerializeField] private float noiseFrequency = 28f;

    [Header("Scaling")]
    [SerializeField] private float comboBoost = 0.5f; // repeated hits stack a bit more

    private float trauma; // 0 to 1 normalized shake intensity
    private float noiseSeed;

    public Vector3 Offset { get; private set; }
    public float RotationZ { get; private set; } // degrees

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        noiseSeed = Random.value * 1000f;
    }

    public static void Trigger(float amount)
    {
        if (Instance == null) return;
        Instance.AddTrauma(amount);
    }

    // conveniently named "hits"
    public static void ShieldHit(float intensity = 1f)  => Trigger(0.45f * intensity);
    public static void PlayerHit(float intensity = 1f)   => Trigger(0.65f * intensity);
    public static void PlayerDeath(float intensity = 1f) => Trigger(0.75f * intensity);

    private void AddTrauma(float amount)
    {
        amount = Mathf.Max(0f, amount);

        // slight stacking boost if you’re already shaking (multi-hit feels stronger)
        float stackingMultiplier = 1f + trauma * comboBoost;
        trauma = Mathf.Clamp01(trauma + amount * stackingMultiplier);
    }

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;

        // fade shake out over time
        trauma = Mathf.Max(0f, trauma - traumaDecay * deltaTime);

        float intensity = trauma;

        // smooth noise signal (Perlin) that changes over time
        float noiseTime = (Time.unscaledTime + noiseSeed) * noiseFrequency;

        float horizontalNoise = Mathf.PerlinNoise(noiseTime, 0.1f) * 2f - 1f;
        float verticalNoise = Mathf.PerlinNoise(0.2f, noiseTime) * 2f - 1f;
        float rotationNoise = Mathf.PerlinNoise(noiseTime, noiseTime) * 2f - 1f;

        Offset = new Vector3(horizontalNoise, verticalNoise, 0f) * (maxPositionOffset * intensity);
        RotationZ = rotationNoise * (maxRotationDegrees * intensity);
    }
}
