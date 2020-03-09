using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PCD_Reader : MonoBehaviour
{
    public TextAsset filename;

    // Start is called before the first frame update
    void Start()
    {         
        // Read each line of the file into a string array. 
        string[] lines = System.IO.File.ReadAllLines(AssetDatabase.GetAssetPath(filename));

        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            // Use a tab to indent each line of the file.
            Debug.Log("\t" + line);
        }
    }//end of start

}
