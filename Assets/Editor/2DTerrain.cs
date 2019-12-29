using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator2D))]
public class TerrainGenerator2DEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
    TerrainGenerator2D menu = (TerrainGenerator2D)target;
    if(GUILayout.Button("Create Mesh")){
      menu.CreateMesh();
    }
  }
}