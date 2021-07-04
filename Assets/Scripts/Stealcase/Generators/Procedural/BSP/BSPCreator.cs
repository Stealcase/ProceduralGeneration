using System.Collections;
using System.Collections.Generic;
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
public enum Direction
{
    Left,
    Right
}
    public class BSPCreator : MonoBehaviour
    {

    [SerializeField]public BSPGenerator generator;
    [Range(8, 128)] public int roomSizeX = 8, roomSizeY = 8;
    [Range(4,32)]public int minRoomSize;
    [SerializeField][Range(6, 48)] private int maxRoomSize;
    public float MaxRoomSize { get => maxRoomSize; set => maxRoomSize = (int)value; }

    [SerializeField][Range(0, 8)] private int minRoomDistance;
    public float MinRoomDistance { get => minRoomDistance; set => minRoomDistance = (int)value; }
    [SerializeField][Range(1, 10)] private int maxIterations;
    public float MaxIterations { get => maxIterations; set => maxIterations = (int)value; }
    [SerializeField][Range(1, 8)] private int corridorWidth;
    public float CorridorWidth { get => corridorWidth; set => corridorWidth = (int)value; }
    [SerializeField] private string seed;
    public int[,] Map { get => generator.Map; }
    public string Seed { get => seed; set => seed = value; }
    public MapRenderer mapRenderer; 




    // Start is called before the first frame update
    void Start()
    {
        Generate();

    }
    public void Generate()
    {

            generator = new BSPGenerator(roomSizeX, roomSizeY, (int)CorridorWidth, Seed);
            generator.GenerateRooms((int)MaxIterations, minRoomSize, (int)MaxRoomSize, (int)MinRoomDistance);
            if(Application.isPlaying)
            {
                generator.GenerateMap();
            }
            else
            {
                generator.GenerateMap();
                // if(mapRenderer != null)
                // {
                //     mapRenderer.RenderMap(Map);
                // }
            }

    }
    public void RenderMap()
    {
        if(mapRenderer != null)
        {
            mapRenderer.RenderMap(Map);
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
