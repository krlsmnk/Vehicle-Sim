using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using static UnityEngine.ParticleSystem;
using System.Linq;

/// <summary>
/// This class processes ASCII or text-based .pcd files into a form that can be easily and quickly read/displayed in the viewer later
/// If the pcd files have already been parsed once before (contain "Parsed.txt" at the end), you do not need this class
/// </summary>
public class parseFrame : MonoBehaviour
{
    public TextAsset currentFrame;
    
    private int lineNum=0; //used to keep track of the header
    
    private char[] delimiterChars = {',', ' '}; //characters which will be removed when parsing the file
    
    private float[] pointVals = {0,0,0,0,0,0,0,0,0};
    
    private List<point> pointCloud;
    
    private ParticleSystem lidarDisplay;

    private ParticleSystem.Particle[] particles;

    private string filePath;

    public int cullingRadius;

    void Start()
    {
        pointCloud = new List<point>();
        filePath = "C:/Users/krlsmnk/Documents/GitHub/Vehicle Sim/Assets/dataframe/fog2_pcd_ASCII/top/";


        //AUTO MODE:
        parseFileDirectory();

        //MANUAL MODE:
            //filePath = AssetDatabase.GetAssetPath(currentFrame);
            //parseFile(filePath);
        
    }//end of start


    /// <summary>
    /// Given a file directory, this method processes all the ASCII pcd files in that directory, to be used by PointCloud.cs
    /// </summary>
    /// <param name="v"></param>
    private void parseFileDirectory()
    {        
         DirectoryInfo dir = new DirectoryInfo(filePath);
         FileInfo[] info = dir.GetFiles("*.pcd");
         info.Select(f => f.FullName).ToArray();
         foreach (FileInfo f in info) 
         { 
             string newFilePath = filePath + f.Name;
    Debug.Log("Attempting to parse: " + newFilePath);
             parseFile(newFilePath);
         }
    }

    void parseFile(string file_path)
    {
       //Skip the file's header info
       bool pastHeader = false;

       StreamReader inp_stm = new StreamReader(file_path);
       while(!inp_stm.EndOfStream)
       {
           //For each line in file:
           string inp_ln = inp_stm.ReadLine( );
           //Debug.Log(inp_ln);

           bool skipPoint = false; //allows to skip data that isn't formatted correctly
                              
           while (!pastHeader)
           {
    //Debug.Log("Not past header. Reading next line.");
                //until you're past the header, read in the next line
                inp_ln = inp_stm.ReadLine( );
    //Debug.Log("Line contents: " + inp_ln);
                //if the 1st character of the line is a number, you're past the header
                if(Char.IsNumber(inp_ln[0])){ pastHeader=true; }
                
           }
            //if past header
                skipPoint = false;
                //remove the "(" and ")" characters from the line
                inp_ln = inp_ln.Replace("(", "");
                inp_ln = inp_ln.Replace(")", "");
                //inp_ln.Trim(new Char[] {'(', ')'} );
                
                //delimit by commas (and spaces) to get values
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
                
            

        lineNum++; //increment line count
       }//end of for each line
       inp_stm.Close( );
        
       //draw cloud
       //drawCloud(pointCloud);
       writePointsToFile(pointCloud, file_path);


    }//end of parseFile

    private void writePointsToFile(List<point> passedCloud, string filePath2)
    {         
        List<string> lines = new List<string>();
        foreach(point currentPoint in pointCloud)
        {
            //write point to file
            lines.Add(currentPoint.ToString());
        }
        string[] lineArray = lines.ToArray();
        System.IO.File.WriteAllLines(filePath2 + "Parsed.txt", lineArray);        
    }

  

}//end of class

