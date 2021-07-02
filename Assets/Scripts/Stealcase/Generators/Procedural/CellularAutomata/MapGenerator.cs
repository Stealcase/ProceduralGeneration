using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Stealcase.Generators.Tiles;

namespace Stealcase.Generators.Procedural.CellularAutomata
{
    public class MapGenerator : MonoBehaviour
    {
        [Range(1, 128)] public int width;
        [Range(1, 128)] public int height;

        [Tooltip("If empty, will use random seed")]public string seed;

        [Range(0, 7)] public int wallDuplicationThreshold;
        [Range(40, 60)] public int randomFillPercent;

        //this is how you create a 2D Array!!!
        private int[,] GeneratedMap;
        private int[,] VisibleMap;
        CellularAutomataGenerator generator;
        public MapRenderer tilemapRenderer;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            generator = new CellularAutomataGenerator(width,height,wallDuplicationThreshold,randomFillPercent);
            GeneratedMap = generator.GenerateMap(seed);

            if(Application.isPlaying)
            {
                StartCoroutine(DelaySmooth(generator,GeneratedMap));
            }
            else
            {
                VisibleMap = generator.SmoothMap(GeneratedMap);
            }
        }
        public void OverwriteTilemap()
        {
            if(tilemapRenderer != null)
            {
                tilemapRenderer.RenderMap(VisibleMap);
            }
        }
        public void OverlayTilemap()
        {
            if(tilemapRenderer != null)
            {
                tilemapRenderer.UpdateMap(VisibleMap);
            }
        }
        public void InvertMap()
        {
            VisibleMap = MapArrayGenerator.InvertMap(VisibleMap);
        }



        private void RandomFillMap()
        {
            if(Application.isPlaying)
            {
                StartCoroutine(VisualizeStutter(GeneratedMap));
            }
            else
            {
                VisibleMap = GeneratedMap;
            }
        }

        public IEnumerator DelaySmooth(CellularAutomataGenerator generator, int[,] map)
        {
            VisibleMap = new int[generator.Width, generator.Height];
            StartCoroutine(VisualizeStutter(map));
            yield return new WaitForSeconds(3);
            StartCoroutine(VisualizeStutter(generator.SmoothMap(map)));
        }

        public IEnumerator VisualizeStutter(int[,] map)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    VisibleMap[x, y] = map[x, y];
                }
                if (x % 5 == 0)
                {
                    yield return 0;
                }
            }
        }




        private void OnDrawGizmos()
        {
            if (VisibleMap != null)
            {
                int arr_width = VisibleMap.GetLength(0);
                int arr_height = VisibleMap.GetLength(1);
                for (int x = 0; x < arr_width; x++)
                {
                    for (int y = 0; y < arr_height; y++)
                    {
                        //If the map coordinate value is 1, gizmo color is black. if value is not 1, White color.
                        Gizmos.color = (VisibleMap[x, y] == 1) ? Color.black : Color.white;
                        //Why negative height and width divided by 2? OOOOH because we want to calculate the Center of the grid (for some reason)
                        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                        //Draw a cube with 
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }
    }
}