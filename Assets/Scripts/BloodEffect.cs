using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodParticle;
    ParticleSystem bloodParticleObject;

    public void SpawnBloodEffect(GameObject body)
    {
        // Instantiate blood particle
        bloodParticleObject = Instantiate(bloodParticle, body.transform.position, body.transform.rotation);
        bloodParticleObject.Play();
    }


}
