using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Stealcase.Generators.Procedural.CellularAutomata;

namespace Stealcase.Generators
{
    
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MapGenerator mapGenerator = (MapGenerator)target;
            if(GUILayout.Button("Generate Map"))
            {
                mapGenerator.GenerateMap();
            }
            if(GUILayout.Button("Overwrite Tilemap"))
            {
                mapGenerator.OverwriteTilemap();
            }
            if(GUILayout.Button("Overlay Tilemap"))
            {
                mapGenerator.OverlayTilemap();
            }
            if(GUILayout.Button("Invert Map"))
            {
                mapGenerator.InvertMap();
            }
        }
    }
    
}
