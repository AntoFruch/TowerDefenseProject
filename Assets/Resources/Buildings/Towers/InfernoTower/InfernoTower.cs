using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework;
public class InfernoTower : Tower
{
    [Header("InfernoTower-Specific fields")]
    [SerializeField] private InfernoTowerPrefabs prefabs;

    [SerializeField] private LineRenderer beamPrefab; 
    [SerializeField] private Transform firePoint;
    private Dictionary<GameObject, LineRenderer> activeBeams = new Dictionary<GameObject, LineRenderer>();

    private AudioSource audioSource;
    private bool IsAudioPlaying;

    protected override void Start()
    {
        base.Start();
        targets = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void UpdateTarget()
    {
        targets = TargetStrategies.Multi(rotatingPart, targets, CurrentRange);
    }


    protected override void Update()
    {
        base.Update();
        HandleBeamVisuals();
    }
    
    protected override void Shoot()
    {
        if (targets.Count == 0 && IsAudioPlaying)
        {
            audioSource.Stop();
            IsAudioPlaying = false;
        }
        foreach (GameObject Monster in targets)
        {
            if (!IsAudioPlaying)
            {
                audioSource.Play();
                IsAudioPlaying = true;
            }
            if (Monster.CompareTag("Enemy"))
            {
                Monster.GetComponent<MonsterController>().TakeDamage((int)Math.Floor(this.CurrentDamage));
            }
        }
    }
    private void HandleBeamVisuals()
    {
        // 1. CLEANUP: Remove beams for enemies that died or left range
        // We create a temporary list of enemies to remove to avoid errors while looping
        List<GameObject> enemiesLost = new List<GameObject>();

        foreach (var enemy in activeBeams.Keys)
        {
            // If enemy is dead (null) OR no longer in our target list
            if (enemy == null || !targets.Contains(enemy))
            {
                enemiesLost.Add(enemy);
            }
        }

        // Destroy the LineRenderers for lost enemies
        foreach (var enemy in enemiesLost)
        {
            if (activeBeams[enemy] != null)
            {
                Destroy(activeBeams[enemy].gameObject);
            }
            activeBeams.Remove(enemy);
        }

        // 2. DRAW: Create or Update beams for current targets
        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            // If we don't have a beam for this specific enemy yet, create one
            if (!activeBeams.ContainsKey(target))
            {
                LineRenderer newBeam = Instantiate(beamPrefab, transform);
                activeBeams.Add(target, newBeam);
            }

            // Update the positions of the beam
            LineRenderer lr = activeBeams[target];
            
            // Start of laser (Tower tip)
            lr.SetPosition(0, firePoint.position); 
            
            // End of laser (Enemy center)
            lr.SetPosition(1, target.transform.position); 
        }
    }



}
