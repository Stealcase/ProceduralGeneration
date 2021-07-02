﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Stealcase.Generators.Procedural.BSP
{
    public class BSPDebugVisualizer : MonoBehaviour
    {
        public BSPCreator bSPCreator;
        public bool ShowCorridors;
        public bool ShowRooms;
        public bool ShowNodes;
        private void Start()
        {
            bSPCreator = GetComponent<BSPCreator>();
        }
        private void OnDrawGizmos()
        {
            if (bSPCreator.generator == null) { return; }
            if (bSPCreator.generator.AllNodes != null && bSPCreator.generator.AllNodes.Count > 0)
            {
                // Debug.Log("Drawing Meshes");
                if(ShowNodes)
                {
                    for (int i = 0; i < bSPCreator.generator.AllNodes.Count; i++)
                    {
                        // Debug.Log($"Drawing Mesh {i}");
                        var mesh = new Mesh();
                        mesh.vertices = new Vector3[]{
                            new Vector3(bSPCreator.generator.AllNodes[i].BottomLeft.x, bSPCreator.generator.AllNodes[i].BottomLeft.y,0),
                            new Vector3(bSPCreator.generator.AllNodes[i].BottomLeft.x, bSPCreator.generator.AllNodes[i].TopRight.y,0),
                            new Vector3(bSPCreator.generator.AllNodes[i].TopRight.x, bSPCreator.generator.AllNodes[i].BottomLeft.y,0),
                            new Vector3(bSPCreator.generator.AllNodes[i].TopRight.x, bSPCreator.generator.AllNodes[i].TopRight.y,0)
                        };
                        mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                        mesh.RecalculateNormals();
                        var colorMod = bSPCreator.generator.AllNodes[i].TreeLayerIndex * 0.2f;
                        Gizmos.color = new Color(colorMod, 0.1f + colorMod, 1f, 1f);
                        Gizmos.DrawWireMesh(mesh, transform.position);
                        var handlePos = new Vector3((mesh.vertices[3].x - mesh.vertices[0].x)/2 + mesh.vertices[0].x, (mesh.vertices[3].y - mesh.vertices[0].y)/2 + mesh.vertices[0].y,0);
                        Handles.Label(transform.position + handlePos, bSPCreator.generator.AllNodes[i].TreeLayerIndex.ToString());
                    }
                }
                if(ShowRooms)
                {
                    for (int i = 0; i < bSPCreator.generator.Rooms.Count; i++)
                    {

                        var roomMesh = new Mesh();
                        roomMesh.vertices = new Vector3[]{
                            new Vector3(bSPCreator.generator.Rooms[i].BottomLeft.x,bSPCreator.generator.Rooms[i].BottomLeft.y,0),
                            new Vector3(bSPCreator.generator.Rooms[i].BottomLeft.x, bSPCreator.generator.Rooms[i].TopRight.y,0),
                            new Vector3(bSPCreator.generator.Rooms[i].TopRight.x, bSPCreator.generator.Rooms[i].BottomLeft.y,0),
                            new Vector3(bSPCreator.generator.Rooms[i].TopRight.x, bSPCreator.generator.Rooms[i].TopRight.y,0)
                        };
                        roomMesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                        roomMesh.RecalculateNormals();
                        Gizmos.color = new Color(1f, 1f, 1f, 1f);
                        Gizmos.DrawMesh(roomMesh, transform.position);
                    }
                }
                if(ShowCorridors)
                {
                    for (int i = 0; i < bSPCreator.generator.Corridors.Count; i++)
                    {

                        var roomMesh = new Mesh();
                        roomMesh.vertices = new Vector3[]{
                            new Vector3(bSPCreator.generator.Corridors[i].BottomLeft.x,bSPCreator.generator.Corridors[i].BottomLeft.y,0),
                            new Vector3(bSPCreator.generator.Corridors[i].BottomLeft.x, bSPCreator.generator.Corridors[i].TopRight.y,0),
                            new Vector3(bSPCreator.generator.Corridors[i].TopRight.x, bSPCreator.generator.Corridors[i].BottomLeft.y,0),
                            new Vector3(bSPCreator.generator.Corridors[i].TopRight.x, bSPCreator.generator.Corridors[i].TopRight.y,0)
                        };
                        roomMesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                        roomMesh.RecalculateNormals();
                        Gizmos.color = new Color(0f, 1f, 1f, 1f);
                        Gizmos.DrawMesh(roomMesh, transform.position);
                        Gizmos.color = new Color(1f, 0f, 0f, 1f);
                        Gizmos.DrawSphere(roomMesh.vertices[0] + transform.position, 1f);
                        Gizmos.color = new Color(1f, 0f, 1f, 1f);
                        Gizmos.DrawSphere(roomMesh.vertices[3] + transform.position, 0.5f);
                        Handles.Label(roomMesh.vertices[0] + transform.position, bSPCreator.generator.Corridors[i].BottomLeft.ToString());

                    }
                }
            }
        }
    }

}