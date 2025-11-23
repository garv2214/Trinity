using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BossKaal bossKaal;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Call this to start the Veernagar rescue mission
    public void StartVeernagarMission()
    {
        // example: notify players, spawn enemies, then activate boss
        Debug.Log("Mission started: Veernagar Rescue");
        // spawn logic goes here

        // After certain conditions, activate boss:
        bossKaal.ActivateBoss();
    }

    public void OnBossDefeated(BossKaal boss)
    {
        Debug.Log("Boss defeated: " + boss.name);
        // reward players, play cutscene, give divine armor, etc.

        // Example: give divine armor to top damager (requires tracking damage, not implemented yet)
    }
}
