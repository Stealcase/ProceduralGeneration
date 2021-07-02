using UnityEditor;
using UnityEngine;

namespace Stealcase.Generators.Procedural.BSP
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
        }
    }


}