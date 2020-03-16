using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class pointCloud_generator : MonoBehaviour
{
    public TextAsset currentFrame;    
    private int lineNum=0; //used to keep track of the header
    private char[] delimiterChars = {','}; //characters which will be removed when parsing the file
    private double[] pointVals = {0,0,0,0,0,0,0,0,0};
    public List<point> pointCloud;

    void Start()
    {
        string filePath = AssetDatabase.GetAssetPath(currentFrame);
        readTextFile(filePath);
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
                    //cast all values as doubles
                    for(int i=0; i<9; i++){
    //Debug.Log("Attempting to parse: " + splitLine[i]);
                        try{
                            pointVals[i] = Double.Parse(splitLine[i]);
                        }
                        catch
                        {
                            Debug.Log("Could not parse: " + splitLine[i] + " in slot: " + i + " on line: " + lineNum);
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
       drawCloud(pointCloud);

    }//end of readTxt


    private void drawCloud(List<point> pointCloud)
    {
        
        Debug.Log(pointCloud.Count);

    }//end of drawCloud


}//end of class

