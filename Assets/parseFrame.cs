using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using static UnityEngine.ParticleSystem;

public class parseFrame : MonoBehaviour
{
    public TextAsset currentFrame;
    
    private int lineNum=0; //used to keep track of the header
    
    private char[] delimiterChars = {','}; //characters which will be removed when parsing the file
    
    private float[] pointVals = {0,0,0,0,0,0,0,0,0};
    
    private List<point> pointCloud;
    
    private ParticleSystem lidarDisplay;

    private ParticleSystem.Particle[] particles;

    private string filePath;

    public int cullingRadius;

    void Start()
    {
        pointCloud = new List<point>();
        
        filePath = AssetDatabase.GetAssetPath(currentFrame);
        parseFile(filePath);
        
    }//end of start


    void parseFile(string file_path)
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

    //Debug.Log("Non Junk Data Detected");
                        //make a point and add it to the cloud
                        try
                        {
    //Debug.Log("Made point");
                            pointCloud.Add(new point(pointVals[0], pointVals[1], pointVals[2], pointVals[3], pointVals[4], pointVals[5], pointVals[6], pointVals[7], pointVals[8]));                            
                        }
                        catch
                        {
                            Debug.Log("Skipped making point: " + lineNum);
                        }                   
                    } 
                }
                
            }

        lineNum++; //increment line count
       }//end of for each line
       inp_stm.Close( );
        
       //draw cloud
       //drawCloud(pointCloud);
       writePointsToFile(pointCloud);


    }//end of readTxt

    private void writePointsToFile(List<point> passedCloud)
    {         
        List<string> lines = new List<string>();
        foreach(point currentPoint in pointCloud)
        {
            //write point to file
            lines.Add(currentPoint.ToString());
        }
        string[] lineArray = lines.ToArray();
        System.IO.File.WriteAllLines(filePath + "Parsed.txt", lineArray);        
    }

  

}//end of class

