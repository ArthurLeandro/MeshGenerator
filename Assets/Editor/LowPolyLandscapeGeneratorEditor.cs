using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LowPolyLandscapeGenerator))]
public class LowPolyLandscapeGeneratorEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
    LowPolyLandscapeGenerator menu = (LowPolyLandscapeGenerator)target;
    if(GUILayout.Button("Create 3D Terrain")){
      menu.CreateMesh();
    }
  }
}