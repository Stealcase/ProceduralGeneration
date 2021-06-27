using UnityEngine;

public class Room
{
    /// <summary>
    /// The Center position of the object
    /// </summary>
    Vector2 Position;
    public float Width{ get; set; }
    public float Height { get; set; }
    public Vector2 BottomLeft { get; set; }
    public Vector2 TopRight { get; set; }
    public Room(Vector2Int bottomLeft, Vector2Int topRight, int minSize)
    {
        //This is wrong
        // Position = new Vector2(this.TopRight.x / 2, this.TopRight.y / 2);

        //The highest number between these two
        var leftXMaxCoord = Mathf.Max(topRight.x - minSize, bottomLeft.x);
        var leftYMaxCoord = Mathf.Max(topRight.y - minSize, bottomLeft.y);

        //Lowest number between these two
        var rightXMinCoord = Mathf.Min(bottomLeft.x + minSize, topRight.x);
        var rightYMinCoord = Mathf.Min(bottomLeft.y + minSize, topRight.y);

        BottomLeft = new Vector2(Random.Range(bottomLeft.x, leftXMaxCoord), Random.Range(bottomLeft.y, leftYMaxCoord));
        
        TopRight = new Vector2(Random.Range(rightXMinCoord, topRight.x), Random.Range(rightYMinCoord, topRight.y));
        
        
        float Width = TopRight.x - BottomLeft.x;
        float Height = TopRight.y - BottomLeft.y;
        Position = new Vector2(TopRight.x - (Width / 2), this.TopRight.y - (Height / 2));

    }
}