using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Stealcase.Generators.Procedural;

namespace Stealcase.Generators.Tiles
{
    public class MapRenderer : MonoBehaviour
    {
        public TileBase tile;
        public Tilemap tilemap;
        private void Start()
        {
            // var arr = MapArrayGenerator.GenerateArray(64, 64, false);
            // MapArrayGenerator.RenderMap(arr, tilemap, tile);
        }

        public void RenderMap(int[,] map)
        {
            MapArrayGenerator.RenderMap(map, tilemap, tile);
        }
        public void UpdateMap(int[,] map)
        {
            MapArrayGenerator.UpdateMap(map, tilemap);
        }
        public void ResetMap()
        {
            tilemap.ClearAllTiles();
        }
    }
}