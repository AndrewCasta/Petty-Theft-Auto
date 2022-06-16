using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField] List<GameObject> bloodDecals;
    ParticleSystem bloodParticleSystem;
    public List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        bloodParticleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        // When particle collides, create decals
        bloodParticleSystem.GetCollisionEvents(other, collisionEvents);
        Instantiate(bloodDecals[Random.Range(0, bloodDecals.Count)], collisionEvents[0].intersection, bloodDecals[0].transform.rotation);
    }
}
