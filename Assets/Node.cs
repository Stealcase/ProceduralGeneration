using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{

    /// <summary>
    /// A BSP Node.
    /// Is a square or rectangle with coordinates for corners. 
    /// Has children that divide this node into smaller divisions.
    /// </summary>
    public class Node
    {
        public int Width { get => TopRight.x - BottomLeft.x; }
        public int Height { get => TopRight.y - BottomLeft.y; }

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
        public Vector2Int BottomLeft { get; set; }
        public Vector2Int BottomRight { get; set; }
        public Vector2Int TopLeft { get; set; }
        public Vector2Int TopRight { get; set; }
        public Room Room { get; set; }
        public System.Random RandomGen { get; set; }




        public bool TrySplit()
        {
            if (TreeLayerIndex == MaxIterations)
            { return false; }
            if(Width > maxSize || Height > maxSize && Width > minSize && Height > minSize)
            {
                RandomSplit();
                return true;
            }
            if (Width > minSize || Height > minSize) 
            {

                RandomSplit();
                return true;
                
                // //Otherwise, choose the biggest
                // if (Width > Height) {
                //     SplitX();
                //     return true;
                // }
                // if(Width < Height)
                // {
                //     SplitY();
                //     return true;
                // }
            }
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
            var Xdivision = Width / 2;
            //Calculate corners of left and right node
            var right_node_left_corner = new Vector2Int(BottomLeft.x + Xdivision, BottomLeft.y);
            var left_node_right_corner = new Vector2Int(TopRight.x - Xdivision, TopRight.y);
            int newIndex = TreeLayerIndex + 1;

            Debug.Log($"X Left. Layer {newIndex} \n Bottom Left {BottomLeft}. Top Right {left_node_right_corner}. Width {left_node_right_corner.x - BottomLeft.x} Height: {left_node_right_corner.y - BottomLeft.y}");
            Debug.Log($"X Right. Layer {newIndex} \n Bottom Left {right_node_left_corner}. Top Right {TopRight} Width {TopRight.x - right_node_left_corner.x} Height: {TopRight.y - right_node_left_corner.y}");

            //Create node filling left half
            LeftChild = new Node(BottomLeft, left_node_right_corner,newIndex, this, MaxIterations, minSize, maxSize, RandomGen);
            //Create node filling right half
            RightChild = new Node(right_node_left_corner, TopRight, newIndex, this, MaxIterations, minSize, maxSize, RandomGen);
        }
        public void SplitY()
        {
            //One is created on the current location, one is created Halfway up from the current position;
            var Ydivision = Height / 2;
            var right_node_left_corner = new Vector2Int(BottomLeft.x, TopRight.y - Ydivision);
            var left_node_right_corner = new Vector2Int(TopRight.x, TopRight.y - Ydivision);
            int newIndex = TreeLayerIndex + 1;

            Debug.Log($"Y Bottom. Layer {newIndex} \n Bottom Left {BottomLeft}. Top Right {left_node_right_corner} Width {left_node_right_corner.x - BottomLeft.x} Height: {left_node_right_corner.y - BottomLeft.y}");
            Debug.Log($"Y Upper. Layer {newIndex} \n Bottom Left {right_node_left_corner}. Top Right {TopRight} Width {TopRight.x - right_node_left_corner.x} Height: {TopRight.y - right_node_left_corner.y}");
            //Create node filling bottom
            LeftChild = new Node(BottomLeft, left_node_right_corner, TreeLayerIndex +1, this, MaxIterations, minSize, maxSize, RandomGen);
            //Create node filling from middle to top.
            RightChild = new Node(right_node_left_corner, TopRight, TreeLayerIndex + 1, this, MaxIterations, minSize, maxSize, RandomGen);

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
        public Node(Vector2Int bottomLeft, Vector2Int topRight, int index, Node _parent, int maxIterations, int minRoomSize, int maxRoomSize, System.Random rand)
        {
            this.MaxIterations = maxIterations;
            this.children = new List<Node>();
            this.Parent = _parent;
            this.BottomLeft = bottomLeft;
            this.TopRight = topRight;
            this.BottomRight = new Vector2Int(TopRight.x, bottomLeft.y);
            this.TopLeft = new Vector2Int(bottomLeft.x,TopRight.y);
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
        public void Traverse(List<Node> nodes, List<Room> rooms)
        {
            Debug.Log($"Traversing Child on layer {TreeLayerIndex}, with rect coords \n left:  {BottomLeft},  right: {TopRight}. Widht: {Width}, Heigth: {Height}");
            nodes.Add(this);
            if(LeftChild != null)
            {
                LeftChild.Traverse(nodes, rooms);
            }
            if(RightChild != null)
            {
                RightChild.Traverse(nodes, rooms);
            }
            if(Room != null)
            {
                rooms.Add(Room);
            }
        }



        private void RemoveChild(Node node)
        {
            children.Remove(node);
        }

    }
}
