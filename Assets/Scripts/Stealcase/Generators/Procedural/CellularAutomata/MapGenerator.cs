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

        [SerializeField] [Tooltip("If empty, will use random seed")] private string seed;
        public string Seed { get => seed; set => seed = value; }

        [SerializeField] [Range(0, 7)] private int wallDuplicationThreshold;
        public float WallDuplicationThreshold { get => wallDuplicationThreshold; set => wallDuplicationThreshold = (int)value; }
        [SerializeField] [Range(40, 60)] private int randomFillPercent;
        public float RandomFillPercent { get => randomFillPercent; set => randomFillPercent = (int)value; }

        //this is how you create a 2D Array!!!
        private int[,] GeneratedMap;
        private int[,] VisibleMap;
        private GameObject[,] cubes;
        CellularAutomataGenerator generator;
        public MapRenderer tilemapRenderer;
        /// <summary>
        /// These are only here to prevent Code Stripping from Deploy
        /// </summary>
        private MeshRenderer[,] meshRenderer;
        private MeshFilter meshFilter;
        private BoxCollider boxCollider;
        public Material OnMaterial;
        public Material OffMaterial;
        private bool enumeratorRunning = false;
        private Coroutine upatingCubes;
        public Transform cubeParent;


        private void Start()
        {
            InitializeCubes(width, height);
            GenerateMap();
        }

        public void GenerateMap()
        {
            generator = new CellularAutomataGenerator(width,height,(int)WallDuplicationThreshold,(int)RandomFillPercent);
            GeneratedMap = generator.GenerateMap(Seed);

            if(Application.isPlaying)
            {
                StartCoroutine(DelaySmoothBuildMode(generator,GeneratedMap));
                
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
            UpdateCubesShortcut();

        }
        public void UpdateCubesShortcut()
        {
            if(Application.isPlaying)
            {
                if(enumeratorRunning)
                {
                    StopCoroutine(upatingCubes);
                    enumeratorRunning = false;
                }
                upatingCubes = StartCoroutine(UpdateCubesDelayed());
            }
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
            UpdateCubesShortcut();
            yield return new WaitForSeconds(3);
            StartCoroutine(VisualizeStutter(generator.SmoothMap(map)));
            UpdateCubesShortcut();
        }
        public IEnumerator DelaySmoothBuildMode(CellularAutomataGenerator generator, int[,] map)
        {
            VisibleMap = map;
            UpdateCubesShortcut();
            yield return new WaitForSeconds(0.6f);
            VisibleMap = generator.SmoothMap(map);
            UpdateCubesShortcut();
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
        public void InitializeCubes(int width, int height)
        {
            if(cubes == null)
            {
                int arr_width = width;
                int arr_height = height;
                cubes = new GameObject[arr_width,arr_height];
                meshRenderer = new MeshRenderer[arr_width,arr_height];
                for (int x = 0; x < arr_width; x++)
                {
                    for (int y = 0; y < arr_height; y++)
                    {

                        //If the map coordinate value is 1, gizmo color is black. if value is not 1, White color.
                        cubes[x, y] =  GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cubes[x, y].transform.SetParent(cubeParent);
                        meshRenderer[x, y] = cubes[x, y].GetComponent<MeshRenderer>();
                        meshRenderer[x, y].material = OffMaterial;
                        cubes[x,y].transform.position = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0) + this.transform.position;
                    }
                }
            }
        }
        public void UpdateCubes()
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
                        meshRenderer[x, y].material = (VisibleMap[x, y] == 1) ? OffMaterial : OnMaterial;
                    }
                }
            }
        }
        public IEnumerator UpdateCubesDelayed()
        {
            enumeratorRunning = true;
            if (VisibleMap != null)
            {
                int arr_width = VisibleMap.GetLength(0);
                int arr_height = VisibleMap.GetLength(1);
                for (int x = 0; x < arr_width; x++)
                {
                    for (int y = 0; y < arr_height; y++)
                    {
                        //If the map coordinate value is 1, gizmo color is black. if value is not 1, White color.
                        meshRenderer[x, y].material = (VisibleMap[x, y] == 1) ? OffMaterial : OnMaterial;
                    }
                    yield return null;
                }
            }
            enumeratorRunning = false;
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
                        Gizmos.DrawCube(pos + this.transform.position, Vector3.one);
                    }
                }
            }
        }
    }
}