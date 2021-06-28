using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Stealcase.Helpers;

namespace Stealcase.Generators.Procedural.BSP
{

    /// <summary>
    /// A BSP Node.
    /// Is a square or rectangle with coordinates for corners. 
    /// Has children that divide this node into smaller divisions.
    /// </summary>
    public class Node
    {
        public int Width { get => Rect.width; }
        public int Height { get => Rect.height; }

        public int minSize;
        public int maxSize;

        private List<Node> children = new List<Node>();
        public List<Node> Children { get => children;}

        public Node Parent { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }

        public int TreeLayerIndex { get; set; }
        private int MaxIterations { get; set; }

        public bool Visited { get; set; }
        /// <summary>
        /// The point where the division occured
        /// </summary>
        private int DivisionPoint{ get; set; }
        /// <summary>
        /// The orientation of the divison.
        /// </summary>
        public Orientation Orientation { get; set; }
        public Vector2Int BottomLeft { get => Rect.min; }
        public Vector2Int TopRight { get => Rect.max; }
        public RectInt Rect { get; set; }
        public Room Room { get; set; }
        public Room Corridor { get; set; }
        public System.Random RandomGen { get; set; }




        public bool TrySplit()
        {
            if (TreeLayerIndex == MaxIterations)
            { return false; }

            if((Width > maxSize || Height > maxSize) && Width > minSize && Height > minSize)
            {
                RandomSplit();
                return true;
            }
            if (Width/2 >= minSize && Height > minSize) 
            {

                SplitX();
                return true;
            }
            if(Height/2 >= minSize)
            {
                SplitY();
                return true;
            }
            Debug.LogWarning($"Couldn't split, was too small. Width: {Width}, Height: {Height}");
            return false;
        }
        public void RandomSplit()
        {
            var val = RandomGen.Next(100);

            if(val > 50)
            {
                SplitY();
            }
            else
            {
                SplitX();
            }
        }
        public void SplitX()
        {
            var tup = CreateVerticalSplit();
            RectInt leftRect = tup.Item1;
            RectInt rightRect = tup.Item2;
            int newIndex = TreeLayerIndex + 1;

            Debug.Log($"X Left. Layer {newIndex} \n {leftRect}");
            Debug.Log($"X Right.  Layer {newIndex} \n {rightRect}");

            LeftChild = new Node(leftRect,newIndex, this, MaxIterations, minSize, maxSize, RandomGen);
            //Create node filling right half
            RightChild = new Node(rightRect, newIndex, this, MaxIterations, minSize, maxSize, RandomGen);
        }
        public void SplitY()
        {
            var tup = CreateHorizontalSplit();
            RectInt leftRect = tup.Item1;
            RectInt rightRect = tup.Item2;
            int newIndex = TreeLayerIndex + 1;

            Debug.Log($"Y Bottom. Layer {newIndex} \n {leftRect}");
            Debug.Log($"Y Upper. Layer {newIndex} \n {rightRect}");
            //Create node filling bottom
            LeftChild = new Node(leftRect, newIndex, this, MaxIterations, minSize, maxSize, RandomGen);
            //Create node filling from middle to top.
            RightChild = new Node(rightRect, newIndex, this, MaxIterations, minSize, maxSize, RandomGen);

        }
        public Tuple<RectInt,RectInt> CreateVerticalSplit()
        {
            Orientation = Orientation.Vertical;
            var divisionPoint = VectorHelper.NumberBetweenNumbers(0, Rect.width, minSize);
            Debug.Log($"X DIVISION: {divisionPoint} iteration {TreeLayerIndex}");
            //Calculate corners of left and right node
            var left_Node_Size = new Vector2Int(divisionPoint, Rect.height);
            var rightNode_Origin = new Vector2Int(Rect.xMin + divisionPoint, Rect.yMin);
            var rightNode_Size = new Vector2Int(Rect.xMax - rightNode_Origin.x, Rect.height);
            
            //Create node filling left half
            var leftRect = new RectInt(Rect.min, left_Node_Size);
            var rightRect = new RectInt(rightNode_Origin, rightNode_Size);


            Debug.Log($"LeftRect CONTAINS {Rect.Contains(leftRect.min)} && {Rect.yMax == leftRect.yMax} {Rect.max} {leftRect.max}");
            Debug.Log($"RightRect: CONTAINS {Rect.Contains(rightRect.min)} && {Rect.max == rightRect.max} {Rect.max} {rightRect.max}");
            Debug.Log($"TopRect: COMPARE LEFT: {leftRect.xMax} RIGHT: {rightRect.xMin}");
            return new Tuple<RectInt, RectInt>(leftRect, rightRect);

        }
        public Tuple<RectInt, RectInt> CreateHorizontalSplit()
        {
            Orientation = Orientation.Horizontal;
            //One is created on the current location, one is created Halfway up from the current position;
            //"NumberBetweenNumbers" is a place where we could possibly apply weight towards bigger or smaller rooms
            var divisionPoint = VectorHelper.NumberBetweenNumbers(0, Rect.height, minSize);
            Debug.Log($"Y DIVISION: {divisionPoint} iteration {TreeLayerIndex}");

            var bottomNode_Size = new Vector2Int(Rect.width, divisionPoint);
            var rightNode_Origin = new Vector2Int(Rect.xMin, Rect.yMin + divisionPoint);
            var rightNodeSize = new Vector2Int(Rect.width, Rect.yMax - rightNode_Origin.y);

            var leftRect = new RectInt(Rect.min, bottomNode_Size);
            var rightRect = new RectInt(rightNode_Origin, rightNodeSize);
            Debug.Log($"TopRect: COMPARE BOTTOM: {leftRect.yMax} TOP: {rightRect.yMin}");

            
            Debug.Log($"TopRect: CONTAINS {Rect.Contains(rightRect.min)} && {Rect.max == rightRect.max}  {Rect.max} {rightRect.max}");
            Debug.Log($"BottomRect: CONTAINS {Rect.Contains(leftRect.min)} && {Rect.xMax == leftRect.xMax} {Rect.max} {leftRect.max}");
            return new Tuple<RectInt, RectInt>(leftRect, rightRect);
        }
        public void GenerateRoom()
        {
            Room = new Room(BottomLeft, TopRight, minSize);
            Debug.Log($"Room coordinates are : {Room.BottomLeft}, {Room.TopRight}, with rect coords \n left:  {BottomLeft},  right: {TopRight}. Widht: {Width}, Heigth: {Height}");
        }


