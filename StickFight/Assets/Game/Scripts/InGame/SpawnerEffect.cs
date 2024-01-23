using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEffect : Singleton<SpawnerEffect>
{
    [SerializeField] private ParticleSystem effectDame;
    public void SpawnerEffectDame(Vector2 position) {
        var effect = effectDame.Spawn(transform);
        effect.transform.position = position;
        effect.Play();
    }
}
