using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNG_Player : MonoBehaviour
{
     public float duration = 0.5f;
     public Texture[] textures;
     
     // Use this for initialization
     void Start () {
         StartCoroutine(DoTextureLoop());
     }
     
     public IEnumerator DoTextureLoop(){
         int i = 0;
         while (true){
             GetComponent<Renderer>().material.mainTexture = textures[i];
             i = (i+1)%textures.Length;
             yield return new WaitForSeconds(duration);
         }
     }
 }