        //Constructors
        /// <summary>
        /// Calculates all 4 corners using just bottom left and top right.
        /// </summary>
        /// <param name="bottomLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="index"> The current Node index</param>
        /// <param name="_parent">The parent Node</param>
        public Node(RectInt rect, int index, Node _parent, int maxIterations, int minRoomSize, int maxRoomSize, System.Random rand)
        {
            this.MaxIterations = maxIterations;
            this.children = new List<Node>();
            this.Parent = _parent;
            this.Rect = rect;
            this.TreeLayerIndex = index;
            this.minSize = minRoomSize;
            this.maxSize = maxRoomSize;
            this.RandomGen = rand;
            // Debug.Log($"Creating new node on layer {index}. \n left Corner {BottomLeft}. Right Corner {TopRight}");
            if(!TrySplit())
            {
                GenerateRoom();
            }
        }
        public void Traverse(List<Node> nodes, List<Node> Leaves)
        {
            Debug.Log($"Traversing Child on layer {TreeLayerIndex}, with rect coords \n left:  {BottomLeft},  right: {TopRight}. Widht: {Width}, Heigth: {Height}");
            nodes.Add(this);
            if(LeftChild != null)
            {
                LeftChild.Traverse(nodes, Leaves);
            }
            if(RightChild != null)
            {
                RightChild.Traverse(nodes, Leaves);
            }
            //If this node is a Leaf
            if(RightChild == null && LeftChild == null && Room != null)
            {
                Leaves.Add(this);
            }
        }
        public void ConnectRooms(List<Room> corridors, int[,] map)
        {
            if(Corridor == null)
            {
                if(RightChild != null && LeftChild != null && RightChild.Room != null && LeftChild.Room != null)
                {
                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            var topYVal = Mathf.Min(LeftChild.Room.TopRight.y, RightChild.Room.TopRight.y);
                            var botYVal = Mathf.Max(LeftChild.Room.BottomLeft.y, RightChild.Room.BottomLeft.y);
                            topYVal = Mathf.Max(topYVal, botYVal);
                            var middleY = VectorHelper.NumberBetweenNumbers(botYVal, topYVal, 0);
                            Corridor = new Room(new Vector2Int(LeftChild.Room.TopRight.x, middleY - 1),new Vector2Int(RightChild.Room.BottomLeft.x, middleY +1),2);
                            break;
                        case Orientation.Vertical:
                            var rightmostX = Mathf.Min(LeftChild.Room.TopRight.x, RightChild.Room.TopRight.x);
                            var leftMostX = Mathf.Max(LeftChild.Room.BottomLeft.x, RightChild.Room.BottomLeft.x);
                            rightmostX = Mathf.Max(rightmostX, leftMostX);
                            var middleX = VectorHelper.NumberBetweenNumbers(leftMostX, rightmostX, 0);
                            Corridor = new Room(new Vector2Int(LeftChild.Room.TopRight.x, middleX - 1),new Vector2Int(RightChild.Room.BottomLeft.x, middleX +1),2);
                            break;
                    }
                    corridors.Add(Corridor);
                    Corridor.ToMap(map);
                }
                if(Parent != null)
                {
                    // Parent.ConnectRooms(corridors, map);
                }
            }
        }

    }
}
