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
        public int RoomMargin;
        public int minSize;
        public int maxSize;
        public int CorridorWidth;

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
        private int DivisionDistance{ get; set; }
        private Vector2Int DivisionPoint{ get; set; }
        /// <summary>
        /// The orientation of the divison.
        /// If Hoorizontal, the rooms are next to eachother.
        /// Vertical means they are above and below
        /// </summary>
        public Orientation SplitDirection { get; set; }
        public Vector2Int BottomLeft { get => Rect.min; }
        public Vector2Int TopRight { get => Rect.max; }
        public RectInt Rect { get; set; }
        public Vector2Int ConnectionPoint { get; set; }
        public Room Room { get; set; }
        public List<Room> Corridor { get; set; }
        public System.Random RandomGen { get; set; }



        public bool TrySplit(System.Random RandomGen)
        {
            if (TreeLayerIndex == MaxIterations)
            { return false; }

            if((Width > maxSize || Height > maxSize) && Width/2 > minSize+RoomMargin * 2 && Height/2 > minSize+RoomMargin * 2)
            {
                RandomSplit(RandomGen);
                return true;
            }
            if (Width/2 >= minSize + RoomMargin * 2 && Height > minSize + RoomMargin * 2) 
            {

                SplitLeftAndRight();
                return true;
            }
            if(Height/2 >= minSize + RoomMargin * 2 && Width > minSize + RoomMargin * 2)
            {
                SplitUpAndDown();
                return true;
            }
            Debug.LogWarning($"Couldn't split, was too small. Width: {Width}, Height: {Height}");
            return false;
        }
        public void RandomSplit(System.Random RandomGen)
        {
            var val = RandomGen.Next(100);

            if(val > 50)
            {
                SplitUpAndDown();
            }
            else
            {
                SplitLeftAndRight();
            }
        }
        public void SplitLeftAndRight()
        {
            var tup = SplitVertically();
            RectInt leftRect = tup.Item1;
            RectInt rightRect = tup.Item2;
            int newIndex = TreeLayerIndex + 1;

            Debug.Log($"X Left. Layer {newIndex} \n {leftRect}");
            Debug.Log($"X Right.  Layer {newIndex} \n {rightRect}");

            LeftChild = new Node(leftRect,newIndex, this, MaxIterations, minSize, maxSize, CorridorWidth, RoomMargin, RandomGen);
            //Create node filling right half
            RightChild = new Node(rightRect, newIndex, this, MaxIterations, minSize, maxSize, CorridorWidth, RoomMargin, RandomGen);
        }
        public void SplitUpAndDown()
        {
            var tup = SplitHorizontally();
            RectInt leftRect = tup.Item1;
            RectInt rightRect = tup.Item2;
            int newIndex = TreeLayerIndex + 1;

            Debug.Log($"Y Bottom. Layer {newIndex} \n {leftRect}");
            Debug.Log($"Y Upper. Layer {newIndex} \n {rightRect}");
            //Create node filling bottom
            LeftChild = new Node(leftRect, newIndex, this, MaxIterations, minSize, maxSize, CorridorWidth, RoomMargin, RandomGen);
            //Create node filling from middle to top.
            RightChild = new Node(rightRect, newIndex, this, MaxIterations, minSize, maxSize, CorridorWidth, RoomMargin, RandomGen);

        }
        public Tuple<RectInt,RectInt> SplitVertically()
        {
            SplitDirection = Orientation.Vertical;
            //Split the Rectangle, but make sure to leave at LEAST minimum size on top or bottom.
            DivisionDistance = VectorHelper.NumberBetweenNumbers(0, Rect.width, minSize, RandomGen);
            Debug.Log($"X DIVISION: {DivisionDistance} iteration {TreeLayerIndex}");
            //Calculate corners of left and right node
            var left_Node_Size = new Vector2Int(DivisionDistance, Rect.height);
            DivisionPoint = new Vector2Int(Rect.xMin + DivisionDistance, Rect.yMin);
            var rightNode_Size = new Vector2Int(Rect.xMax - DivisionPoint.x, Rect.height);
            
            //Create node filling left half
            var leftRect = new RectInt(Rect.min, left_Node_Size);
            var rightRect = new RectInt(DivisionPoint, rightNode_Size);


            Debug.Log($"LeftRect CONTAINS {Rect.Contains(leftRect.min)} && {Rect.yMax == leftRect.yMax} {Rect.max} {leftRect.max}");
            Debug.Log($"RightRect: CONTAINS {Rect.Contains(rightRect.min)} && {Rect.max == rightRect.max} {Rect.max} {rightRect.max}");
            Debug.Log($"TopRect: COMPARE LEFT: {leftRect.xMax} RIGHT: {rightRect.xMin}");
            return new Tuple<RectInt, RectInt>(leftRect, rightRect);

        }
        public Tuple<RectInt, RectInt> SplitHorizontally()
        {
            SplitDirection = Orientation.Horizontal;
            //One is created on the current location, one is created Halfway up from the current position;
            //"NumberBetweenNumbers" is a place where we could possibly apply weight towards bigger or smaller rooms
            DivisionDistance = VectorHelper.NumberBetweenNumbers(0, Rect.height, minSize, RandomGen);
            Debug.Log($"Y DIVISION: {DivisionDistance} iteration {TreeLayerIndex}");

            var bottomNode_Size = new Vector2Int(Rect.width, DivisionDistance);
            DivisionPoint = new Vector2Int(Rect.xMin, Rect.yMin + DivisionDistance);
            var rightNodeSize = new Vector2Int(Rect.width, Rect.yMax - DivisionPoint.y);

            var leftRect = new RectInt(Rect.min, bottomNode_Size);
            var rightRect = new RectInt(DivisionPoint, rightNodeSize);
            Debug.Log($"TopRect: COMPARE BOTTOM: {leftRect.yMax} TOP: {rightRect.yMin}");

            
            Debug.Log($"TopRect: CONTAINS {Rect.Contains(rightRect.min)} && {Rect.max == rightRect.max}  {Rect.max} {rightRect.max}");
            Debug.Log($"BottomRect: CONTAINS {Rect.Contains(leftRect.min)} && {Rect.xMax == leftRect.xMax} {Rect.max} {leftRect.max}");
            return new Tuple<RectInt, RectInt>(leftRect, rightRect);
        }
        public void GenerateRoom(System.Random rand = null)
        {
            Room = new Room(BottomLeft, TopRight, minSize, RoomMargin,rand);
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
        public Node(RectInt rect, int index, Node _parent, int maxIterations, int minRoomSize, int maxRoomSize,int corridorSize, int roomMargin, System.Random rand)
        {
            this.MaxIterations = maxIterations;
            this.children = new List<Node>();
            this.Parent = _parent;
            this.Rect = rect;
            this.RoomMargin = roomMargin;
            this.CorridorWidth = corridorSize;
            this.TreeLayerIndex = index;
            this.minSize = minRoomSize;
            this.maxSize = maxRoomSize;
            this.RandomGen = rand;
            // Debug.Log($"Creating new node on layer {index}. \n left Corner {BottomLeft}. Right Corner {TopRight}");
            if(!TrySplit(rand))
            {
                GenerateRoom(rand);
            }

        }
        public void GenerateCorridor(List<Room> corridors)
        {
            //Does this have a room or corridor? If Yes, Return;
            //Do children have rooms? If yes, Generate Corridor.
            //Do children have corridors? 
            if(Corridor != null)
            {
                return;
            }
            if(Room != null)
            {
                if(Parent != null)
                {
                    Parent.GenerateCorridor(corridors);
                }
                return;
            }
            Room leftRoom;
            Room rightRoom;

            if(LeftChild != null && LeftChild.Room != null && RightChild != null && RightChild.Room != null)
            {
                leftRoom = LeftChild.Room;
                rightRoom = RightChild.Room;
            }
            else
            {
                if (SplitDirection == Orientation.Vertical)
                {
                    leftRoom = LeftChild.DigForRoom(Direction.Right);
                    rightRoom = RightChild.DigForRoom(Direction.Left);
                }
                else
                {
                    leftRoom = LeftChild.DigForRoom(Direction.Left);
                    rightRoom = RightChild.DigForRoom(Direction.Right);
                }
            }
            Corridor = new List<Room>();
            if(leftRoom == null || rightRoom == null)
            {
                return;
            }
            if(SplitDirection == Orientation.Vertical)
            {
                Corridor = ConnectHorizontalRooms(leftRoom, rightRoom);
            }
            if (SplitDirection == Orientation.Horizontal)
            {
                Corridor = ConnectVerticalRooms(leftRoom, rightRoom);
            }
            corridors.AddRange(Corridor);

            if(Parent != null)
            {
                Parent.GenerateCorridor(corridors);
            }
        }

        private List<Room> ConnectVerticalRooms(Room bottomRoom, Room topRoom)
        {
            //Try to get Values that match
            List<Room> corridors = new List<Room>();
            var leftXVal = Mathf.Max(bottomRoom.Rect.xMin, topRoom.Rect.xMin);
            Debug.Log($"Left X value {leftXVal}");
            var rightXVal = Mathf.Min(bottomRoom.Rect.xMax, topRoom.Rect.xMax);
            Debug.Log($"Right x value {rightXVal}");
            rightXVal = Mathf.Max(leftXVal, rightXVal);
            Debug.Log($"Calculated Right X val {rightXVal}");
            var xPoint = VectorHelper.NumberBetweenNumbers(leftXVal, rightXVal, CorridorWidth, RandomGen);
            Debug.Log($"Calculated POINT {xPoint}");
            //If the rooms cant be connected with a straight Line, connect with a z Pattern.
            if (xPoint == -1)
            {
                    //Calculate the left YLocation to start from
                    bool bottom_tangential = false;
                    bool top_tangential = false;
                    bool bottom_right = false;
                    bool top_right = false;
                    int bottomCorridor_X_Origin = 0;
                    int topCorridor_X_Origin = 0;
                    //Pick the closest points to connect, either top corner <=> bottom corner || bottom Corner <=> Top Corner
                    if(Mathf.Abs(bottomRoom.Rect.xMax - topRoom.Rect.xMin) < Mathf.Abs(bottomRoom.Rect.xMin - topRoom.Rect.xMax))
                    {
                        bottomCorridor_X_Origin = bottomRoom.Rect.xMax - CorridorWidth;
                        topCorridor_X_Origin = topRoom.Rect.xMin;
                        bottom_right = true;
                    }
                    else
                    {
                        //left point with Corridor Width offset
                        topCorridor_X_Origin = topRoom.Rect.xMax - CorridorWidth;
                        top_right = true;
                        bottomCorridor_X_Origin = bottomRoom.Rect.xMin;
                    }

                    //If the Right Room or left Room is adjacent to the divison point
                    top_tangential = DivisionPoint.y == topRoom.Rect.yMin;
                    bottom_tangential = DivisionPoint.y == bottomRoom.Rect.yMax;

                    //Calculate if corridor should expand upwards or downwards
                    if (!bottom_tangential)
                    {
                        //Add Corridor from Left Room To Divison Point
                        // int yOffset = left_top ? + CorridorWidth : 0;
                        corridors.Add(new Room(new RectInt(bottomCorridor_X_Origin, bottomRoom.Rect.yMax, CorridorWidth, DivisionPoint.y - bottomRoom.Rect.yMax)));
                    }
                    //Check if Right Child is adjacent to divison Point
                    if (!top_tangential)
                    {
                        //Add Corridor from Right Room To Divison Point
                        int yOffset = top_right ? -CorridorWidth : 0;
                        corridors.Add(new Room(new RectInt(topCorridor_X_Origin, DivisionPoint.y, CorridorWidth, topRoom.Rect.yMin - DivisionPoint.y)));
                    }

                    bool rightLowest = bottomCorridor_X_Origin > topCorridor_X_Origin;

                    int offset = top_right ? CorridorWidth : 0;
                    int xPos = Mathf.Min(bottomCorridor_X_Origin, topCorridor_X_Origin) + offset;
                    int horizontalCorridor = Mathf.Abs(topCorridor_X_Origin - bottomCorridor_X_Origin);
                    corridors.Add(new Room(new RectInt(xPos, DivisionPoint.y, horizontalCorridor, CorridorWidth)));
            }
            else
            {
                corridors.Add(new Room(new RectInt(new Vector2Int(xPoint - CorridorWidth, bottomRoom.Rect.yMax), new Vector2Int(CorridorWidth, topRoom.Rect.yMin - bottomRoom.Rect.yMax))));
            }
            return corridors;
        }
        private List<Room> ConnectHorizontalRooms(Room leftRoom, Room rightRoom)
        {
            List<Room> corridors = new List<Room>();
            Debug.Log("Running LeftRightSplit");
            //Try to get Values that match
            var topYVal = Mathf.Min(leftRoom.Rect.yMax, rightRoom.Rect.yMax);
            Debug.Log($"Top Y value {topYVal}");
            var botYVal = Mathf.Max(leftRoom.Rect.yMin, rightRoom.Rect.yMin);
            Debug.Log($"Bot Y value {botYVal}");
            topYVal = Mathf.Max(topYVal, botYVal);
            Debug.Log($"Calculated Top Y val {topYVal}");
            var yPoint = VectorHelper.NumberBetweenNumbers(botYVal, topYVal, CorridorWidth, RandomGen);
            Debug.Log($"Calculated POINT {yPoint}");
            //If the rooms cant be connected with a straight Line, connect with a z Pattern.
            if (yPoint == -1)
            {
                    //Calculate the left YLocation to start from
                    bool left_tangential = false;
                    bool right_tangential = false;
                    bool left_top = false;
                    bool right_top = false;
                    int leftCorridor_Y_Origin = 0;
                    int rightCorridor_Y_Origin = 0;
                    //Pick the closest points to connect, either top corner <=> bottom corner || bottom Corner <=> Top Corner
                    if(Mathf.Abs(leftRoom.Rect.yMax - rightRoom.Rect.yMin) < Mathf.Abs(leftRoom.Rect.yMin - rightRoom.Rect.yMax))
                    {
                        leftCorridor_Y_Origin = leftRoom.Rect.yMax - CorridorWidth;
                        rightCorridor_Y_Origin = rightRoom.Rect.yMin;
                        left_top = true;
                    }
                    else
                    {
                        //left point with Corridor Width offset
                        rightCorridor_Y_Origin = rightRoom.Rect.yMax - CorridorWidth;
                        right_top = true;
                        leftCorridor_Y_Origin = leftRoom.Rect.yMin;
                    }

                    //If the Right Room or left Room is adjacent to the divison point
                    right_tangential = DivisionPoint.x == rightRoom.Rect.xMin;
                    left_tangential = DivisionPoint.x == leftRoom.Rect.xMax;

                    //Calculate if corridor should expand upwards or downwards
                    if (!left_tangential)
                    {
                        //Add Corridor from Left Room To Divison Point
                        // int yOffset = left_top ? + CorridorWidth : 0;
                        corridors.Add(new Room(new RectInt(leftRoom.Rect.xMax, leftCorridor_Y_Origin, DivisionPoint.x - leftRoom.Rect.xMax + CorridorWidth, CorridorWidth)));
                    }
                    //Check if Right Child is adjacent to divison Point
                    if (!right_tangential)
                    {
                        //Add Corridor from Right Room To Divison Point
                        int yOffset = right_top ? -CorridorWidth : 0;
                        corridors.Add(new Room(new RectInt(DivisionPoint.x, rightCorridor_Y_Origin, rightRoom.Rect.xMin - DivisionPoint.x, CorridorWidth)));
                    }

                    bool rightLowest = leftCorridor_Y_Origin > rightCorridor_Y_Origin;

                    int offset = right_top ? CorridorWidth : 0;
                    int yPos = Mathf.Min(leftCorridor_Y_Origin, rightCorridor_Y_Origin) + offset;
                    int verticalCorridorHeight = Mathf.Abs(rightCorridor_Y_Origin - leftCorridor_Y_Origin);
                    corridors.Add(new Room(new RectInt(DivisionPoint.x, yPos, CorridorWidth, verticalCorridorHeight)));
                    //If LeftTanget && rightTangent && leftY < RightY
            }
            else
            {
                //Connect rooms with a straight Line
                corridors.Add(new Room(new RectInt(leftRoom.Rect.xMax, yPoint - CorridorWidth, rightRoom.Rect.xMin - leftRoom.Rect.xMax, CorridorWidth)));
            }
            return corridors;
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
        public Room DigForRoom(Direction direction)
        {
            if(Room != null)
            {
                return Room;
            }
            if(LeftChild == null || RightChild == null)
            {
                return null;
            }
            switch (direction)
            {
                case Direction.Left:
                    return LeftChild.DigForRoom(direction);

                default:
                    return RightChild.DigForRoom(direction);

            }
        }

    }
}
