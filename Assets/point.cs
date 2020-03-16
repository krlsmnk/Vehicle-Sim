using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
    public double x, y, z, intensity, timestamp, ring, reflectivity, noise, range;

    public point(double xVal, double yVal, double zVal, double inten, double tmst, double ringg, double refl, double ns, double rng)
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
    public void Set_X(double value){x=value;}
    public void Set_Y(double value){y=value;}
    public void Set_Z(double value){z=value;}
    public void Set_Intensity(double value){intensity=value;}
    public void Set_Timestamp(double value){timestamp=value;}
    public void Set_Ring(double value){ring=value;}
    public void Set_Ref(double value){reflectivity=value;}
    public void Set_Noise(double value){noise=value;}
    public void Set_Rng(double value){range=value;}

}//end of class

