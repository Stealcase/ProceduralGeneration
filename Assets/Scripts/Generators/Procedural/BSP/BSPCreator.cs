using System.Collections;
using System.Collections.Generic;
using Stealcase.Helpers;
using UnityEngine;
using Stealcase.Generators.Tiles;

namespace Stealcase.Generators.Procedural.BSP
{
    
public enum Orientation
{
    Horizontal,
    Vertical,
    Random
}
    public class BSPCreator : MonoBehaviour
    {

    [SerializeField]public BSPGenerator generator;
    [Range(8, 128)] public int roomSizeX = 8, roomSizeY = 8;
    [Range(4,32)]public int minRoomSize;
    [Range(6,48)]public int maxRoomSize;
    [Range(1,10)]public int maxIterations;
    [Range(1,8)]public int corridorWidth;
    public bool isGenerating = false;
    public int[,] map;
    public MapRenderer mapRenderer; 



    // Start is called before the first frame update
    void Start()
    {
        Generate();

    }
    public void Generate()
    {

        generator = new BSPGenerator(roomSizeX, roomSizeY);
        
        map = generator.GenerateMap(maxIterations, minRoomSize,maxRoomSize);
        if(mapRenderer != null)
        {
            mapRenderer.RenderMap(map);
        }
        isGenerating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGenerating)
        {
            Generate();
        }
        
    }
        // {
        //     for(int x = 0; x < width; x++)
        //     {
                // for (int y = 0; y < height; y++)
                // {
                // //If the map coordinate value is 1, gizmo color is black. if value is not 1, White color.
                // Gizmos.color = (VisibleMap[x, y] == 1) ? Color.black : Color.white;
                // //Why negative height and width divided by 2? OOOOH because we want to calculate the Center of the grid (for some reason)
                // Vector3 pos = new Vector3(-width/2 + x +.5f,0, -height/2 + y + .5f);
                // //Draw a cube with 
                // Gizmos.DrawCube(pos, Vector3.one);
                // }
            // }
        // }
    }
}
