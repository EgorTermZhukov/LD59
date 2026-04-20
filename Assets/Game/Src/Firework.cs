using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class Firework : MonoBehaviour
{
    public GameObject ExplosionParticles;
    public GameObject FuseStartupParticles;
    public GameObject TrailParticles;

    public ParticleSystem CreatedTrailParticles;

    public FireworkBox FireworkBox;
    public FireworkLauncher Launcher;

    public Transform FuseAnchor;

    public float Lifetime;
    public float InitialLifetime;

    public Vector3 Direction = Vector3.up;
    public Vector3 DirectionAtWobbleStart;

    // flags
    public bool Launched = false;
    public bool IsAlive = true;
    public bool YWobbleStarted = false;

    // speed
    public float Speed;
    public float Acceleration;
    public float Drag;


    // Wobbling
    public float FrequencyX;
    public float PhaseX = 0f;
    public float AmplitudeX = 1f;

    public float FrequencyY;
    public float PhaseY = 0f;
    public float AmplitudeY = 1f;

    public float TimeUntilYWobbleStartsSeconds; 
    public float WobbleDelay;



    void Start()
    {
        // puff particles and sound and tween
    }

    void Update()
    {
        if (!Launched)
            return;
        if(!IsAlive)
            return;

        if (Speed > 0)
            Speed += Acceleration * Time.deltaTime;
        
        Speed = Mathf.Max(0f, Speed);

        var elapsed = InitialLifetime - Lifetime;
        if (TimeUntilYWobbleStartsSeconds <= 0 && !YWobbleStarted)
        {
            DirectionAtWobbleStart = Direction;
            YWobbleStarted = true;
        }

        float sineOffsetX = 0f;
        float sineOffsetY = 0f;

        if (YWobbleStarted)
        {
            sineOffsetX = DirectionAtWobbleStart.x + Mathf.Sin(elapsed * FrequencyX + PhaseX) * AmplitudeX;
            sineOffsetY = DirectionAtWobbleStart.y + Mathf.Sin(elapsed * FrequencyY + PhaseY) * AmplitudeY;
        }
        else
        {
            sineOffsetX = Mathf.Sin(elapsed * FrequencyX + PhaseX) * AmplitudeX;
            sineOffsetY = 1f;
        }

        Direction = new Vector3(sineOffsetX, sineOffsetY, 0f).normalized;

        transform.position += Direction * Speed / G.main.MetersPerUnit * Time.deltaTime;
        transform.up = Direction;

        Launcher.EvaluateAndShowStats(this);

        if(Lifetime <= 0f)
        {
            AskForExplosion();
            return;
        }

        Lifetime -= Time.deltaTime;
        Acceleration -= Drag * Time.deltaTime;
        TimeUntilYWobbleStartsSeconds -= Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Firework")
            return;
        AskForExplosion();
    }
    public void AskForExplosion()
    {
        IsAlive = false;
        FireworkBox.EvaluateExplosion(this);
    }
    public void Explode()
    {
        CreatedTrailParticles.Stop();
        var explosionParticles = Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
        AudioController.Instance.PlaySound3D("Explosion", transform.position);

        Launcher = null;
        FireworkBox = null;

        Destroy(gameObject);
    }
    public void Launch()
    {
        Launched = true;
        CreatedTrailParticles = Instantiate(TrailParticles, FuseAnchor).GetComponent<ParticleSystem>();
        // Play some particles
    }

    public void Init(FireworkBox box, FireworkLauncher launcher)
    {
        FireworkBox = box;
        Launcher = launcher;

        //Lifetime = UnityEngine.Random.Range(launcher.MinLifetimeSeconds,
        // launcher.MinLifetimeSeconds + launcher.UpperLifetimeVariationSeconds);A

        Lifetime = box.FireworkLifetime;
        InitialLifetime = Lifetime;

        Speed = box.FireworkStartSpeed;
        Acceleration = box.Acceleration;
        Drag = box.Drag;

        FrequencyX = box.FrequencyX;
        FrequencyY = box.FrequencyY;

        WobbleDelay = box.WobbleDelay;

        PhaseX = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        PhaseY = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        TimeUntilYWobbleStartsSeconds = WobbleDelay;

        // play some charging animation, like shake and add a continuous fushhh sound
    }
}
