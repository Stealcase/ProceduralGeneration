using UnityEditor;
using UnityEngine;
using Stealcase.Generators.Procedural.BSP;
namespace Stealcase.Generators
{
    
    [CustomEditor(typeof(BSPCreator))]
    public class BSPCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BSPCreator bspCreator = (BSPCreator)target;
            if(GUILayout.Button("Generate Map"))
            {
                bspCreator.Generate();
            }
            if(GUILayout.Button("Render Map"))
            {
                bspCreator.RenderMap();
            }
        }
    }


}