using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using static UnityEngine.ParticleSystem;

public class pointCloud_generator : MonoBehaviour
{
    public TextAsset currentFrame;
    
    public int cullingRadius;
    
    private int lineNum=0; //used to keep track of the header
    
    private char[] delimiterChars = {','}; //characters which will be removed when parsing the file
    
    private float[] pointVals = {0,0,0,0,0,0,0,0,0};
    
    private List<point> pointCloud;
    
    private ParticleSystem lidarDisplay;

    private ParticleSystem.Particle[] particles;

    void Start()
    {
        pointCloud = new List<point>();
        particles = new ParticleSystem.Particle[0];
        lidarDisplay = gameObject.GetComponent<ParticleSystem>();

        string filePath = AssetDatabase.GetAssetPath(currentFrame);
        readTextFile(filePath);

        RenderLidar();
    }//end of start


    void readTextFile(string file_path)
    {
       StreamReader inp_stm = new StreamReader(file_path);
       while(!inp_stm.EndOfStream)
       {
           //For each line in file:
           string inp_ln = inp_stm.ReadLine( );
           //Debug.Log(inp_ln);

            bool skipPoint = false; //allows to skip data that isn't formatted correctly
           
            //if past header
            if(lineNum > 9)
            {
                skipPoint = false;
                //remove the "(" and ")" characters from the line
                inp_ln = inp_ln.Replace("(", "");
                inp_ln = inp_ln.Replace(")", "");
                //inp_ln.Trim(new Char[] {'(', ')'} );
                
                //delimit by commas to get values
                string[] splitLine = inp_ln.Split(delimiterChars);
                if(splitLine.Length == 9) //skip data entry if not formatted correctly
                {                
                    //cast all values as floats
                    for(int i=0; i<9; i++){
    //Debug.Log("Attempting to parse: " + splitLine[i]);
                        try{
                            pointVals[i] = float.Parse(splitLine[i]);
                        }
                        catch
                        {
    //Debug.Log("Could not parse: " + splitLine[i] + " in slot: " + i + " on line: " + lineNum);
                            skipPoint = true;
                        }
                    }
                
                    //cull user-defined radius (if they have defined one)
                    if(cullingRadius!= 0)
                        {
                            //if |x|, |y|, or |z| are larger than the culling radius, skip that point
                            if( (Math.Abs(pointVals[0]) > cullingRadius) 
                                || (Math.Abs(pointVals[1]) > cullingRadius)
                                || (Math.Abs(pointVals[2]) > cullingRadius)
                            )
                            {
                                skipPoint = true;
                            }
                        }

    //Debug.Log(pointVals);

                    //avoid junk data
                    if(pointVals[0] != 0 && skipPoint == false)
                    {
                        //make a point and add it to the cloud
                        try
                        {
    //Debug.Log("Made point");
                            pointCloud.Add(new point(pointVals[0], pointVals[1], pointVals[2], pointVals[3], pointVals[4], pointVals[5], pointVals[6], pointVals[7], pointVals[8]));
                        }
                        catch
                        {
    //Debug.Log("Skipped making point: " + lineNum);
                        }                   
                    } 
                }
                
            }

        lineNum++; //increment line count
       }//end of for each line
       inp_stm.Close( );
        
       //draw cloud
       //drawCloud(pointCloud);
       drawCloudParticles(pointCloud);

    }//end of readTxt

    private void drawCloudParticles(List<point> passedCloud)
    {
        //create an array of particles with size = #points
        List<ParticleSystem.Particle> newParticles = new List<ParticleSystem.Particle>();

        int i = 0;
        foreach(point currentPoint in passedCloud)
        {
            //create a particle
            newParticles.Add(new ParticleSystem.Particle
                        {
                            remainingLifetime = float.MaxValue,
                            position = new Vector3(currentPoint.x, currentPoint.y, currentPoint.z),
                            startSize = 1f,
                            startColor = Color.white
                        });                        
        }
        particles = newParticles.ToArray();
        
    }

    private void RenderLidar()
        {
            var p = particles;
            if (p.Length > 0)
            {
                lidarDisplay.SetParticles(particles, particles.Length);
            }
        }

    private void drawCloud(List<point> passedCloud)
    {        
        //Debug.Log(pointCloud.Count);

        foreach (point currentPoint in passedCloud){
            //create an instance of the point prefab at the point's coordinates
            GameObject point = Instantiate(Resources.Load("Point", typeof(GameObject)) as GameObject);
            point.transform.position = new Vector3(currentPoint.x, currentPoint.y, currentPoint.z);
            
            //assign color to point
            //point.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f); // Set to opaque black

        }

    }//end of drawCloud


}//end of class

