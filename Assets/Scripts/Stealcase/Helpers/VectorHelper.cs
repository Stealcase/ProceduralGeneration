using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealcase.Helpers
{

public static class VectorHelper 
{

    /// <summary>
    /// Returns a rectangle that fits anywhere within the defined square.
    /// Rectangle has a minimum width and height of minimunSize
    /// </summary>
    /// <param name="BottomLeft"></param>
    /// <param name="TopRight"></param>
    /// <param name="minimumSize"></param>
    /// <returns></returns>
    public static Tuple<Vector2Int, Vector2Int> RectWithinRect(Vector2Int BottomLeft, Vector2Int TopRight, int minimumSize, System.Random rand = null) 
    {
        rand = rand ?? new System.Random();
        //The highest number between these two
        //Pick a spot
        //Then Try to find a spot that
        var leftX_MaxCoord = Mathf.Max(TopRight.x - minimumSize, BottomLeft.x);
        int leftXCoord = rand.Next(BottomLeft.x, leftX_MaxCoord);
        //Lowest number between these two
        var rightXMinCoord = Mathf.Min(leftXCoord + minimumSize, TopRight.x);
        int rightXCoord = rand.Next(rightXMinCoord, TopRight.x);



        var leftY_MaxCoord = Mathf.Max(TopRight.y - minimumSize, BottomLeft.y);
        var leftYCoord = rand.Next(BottomLeft.y, leftY_MaxCoord);
        
        var rightYMinCoord = Mathf.Min(leftYCoord + minimumSize, TopRight.y);
        var rightYCoord = rand.Next(rightYMinCoord, TopRight.y);

        var _bottomLeft = new Vector2Int(leftXCoord,leftYCoord);
        var _topRight = new Vector2Int(rightXCoord,rightYCoord);

        return new Tuple<Vector2Int, Vector2Int>(_bottomLeft, _topRight);
    }
    public static RectInt RectWithinBounds(Vector2Int BottomLeft, Vector2Int TopRight, int minimumSize, int margin = 0, System.Random rand = null) 
    {
        rand = rand ?? new System.Random();
        //The highest number between these two
        //Pick a spot
        //Then Try to find a spot that
        var leftX_MaxCoord = Mathf.Max(TopRight.x - (minimumSize + margin), BottomLeft.x + margin);
        int leftXCoord = rand.Next(BottomLeft.x + margin, leftX_MaxCoord);
        //Lowest number between these two

        //Define smallest possible x Coord
        var rightXMinCoord = Mathf.Min(leftXCoord + minimumSize, TopRight.x - margin);
        //Randomly Define Size:
        int rightXCoord = rand.Next(rightXMinCoord, TopRight.x - margin);
        var width = rightXCoord - leftXCoord;

        //Define biggest possible Y Coord
        var leftY_MaxCoord = Mathf.Max(TopRight.y - (minimumSize + margin), BottomLeft.y + margin);
        var leftYCoord = rand.Next(BottomLeft.y + margin, leftY_MaxCoord);
        
        //Randomly Define Y Size:
        var rightYMinCoord = Mathf.Min(leftYCoord + minimumSize, TopRight.y - margin);
        var rightYCoord = rand.Next(rightYMinCoord, TopRight.y - margin);
        var height = rightYCoord - leftYCoord;

        return new RectInt(leftXCoord, leftYCoord, width, height);
    }
    /// <summary>
    /// Returns the length between two numbers
    /// </summary>
    /// <param name="topRight"></param>
    /// <param name="bottomLeft"></param>
    /// <returns></returns>
    public static int CalculateLength(int topRight, int bottomLeft) => topRight - bottomLeft;
    public static int CalculateHeight(int topRightY, int bottomLeftY) => CalculateLength(topRightY,bottomLeftY);
    public static int CalculateWidth(int topRightX, int bottomLeftX) => CalculateLength(topRightX,bottomLeftX);

/// <summary>
/// Returns the number between the minimun and maximum number, and within the margin to the left and right
/// </summary>
/// <param name="min"></param>
/// <param name="max"></param>
/// <param name="margin"></param>
/// <returns></returns>
    public static int NumberBetweenNumbers(int min, int max, int margin, System.Random rand = null)
    {
        if(max - min < margin)
        {
            Debug.LogWarning($"Warning: Margin is too big. Min: {min} Max: {max} Margin: {margin}");
            return -1;
        }
        var minimum = min + margin;
        var maximum = max - margin;
        rand = rand ?? new System.Random();
        maximum = Mathf.Max(minimum, maximum);
        var randomPoint = rand.Next(minimum, maximum);
        return randomPoint;
    }

}
}
