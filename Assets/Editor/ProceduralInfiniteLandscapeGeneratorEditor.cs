using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralInfiniteLandscapeGenerator))]
public class ProceduralInfiniteLandscapeGeneratorEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
    ProceduralInfiniteLandscapeGenerator menu = (ProceduralInfiniteLandscapeGenerator)target;
    if(GUILayout.Button("Create Infinite Terrain")){
      menu.CreateMesh();
    }
    
  }
}