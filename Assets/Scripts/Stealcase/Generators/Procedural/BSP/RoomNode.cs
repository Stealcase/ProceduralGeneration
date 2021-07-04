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
        public RoomNode(RectInt rect, int index, Node _parent, int maxIterations, int minRoomSize, int maxRoomSize, int cooridorSize, int roomMargin, System.Random rand) : base(rect, index, _parent, maxIterations, minRoomSize, maxRoomSize, cooridorSize, roomMargin, rand)
        {
        }

    }
}