using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPlayer : MonoBehaviour
{

   [SerializeField] ParticleSystem[] particles;


    public void Play()
    {
        foreach (ParticleSystem particleSystem in particles)
        {
            particleSystem.Play();
        }
    }

    public void Stop()
    {
        foreach (ParticleSystem particleSystem in particles)
        {
            particleSystem.Stop();
        }
    }


}
