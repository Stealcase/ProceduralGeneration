using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealcase.Helpers
{

public static class VectorHelper 
{
    public static System.Random random = new System.Random();
    /// <summary>
    /// Returns a rectangle that fits anywhere within the defined square.
    /// Rectangle has a minimum width and height of minimunSize
    /// </summary>
    /// <param name="BottomLeft"></param>
    /// <param name="TopRight"></param>
    /// <param name="minimumSize"></param>
    /// <returns></returns>
    public static Tuple<Vector2Int, Vector2Int> RectWithinRect(Vector2Int BottomLeft, Vector2Int TopRight, int minimumSize) 
    {

        //The highest number between these two
        //Pick a spot
        //Then Try to find a spot that
        var leftX_MaxCoord = Mathf.Max(TopRight.x - minimumSize, BottomLeft.x);
        int leftXCoord = random.Next(BottomLeft.x, leftX_MaxCoord);
        //Lowest number between these two
        var rightXMinCoord = Mathf.Min(leftXCoord + minimumSize, TopRight.x);
        int rightXCoord = random.Next(rightXMinCoord, TopRight.x);



        var leftY_MaxCoord = Mathf.Max(TopRight.y - minimumSize, BottomLeft.y);
        var leftYCoord = random.Next(BottomLeft.y, leftY_MaxCoord);
        
        var rightYMinCoord = Mathf.Min(leftYCoord + minimumSize, TopRight.y);
        var rightYCoord = random.Next(rightYMinCoord, TopRight.y);

        var _bottomLeft = new Vector2Int(leftXCoord,leftYCoord);
        var _topRight = new Vector2Int(rightXCoord,rightYCoord);

        return new Tuple<Vector2Int, Vector2Int>(_bottomLeft, _topRight);

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
    public static int NumberBetweenNumbers(int min, int max, int margin)
    {
        if(max - min < margin)
        {
                Debug.LogError($"Warning: Margin is too big. Min: {min} Max: {max} Margin: {margin}");
            }
        var minimum = min + margin;
        var maximum = max - margin;
        maximum = Mathf.Max(minimum, maximum);
        var randomPoint = random.Next(minimum, maximum);
        return randomPoint;
    }

}
}
