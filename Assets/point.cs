using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
    public float x, y, z, intensity, timestamp, ring, reflectivity, noise, range;

    public point(float xVal, float yVal, float zVal, float inten, float tmst, float ringg, float refl, float ns, float rng)
   {
        x=xVal;
        y=yVal;
        z=zVal;
        intensity = inten;
        timestamp = tmst;
        ring = ringg;
        reflectivity = refl;
        noise = ns;
        range = rng;
   }//end of constructor

    //setters
    public void Set_X(float value){x=value;}
    public void Set_Y(float value){y=value;}
    public void Set_Z(float value){z=value;}
    public void Set_Intensity(float value){intensity=value;}
    public void Set_Timestamp(float value){timestamp=value;}
    public void Set_Ring(float value){ring=value;}
    public void Set_Ref(float value){reflectivity=value;}
    public void Set_Noise(float value){noise=value;}
    public void Set_Rng(float value){range=value;}

    override public string ToString()
    {
        return x + ", " +
            y + ", " +
            z + ", " +
            intensity + ", " +
            timestamp + ", " +
            ring + ", " +
            reflectivity + ", " +
            noise  + ", " + 
            range;
    }

}//end of class

