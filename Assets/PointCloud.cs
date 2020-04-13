using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PointCloud : MonoBehaviour {

    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    bool bPointsUpdated = false;
    public TextAsset parsedFile;
    public int cullingRadius;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        InitializeIfNeeded();
        
        if(Input.GetKeyDown("space"))
        {
            SetPoints(m_System.maxParticles/2);
            //RandomColorChange();
            //RandomPositionChange();
            Debug.Log("Set points");
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
            col.r = UnityEngine.Random.Range(0.0f, 1.0f);
            col.g = UnityEngine.Random.Range(0.0f, 1.0f);
            col.b = UnityEngine.Random.Range(0.0f, 1.0f);
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
            pos.Set(UnityEngine.Random.Range(0.0f, 100.0f), UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 100.0f));

            m_Particles[i].position = pos;
        }
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    public void SetPoints(int points)
    {
        Vector3[] pos = new Vector3[points];
        Color[] col = new Color[points];
        

        //pull values from the pre-parsed file
        readFile(parsedFile);

        // update the particle system

        /*
        // Get the particle array
        int numParticlesAlive = m_System.GetParticles(m_Particles);
       
        // if particle count less than points being updated
        if(numParticlesAlive < points)
        {
            // emit some more particles
            m_System.Emit(points - numParticlesAlive);
            numParticlesAlive = m_System.GetParticles(m_Particles);
        }
        */
    }

    private void readFile(TextAsset parsedFile)
    {
        //reads the parsed point file and makes particles
        List<ParticleSystem.Particle> newParticles = new List<ParticleSystem.Particle>();
        string filePath = AssetDatabase.GetAssetPath(parsedFile);
        StreamReader inp_stm = new StreamReader(filePath);
        
        //For each line in file:
        while(!inp_stm.EndOfStream)
       {           
           //Read the line in
           string inp_ln = inp_stm.ReadLine( );
           //delimit by comma
           string[] splitLine = inp_ln.Split(',');

           //create a particle
            newParticles.Add(new ParticleSystem.Particle
                        {
                            remainingLifetime = float.MaxValue,
                            position = new Vector3(float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[0])),
                            startSize = 1f,
                            startColor = calculateColor(splitLine)
                        });                        
        }//end of while

        //set the particle system to the new particles
        var p = newParticles.ToArray();
            if (p.Length > 0)
            {
                m_System.SetParticles(p, p.Length);
            }

    }
    
     private Color32 calculateColor(string[] splitPoint)
    {
        float multiplier = 1 / cullingRadius;               
        Color32 pointColor = new Color(sigmoid(float.Parse(splitPoint[0])), sigmoid(float.Parse(splitPoint[1])), sigmoid(float.Parse(splitPoint[2])), 1);

        return pointColor;
    }

      private float sigmoid(float x)
    {
        return Convert.ToSingle(1.0 / (1.0 + Math.E-x));
    }
    
    }//end of class

