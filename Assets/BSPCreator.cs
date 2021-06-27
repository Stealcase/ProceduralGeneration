﻿using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;


public enum Orientation
{
    Horizontal,
    Vertical,
    Random
}
public class BSPCreator : MonoBehaviour
{

    BSPGenerator generator;
    public int roomSizeX, roomSizeY;
    public int minRoomSize;
    public int maxRoomSize;
    public int maxIterations;
    public int corridorWidth;
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
    private void OnDrawGizmos()
    {
        if (generator == null) { return; }
        if (generator.AllNodes != null && generator.AllNodes.Count > 0)
        {
            // Debug.Log("Drawing Meshes");
            for (int i = 0; i < generator.AllNodes.Count; i++)
            {
            // Debug.Log($"Drawing Mesh {i}");
                var mesh = new Mesh();
                mesh.vertices = new Vector3[]{
                    new Vector3(generator.AllNodes[i].BottomLeft.x, generator.AllNodes[i].BottomLeft.y,0),
                    new Vector3(generator.AllNodes[i].TopLeft.x, generator.AllNodes[i].TopLeft.y,0),
                    new Vector3(generator.AllNodes[i].BottomRight.x, generator.AllNodes[i].BottomRight.y,0),
                    new Vector3(generator.AllNodes[i].TopRight.x, generator.AllNodes[i].TopRight.y,0)
                };
                mesh.triangles = new int[]{ 0, 1, 2, 1,3,2};
                mesh.RecalculateNormals();
                var colorMod = generator.AllNodes[i].TreeLayerIndex * 0.2f;
                Gizmos.color = new Color(colorMod, 0.1f + colorMod, 1f, 1f);
                Gizmos.DrawWireMesh(mesh,transform.position);
            }
            for (int i = 0; i < generator.Rooms.Count; i++)
            {

                var roomMesh = new Mesh();
                roomMesh.vertices = new Vector3[]{
                    new Vector3(generator.Rooms[i].BottomLeft.x,generator.Rooms[i].BottomLeft.y,0),
                    new Vector3(generator.Rooms[i].BottomLeft.x, generator.Rooms[i].TopRight.y,0),
                    new Vector3(generator.Rooms[i].TopRight.x, generator.Rooms[i].BottomLeft.y,0),
                    new Vector3(generator.Rooms[i].TopRight.x, generator.Rooms[i].TopRight.y,0)
                };
                roomMesh.triangles = new int[]{ 0, 1, 2, 1,3,2};
                roomMesh.RecalculateNormals();
                Gizmos.color = new Color(1f, 1f, 1f, 1f);
                Gizmos.DrawMesh(roomMesh, transform.position);
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
