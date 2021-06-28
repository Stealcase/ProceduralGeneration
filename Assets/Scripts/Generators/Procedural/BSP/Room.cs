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
        public void ToMap(int[,] map)
        {
            var xStart = Rect.xMin;
            var xEnd = Rect.xMax;
            var ystart = Rect.yMin;
            var yEnd = Rect.yMax;

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = ystart; y < yEnd; y++)
                {
                    try
                    {
                        map[x, y] = 1;
                    }
                    catch
                    {
                        Debug.LogError($"Index out of range: x: {x}, y: {y}");
                    }
                }
            }
        }
    }
}