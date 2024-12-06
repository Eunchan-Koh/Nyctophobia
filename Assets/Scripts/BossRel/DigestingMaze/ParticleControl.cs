using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    ParticleSystem[] particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    public void UpdateSizes()
    {
        particles ??= GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < particles.Length; i++){
            var shape = particles[i].shape;
            if(transform.localScale.y > transform.localScale.x)
                shape.radius = transform.localScale.y/2;
            else
                shape.radius = transform.localScale.x/2;
        }
    }
}
