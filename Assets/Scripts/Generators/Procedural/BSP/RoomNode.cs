using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Stealcase.Generators.Procedural.BSP
{
    public class RoomNode : Node
    {
        public RoomNode(Vector2Int bottomLeft, Vector2Int topRight, int index, Node _parent, int maxIterations, int minRoomSize, int maxRoomSize, System.Random rand) : base(bottomLeft, topRight, index, _parent, maxIterations, minRoomSize, maxRoomSize, rand)
        {
        }

    }
}