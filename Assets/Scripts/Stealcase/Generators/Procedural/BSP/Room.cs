using System;
using Stealcase.Helpers;
using UnityEngine;

namespace Stealcase.Generators.Procedural.BSP
{
    public class Room
    {
        /// <summary>
        /// The Center position of the object
        /// </summary>
        // Vector2 Position;
        public int Width { get => Rect.width; }
        public int Height { get => Rect.height; }
        public int Area { get => Width * Height; }
        public Vector2Int BottomLeft { get => Rect.min; }
        public Vector2Int TopRight { get => Rect.max; }
        public RectInt Rect { get; set; }
        public Vector2 Center { get => new Vector2(BottomLeft.x + (Width / 2), BottomLeft.y + (Height / 2)); }
        public Room(Vector2Int bottomLeft, Vector2Int topRight, int minSize)
        {
            Rect = VectorHelper.RectWithinBounds(bottomLeft, topRight, minSize);
            // Debug.Log($"Room BottomLeft {Rect.min}");
            // Debug.Log($"Room TopRight {Rect.max}");
            Debug.Log($"Created room with dimensions Width: {Width} Height: {Height}");
        }
        public Room(RectInt rect)
        {
            Rect = rect;
            // Debug.Log($"Room BottomLeft {Rect.min}");
            // Debug.Log($"Room TopRight {Rect.max}");
            Debug.Log($"Created room with dimensions Width: {Width} Height: {Height}");
        }
        public void ToMap(int[,] map)
        {
            var xStart = BottomLeft.x;
            var xEnd = TopRight.x;
            var ystart = BottomLeft.y;
            var yEnd = TopRight.y;
            var mapX = map.GetLength(0);
            var mapY = map.GetLength(1);

            for (int x = xStart; x < xEnd; x++)
            {
                if (mapX <= x)
                {
                    Debug.LogWarning($"X Index out of range: x: {x}");
                    continue;
                }
                for (int y = ystart; y < yEnd; y++)
                {
                    if (mapY <= y)
                    {
                        Debug.LogWarning($"Y Index out of range: x: {x}, y: {y}");
                        break;
                    }
                    try
                    {
                        map[x, y] = 1;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.LogError($"Index out of range: x: {x}, y: {y}. Map X: {mapY}; Map Y: {mapX}.");
                        throw new IndexOutOfRangeException($"{e.Message} Index out of range: x: {x}, y: {y}. Map X: {mapY}; Map Y: {mapX}.");
                    }
                }
            }
        }
    }
}