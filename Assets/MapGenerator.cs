using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapGenerator : MonoBehaviour
{
 [Range(1,128)]public int width;
 [Range(1,128)]public int height;

 public string seed;
 public bool useRandomSeed;

 [Range(0,7)] public int wallDuplicationThreshold;

 [Range(40, 60)] public int randomFillPercent;

 [FormerlySerializedAs("runSmooth")] public bool reGenerate = false;
 //this is how you create a 2D Array!!!
 private int[,] GeneratedMap;
 private int[,] VisibleMap;

 private void Start()
 {
  GeneratedMap = new int[width, height];
  VisibleMap = new int[width, height];
  GenerateMap();
 }

 private void GenerateMap()
 {

  RandomFillMap();
  StartCoroutine(DelaySmooth());
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
     GeneratedMap[x, y] = 1;
    }
    else
    {
     //.next gives number between Minimum value and maximum value, based on seed.
    
     //If number is less than random fill percent, return 1. If more, return 0.
     GeneratedMap[x, y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1:0 ;
    }
   }

   StartCoroutine(VisualizeStutter(GeneratedMap));
  }
  
 }

 public IEnumerator DelaySmooth()
 {
  yield return new WaitForSeconds(2);
  SmoothMap();
 }

 public IEnumerator VisualizeStutter(int[,] map)
 {
  
  System.Random visualizeRandom = new System.Random(seed.GetHashCode());
  for (int x = 0; x < width; x++)
  {
   for (int y = 0; y < height; y++)
   {
    VisibleMap[x, y] = map[x,y];
   }
   if (x % 5 == 0)
   {
    yield return 0;
   }
  }
 }

 public void Update()
 {
  if (reGenerate)
  {
   reGenerate = false;
   GenerateMap();
  }
 }

 void SmoothMap()
 {
  int[,] smoothedMap = GeneratedMap;
  for (int x = 0; x < width; x++)
  {
   for (int y = 0; y < height; y++)
   {
    int neighborWallTiles = GetSurroundingWallCount(x, y);
    if (neighborWallTiles > wallDuplicationThreshold)
    {
     smoothedMap[x, y] = 1;
    }
    else if (neighborWallTiles < wallDuplicationThreshold)
    {
     smoothedMap[x, y] = 0;
    }
   }
  }
  
  StartCoroutine(VisualizeStutter(smoothedMap));
//Smoothed Map is now main map
 }

//Return the number of walls that surround a tile.
 int GetSurroundingWallCount(int gridX, int gridY)
  {
   int wallCount = 0;
   //Looping through a 3-by-3 grid!
   //Get the left neighbor, The current pos, then the Right neighbor
   for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
   {
    //Down neighbor, current pos, Up Neighbor;
    for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
    {
     //Check if tile is INSIDE the  map, so we dont get an OUT OF INDEX ARRAY error
     if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
     {
      //IGnore middle Tile
      if (neighborX != gridX || neighborY != gridY)
      {
       //Map is a number between 0 and 1. If there is a wall, increment wallcount.
       wallCount += GeneratedMap[neighborX, neighborY];
      }
     }
     else
     {
      //If the tile is outside the map, count that as a wall.
      wallCount++;
     }

    }

   }
   return wallCount;
  }

  private void OnDrawGizmos()
 {
  if (VisibleMap != null)
  {
   for(int x = 0; x < width; x++)
   {
    for (int y = 0; y < height; y++)
    {
     //If the map coordinate value is 1, gizmo color is black. if value is not 1, White color.
     Gizmos.color = (VisibleMap[x, y] == 1) ? Color.black : Color.white;
     //Why negative height and width divided by 2? OOOOH because we want to calculate the Center of the grid (for some reason)
     Vector3 pos = new Vector3(-width/2 + x +.5f,0, -height/2 + y + .5f);
     //Draw a cube with 
     Gizmos.DrawCube(pos, Vector3.one);
    }
   }
  }
 }
}
