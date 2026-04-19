using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.WSA;

public class Firework : MonoBehaviour
{
    public GameObject ExplosionParticles;
    public GameObject FuseStartupParticles;
    public GameObject TrailParticles;

    public ParticleSystem CreatedTrailParticles;

    public FireworkLauncher Launcher;
    // define state there but every object is managed by the launcher OR IS IT

    public Transform FuseAnchor;

    public float Lifetime;
    public float InitialLifetime;

    public Vector3 Direction = Vector3.up;

    public float Speed;
    public float Acceleration;

    public bool Launched = false;
    public bool IsAlive = true;
    public bool YWobbleStarted = false;

    // Wobbling
    public float FrequencyX = 5f;
    public float PhaseX = 0f;
    public float AmplitudeX = 1f;

    public float FrequencyY = 0.5f;
    public float PhaseY = 0f;
    public float AmplitudeY = 1f;
    public float MaxFrequencyY = 2f;

    public float TimeUntilYWobbleStartsSeconds; 

    public float WobbleLifetimeQuantity = 0.1f;

    public float LastSineOffsetX;

    public Vector3 DirectionAtWobbleStart;

    // will change over time
    // frequency will the the function of 

    void Start()
    {
        
    }

    // It's all over the place, i will figure out the proper structure later
    void Update()
    {
        if (!Launched)
            return;
        if(!IsAlive)
            return;

        if (Speed > 0)
            Speed += Acceleration * Time.deltaTime;
        
        Speed = Mathf.Max(0f, Speed);

        Acceleration -= Launcher.Drag * Time.deltaTime;

        var elapsed = InitialLifetime - Lifetime;
        if (TimeUntilYWobbleStartsSeconds <= 0 && !YWobbleStarted)
        {
            DirectionAtWobbleStart = Direction;
            YWobbleStarted = true;
        }

        float sineOffsetX = Mathf.Sin(elapsed * FrequencyX + PhaseX) * AmplitudeX;
        float sineOffsetY = 1f;
        if (YWobbleStarted)
        {
            sineOffsetX = DirectionAtWobbleStart.x + Mathf.Sin(elapsed * FrequencyX + PhaseX) * AmplitudeX;
            sineOffsetY = DirectionAtWobbleStart.y + Mathf.Sin(elapsed * FrequencyY + PhaseY) * AmplitudeY;
        }

        Direction = new Vector3(sineOffsetX, sineOffsetY, 0f).normalized;

        transform.position += Direction * Speed / G.main.MetersPerUnit * Time.deltaTime;
        transform.up = Direction;

        Launcher.EvaluateAndShowStats(this);

        if(Lifetime <= 0f)
            AskForExplosion();

        //FrequencyY += FrequencyYPerSecond * Time.deltaTime;
        FrequencyY = Mathf.Min(FrequencyY, MaxFrequencyY);

        Lifetime -= Time.deltaTime;
        TimeUntilYWobbleStartsSeconds -= Time.deltaTime;
        LastSineOffsetX = sineOffsetX;
    }
    public void AskForExplosion()
    {
        IsAlive = false;
        Launcher.EvaluateExplosion(this);
    }
    public void Explode()
    {
        CreatedTrailParticles.Stop();
        var explosionParticles = Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
        AudioController.Instance.PlaySound3D("Explosion", transform.position);
        Destroy(gameObject);
    }
    public void Launch()
    {
        Launched = true;
        CreatedTrailParticles = Instantiate(TrailParticles, FuseAnchor).GetComponent<ParticleSystem>();
        // Play some particles
    }

    public void Setup(FireworkLauncher launcher)
    {
        Launcher = launcher;

        Lifetime = UnityEngine.Random.Range(launcher.MinLifetimeSeconds,
         launcher.MinLifetimeSeconds + launcher.UpperLifetimeVariationSeconds);
        InitialLifetime = Lifetime;

        Speed = launcher.StartSpeed;
        Acceleration = launcher.StartingAcceleration;

        PhaseX = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        PhaseY = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        //TimeUntilYWobbleStartsSeconds = Lifetime - WobbleLifetimeQuantity;
        TimeUntilYWobbleStartsSeconds = 0f;
        // play some charging animation, like shake
    }
}
