using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Health))]
public class BossKaal : MonoBehaviour
{
    public enum Phase { Phase1, Phase2, Phase3, Dead }
    public Phase currentPhase = Phase.Phase1;

    public float phase2Threshold = 0.66f; // 66%
    public float phase3Threshold = 0.33f; // 33%

    public GameObject firestormPrefab; // particle VFX prefab
    public Transform[] firestormPoints;
    public float firestormInterval = 8f;

    public Transform spearTargetPoint; // usually player or tank
    public float spearInterval = 6f;
    public int spearDamage = 40;

    public float fearRadius = 8f;
    public float fearDuration = 2f;
    public int fearDamage = 10; // damage during fear pulse

    Health health;
    bool isActive = false;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnHealthChanged += OnHealthChanged;
        health.OnDied += OnDied;
    }

    void Start()
    {
        StartCoroutine(BossLoop());
    }

    IEnumerator BossLoop()
    {
        // wait for activation by GameManager or trigger
        while (!isActive) yield return null;

        while (currentPhase != Phase.Dead)
        {
            // choose ability based on timers and phase
            if (currentPhase == Phase.Phase1)
            {
                yield return StartCoroutine(FirestormRoutine());
            }
            else if (currentPhase == Phase.Phase2)
            {
                yield return StartCoroutine(SpearRoutine());
            }
            else if (currentPhase == Phase.Phase3)
            {
                yield return StartCoroutine(FearRoutine());
            }

            yield return null;
        }
    }

    // Example ability routines (you can randomize and add animations)
    IEnumerator FirestormRoutine()
    {
        // spawn firestorms at several points
        foreach (var p in firestormPoints)
        {
            if (p == null) continue;
            Instantiate(firestormPrefab, p.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(firestormInterval);
    }

    IEnumerator SpearRoutine()
    {
        // a targeted spear thrust to a player/tank
        // simple implementation: find nearest player
        var player = FindClosestPlayer();
        if (player != null)
        {
            // teleport spear or animate; apply damage if in range
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= 10f)
            {
                Health ph = player.GetComponent<Health>();
                if (ph != null) ph.TakeDamage(spearDamage);
            }
        }
        yield return new WaitForSeconds(spearInterval);
    }

    IEnumerator FearRoutine()
    {
        // Fear scream: players lose control (you can disable input) and take periodic damage
        Collider[] cols = Physics.OverlapSphere(transform.position, fearRadius, LayerMask.GetMask("Player"));
        foreach (var c in cols)
        {
            Health h = c.GetComponent<Health>();
            if (h != null)
                h.TakeDamage(fearDamage);
            // optionally apply a "stun" effect script here
        }
        yield return new WaitForSeconds(fearDuration);
    }

    public void ActivateBoss()
    {
        isActive = true;
    }

    void OnHealthChanged(int hp)
    {
        float pct = (float)hp / health.maxHealth;
        if (pct <= phase3Threshold)
            currentPhase = Phase.Phase3;
        else if (pct <= phase2Threshold)
            currentPhase = Phase.Phase2;
    }

    void OnDied()
    {
        currentPhase = Phase.Dead;
        // play death cinematic, reward players, call GameManager for drops
        GameManager.Instance.OnBossDefeated(this);
    }

    Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform best = null;
        float bestDist = float.MaxValue;
        foreach (var p in players)
        {
            float d = Vector3.Distance(transform.position, p.transform.position);
            if (d < bestDist) { bestDist = d; best = p.transform; }
        }
        return best;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, fearRadius);
    }
}
