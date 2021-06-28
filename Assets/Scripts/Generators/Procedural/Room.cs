using Stealcase.Helpers;
using UnityEngine;

public class Room
{
    /// <summary>
    /// The Center position of the object
    /// </summary>
    // Vector2 Position;
    public int Width{ get; set; }
    public int Height { get; set; }
    public Vector2Int BottomLeft { get; set; }
    public Vector2Int TopRight { get; set; }
    public RectInt Rect { get; set; }
    public Vector2 Center { get => new Vector2(TopRight.x - (Width / 2), TopRight.y - (Height / 2)); }
    public Room(Vector2Int bottomLeft, Vector2Int topRight, int minSize)
    {
        Rect = VectorHelper.RectWithinBounds(bottomLeft, topRight, minSize);
        // Rect.x = tuple.Item1.x;


        Debug.Log($"Room BottomLeft {BottomLeft}");
        Debug.Log($"Room TopRight {TopRight}");
        // Position = new Vector2(TopRight.x - (Width / 2), this.TopRight.y - (Height / 2));
    }
    public void ToMap(int[,] map)
    {
        var xStart = BottomLeft.x;
        var xEnd = TopRight.x;
        var ystart = BottomLeft.y;
        var yEnd = TopRight.y;
        
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