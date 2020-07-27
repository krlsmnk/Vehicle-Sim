using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PointCloud : MonoBehaviour {

    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    bool bPointsUpdated = false;
    //public TextAsset parsedFile;
    public int cullingRadius;
    private string filePath = "C:/Users/krlsmnk/Documents/GitHub/Vehicle Sim/Assets/dataframe/fog2_pcd_ASCII/top/";
    public float timeRemaining;
    public bool isCountingDown = false;
    public int iterator =0;
    FileInfo[] globalInfo;
    public float timeBetweenFrames;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        InitializeIfNeeded();
        
        if(Input.GetKeyDown("space"))
        {
            Debug.Log("Space bar pressed: Setting points");
            SetPoints(m_System.maxParticles/2);
            //RandomColorChange();
            //RandomPositionChange();            
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

        getParsedFilesFromDirectory();


    }

    private void readFile(string parsedFilePath)
    {


        //reads the parsed point file and makes particles
        List<ParticleSystem.Particle> newParticles = new List<ParticleSystem.Particle>();
        //string filePath = AssetDatabase.GetAssetPath(parsedFile);
        StreamReader inp_stm = new StreamReader(parsedFilePath);
        
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
                //Find all other particle systems and clear them
                ParticleSystem[] allPartSystems = GameObject.FindObjectsOfType<ParticleSystem>();
                foreach(ParticleSystem currentSystem in allPartSystems) currentSystem.Clear();
                        
                //m_System.Clear(); //remove any existing particles to prevent bloat
                m_System.SetParticles(p, p.Length);
                //m_Particles = p;
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
    
     /// <summary>
    /// Given a file directory, this method gets all the parsed .txt pointCloud files
    /// </summary>
    /// <param name="v"></param>
    private void getParsedFilesFromDirectory()
    {        
         DirectoryInfo dir = new DirectoryInfo(filePath);
         globalInfo = dir.GetFiles("*.txt");
         drawPointClouds();
         
         
        /*
        info.Select(f => f.FullName).ToArray());
        foreach (FileInfo f in info) 
         {
            
            if (!isCountingDown) { 
            //Load the pointCloud from this file, then set a timer until load the next one
             string newFilePath = filePath + f.Name;
    Debug.Log("Reading File: " + newFilePath);
             readFile(newFilePath);
             setWaitTimer(15f); //wait X seconds until loading the next pointCloud
             }
         }//end of forEachFile
         */
    }

    private void drawPointClouds()
    {
        if (iterator < globalInfo.Length) { 
            string newFilePath = filePath + globalInfo[iterator].Name;
        Debug.Log("Reading File: " + newFilePath);
            readFile(newFilePath);
            setWaitTimer(1f); //wait X seconds until loading the next pointCloud
        }
    }



    /// <summary>
    /// Method waits a number of seconds
    /// </summary>
    /// <param name="duration"></param>
    private void setWaitTimer(float duration)
    {        
        if (!isCountingDown) {
             isCountingDown = true;
             timeRemaining = duration;
             Invoke ( "_tick", timeBetweenFrames );
         }
    }

    private void _tick() {
         timeRemaining--;
         if(timeRemaining > 0) {
             Invoke ( "_tick", 1f );
         } else {
             isCountingDown = false;
            iterator++;
            drawPointClouds();
         }
     }

}//end of class

