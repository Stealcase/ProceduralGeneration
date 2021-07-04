using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Stealcase.Generators.Procedural.BSP
{
    public class BSPDebugVisualizer : MonoBehaviour
    {
        public BSPCreator bSPCreator;
        public bool ShowCorridors;
        public bool ShowRooms;
        public bool ShowNodes;
        public List<Mesh> FrameMeshes = new List<Mesh>();
        public List<Mesh> RoomMeshes = new List<Mesh>();
        public List<Mesh> CorridorMeshes = new List<Mesh>();
        public List<MeshFilter> RoomObjects = new List<MeshFilter>();
        public List<MeshFilter> frameObjects = new List<MeshFilter>();
        public List<MeshFilter> corridorObjects = new List<MeshFilter>();
        public MeshFilter prefab;
        public Transform objectParent;
        // public List<Material> flip;
        public Material flip;
        public Material room;
        public Material corridor;
        private void Start()
        {
            bSPCreator = GetComponent<BSPCreator>();
        }
        public void Generate()
        {
            StartCoroutine(GenerateMeshes());
        }
        public void DisableObjects(List<MeshFilter> obj)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].gameObject.SetActive(false);
            }
        }
        public IEnumerator GenerateMeshes()
        {
            DisableObjects(corridorObjects);
            DisableObjects(frameObjects);
            DisableObjects(RoomObjects);
            //Generate 
            if (bSPCreator.generator.AllNodes != null && bSPCreator.generator.AllNodes.Count > 0)
            {
                yield return new WaitForSeconds(0.1f);
                for (int i = 0; i < bSPCreator.generator.AllNodes.Count; i++)
                {
                    // Debug.Log($"Drawing Mesh {i}");
                    if (i >= FrameMeshes.Count)
                    {
                        FrameMeshes.Add(new Mesh());
                        frameObjects.Add(Instantiate(prefab, transform.position, Quaternion.identity, objectParent));
                    }
                    frameObjects[i].gameObject.SetActive(true);
                    // objects[i].transform.position += new Vector3(0, 0, i);
                    FrameMeshes[i].vertices = new Vector3[]{
                    new Vector3(bSPCreator.generator.AllNodes[i].BottomLeft.x, bSPCreator.generator.AllNodes[i].BottomLeft.y,0),
                    new Vector3(bSPCreator.generator.AllNodes[i].BottomLeft.x, bSPCreator.generator.AllNodes[i].TopRight.y,0),
                    new Vector3(bSPCreator.generator.AllNodes[i].TopRight.x, bSPCreator.generator.AllNodes[i].BottomLeft.y,0),
                    new Vector3(bSPCreator.generator.AllNodes[i].TopRight.x, bSPCreator.generator.AllNodes[i].TopRight.y,0)
                };
                    Vector2[] uv = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0,1),
                    new Vector2(1,0),
                    new Vector2(1,1),
                };
                    FrameMeshes[i].triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                    FrameMeshes[i].RecalculateNormals();
                    yield return new WaitForSeconds(0.1f);
                    // var colorMod = bSPCreator.generator.AllNodes[i].TreeLayerIndex * 0.2f;
                    // Gizmos.color = new Color(colorMod, 0.1f + colorMod, 1f, 1f);
                    // Gizmos.DrawWireMesh(meshes[i], transform.position);
                    // var handlePos = new Vector3((meshes[i].vertices[3].x - meshes[i].vertices[0].x)/2 + meshes[i].vertices[0].x, (meshes[i].vertices[3].y - meshes[i].vertices[0].y)/2 + meshes[i].vertices[0].y,0);
                    // Handles.Label(transform.position + handlePos, bSPCreator.generator.AllNodes[i].TreeLayerIndex.ToString());
                    frameObjects[i].mesh = FrameMeshes[i];
                    frameObjects[i].mesh.uv = uv;
                    frameObjects[i].GetComponent<MeshRenderer>().material = flip;
                }
                for (int i = 0; i < bSPCreator.generator.Rooms.Count; i++)
                {
                    if (i >= RoomMeshes.Count)
                    {
                        RoomMeshes.Add(new Mesh());
                        RoomObjects.Add(Instantiate(prefab, transform.position, Quaternion.identity, objectParent));
                    }
                    RoomObjects[i].transform.position += new Vector3(0, 0, -1f);
                    RoomObjects[i].gameObject.SetActive(true);
                    RoomMeshes[i].vertices = new Vector3[]{
                    new Vector3(bSPCreator.generator.Rooms[i].BottomLeft.x,bSPCreator.generator.Rooms[i].BottomLeft.y,0),
                    new Vector3(bSPCreator.generator.Rooms[i].BottomLeft.x, bSPCreator.generator.Rooms[i].TopRight.y,0),
                    new Vector3(bSPCreator.generator.Rooms[i].TopRight.x, bSPCreator.generator.Rooms[i].BottomLeft.y,0),
                    new Vector3(bSPCreator.generator.Rooms[i].TopRight.x, bSPCreator.generator.Rooms[i].TopRight.y,0)
                };
                    Vector2[] uv = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0,1),
                    new Vector2(1,0),
                    new Vector2(1,1),
                };
                    RoomMeshes[i].triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                    RoomMeshes[i].RecalculateNormals();

                    RoomObjects[i].mesh = RoomMeshes[i];
                    RoomObjects[i].mesh.uv = uv;
                    RoomObjects[i].GetComponent<MeshRenderer>().material = room;
                    yield return new WaitForSeconds(0.1f);
                }
                for (int i = 0; i < bSPCreator.generator.Corridors.Count; i++)
                {
                    if (i >= CorridorMeshes.Count)
                    {
                        CorridorMeshes.Add(new Mesh());
                        corridorObjects.Add(Instantiate(prefab, transform.position, Quaternion.identity, objectParent));
                    }
                    corridorObjects[i].gameObject.SetActive(true);
                    corridorObjects[i].transform.position += new Vector3(0, 0, -2f);
                    CorridorMeshes[i].vertices = new Vector3[]{
                    new Vector3(bSPCreator.generator.Corridors[i].BottomLeft.x,bSPCreator.generator.Corridors[i].BottomLeft.y,0),
                    new Vector3(bSPCreator.generator.Corridors[i].BottomLeft.x, bSPCreator.generator.Corridors[i].TopRight.y,0),
                    new Vector3(bSPCreator.generator.Corridors[i].TopRight.x, bSPCreator.generator.Corridors[i].BottomLeft.y,0),
                    new Vector3(bSPCreator.generator.Corridors[i].TopRight.x, bSPCreator.generator.Corridors[i].TopRight.y,0)
                };
                    Vector2[] uv = new Vector2[]{
                    new Vector2(0,0),
                    new Vector2(0,1),
                    new Vector2(1,0),
                    new Vector2(1,1),
                };
                    CorridorMeshes[i].triangles = new int[] { 0, 1, 2, 1, 3, 2 };
                    CorridorMeshes[i].RecalculateNormals();

                    corridorObjects[i].mesh = CorridorMeshes[i];
                    corridorObjects[i].mesh.uv = uv;
                    corridorObjects[i].GetComponent<MeshRenderer>().material = corridor;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
#if UNITY_EDITOR
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
 #endif
    }
}