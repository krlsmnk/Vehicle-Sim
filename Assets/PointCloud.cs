using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloud : MonoBehaviour {

    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    bool bPointsUpdated = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        InitializeIfNeeded();

        Color col = new Color();
        if(Input.GetKeyDown("space"))
        {
            SetRandomPoints(m_System.maxParticles/2);
            //RandomColorChange();
            //RandomPositionChange();
            Debug.Log("Set random points");
        }
	}

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }

    public void RandomColorChange()
    {
        Color col = new Color();
        Debug.Log("Random Color Change");

        int numParticlesAlive = m_System.GetParticles(m_Particles);
        Debug.Log("NumParts: " + numParticlesAlive);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            col.r = Random.Range(0.0f, 1.0f);
            col.g = Random.Range(0.0f, 1.0f);
            col.b = Random.Range(0.0f, 1.0f);
            col.a = 1.0f;

            m_Particles[i].startColor = col;
        }
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    public void RandomPositionChange()
    {
        Vector3 pos = new Vector3();
        Debug.Log("Random Position Change");

        int numParticlesAlive = m_System.GetParticles(m_Particles);
        Debug.Log("NumParts: " + numParticlesAlive);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            pos.Set(Random.Range(0.0f, 100.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 100.0f));

            m_Particles[i].position = pos;
        }
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    public void SetRandomPoints(int points)
    {
        Vector3[] pos = new Vector3[points];
        Color[] col = new Color[points];
        
        // generate random values - replace with pulling values from the pcd file
        for(int i = 0; i < points; i++)
        {
            pos[i].Set(Random.Range(0.0f, 100.0f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 100.0f));
            col[i].r = Random.Range(0.0f, 1.0f);
            col[i].g = Random.Range(0.0f, 1.0f);
            col[i].b = Random.Range(0.0f, 1.0f);
            col[i].a = 1.0f;
        }

        // update the particle system

        // Get the particle array
        int numParticlesAlive = m_System.GetParticles(m_Particles);
       
        // if particle count less than points being updated
        if(numParticlesAlive < points)
        {
            // emit some more particles
            m_System.Emit(points - numParticlesAlive);
            numParticlesAlive = m_System.GetParticles(m_Particles);
        }

        // update particle position and color
        // current implementation leaves any extra particles in the scene, should determine how to remove those
        for (int i = 0; i < points; i++)
        {
            m_Particles[i].position = pos[i];
            m_Particles[i].startColor = col[i];
        }
        // set particles with new values
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }
}
