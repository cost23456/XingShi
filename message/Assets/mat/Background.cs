using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
   private Material material;
   private void Awake()
   {
      material = GetComponent<SpriteRenderer>().material;
   }
   private void Update()
   {
      if (Input.GetKey(KeyCode.D))
      {
         
      material.mainTextureOffset = new Vector2(Time.time * scrollSpeed, 0);
      }
      if(Input.GetKey(KeyCode.A))
      {
         material.mainTextureOffset = new Vector2(Time.time * -scrollSpeed, 0);
      }
       
   }
}
