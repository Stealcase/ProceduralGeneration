using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
 [Range(1,3840)]public int width;
 [Range(1,3840)]public int height;

 public string seed;
 public bool useRandomSeed;

 [Range(0, 100)] public int randomFillPercent;

 //this is how you create a 2D Array!!!
 private int[,] map;


 private void Start()
 {
  GenerateMap();
 }

 private void GenerateMap()
 {
  map = new int[width, height];
  RandomFillMap();
 }


 private void RandomFillMap()
 {
  if (useRandomSeed)
  {
   seed = Time.time.ToString();
  }
  //PseudoRandomNumberGeneratior;
  System.Random pseudoRandom = new System.Random(seed.GetHashCode());
 
  //2D Array iteration
  for (int x = 0; x < width; x++)
  {
   for (int y = 0; y < height; y++)
   {
    //Catches the edges of the map
    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
    {
     map[x, y] = 1;
    }
    else
    {
     //.next gives number between Minimum value and maximum value, based on seed.
    
     //If number is less than random fill percent, return 1. If more, return 0.
     map[x, y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1:0 ;
    }
    
   }
  }
 }

 void SmoothMap()
 {
  
 }

 private void OnDrawGizmos()
 {
  if (map != null)
  {
   for(int x = 0; x < width; x++)
   {
    for (int y = 0; y < height; y++)
    {
     //If the map coordinate value is 1, gizmo color is black. if value is not 1, White color.
     Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
     //Why negative height and width divided by 2? OOOOH because we want to calculate the Center of the grid (for some reason)
     Vector3 pos = new Vector3(-width/2 + x +.5f,0, -height/2 + y + .5f);
     //Draw a cube with 
     Gizmos.DrawCube(pos, Vector3.one);
    }
   }
  }
 }
}
