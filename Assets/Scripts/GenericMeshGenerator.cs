using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter)), RequireComponent (typeof (MeshRenderer)), RequireComponent (typeof (MeshCollider))]
[ExecuteInEditMode]
public abstract class GenericMeshGenerator : MonoBehaviour {
  #region Properties
  protected int numTriangles, numVertices;
  [SerializeField] protected Material material;

  #region Lists of Items
  protected List<Vector3> vertices;
  protected List<Vector3> normals;
  protected List<Vector4> tangents;
  protected List<Vector2> uvs;
  protected List<Color32> vertexColors;
  protected List<int> triangles;
  #endregion

  #region Mesh Related Subjects
  
    [HideInInspector] public Mesh mesh;
    [HideInInspector] public MeshFilter meshFilter;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public MeshCollider meshCollider;
  #endregion

  #endregion

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void OnEnable () {
    meshFilter = GetComponent<MeshFilter> ();
    meshRenderer = GetComponent<MeshRenderer> ();
    meshCollider = GetComponent<MeshCollider> ();
    meshRenderer.sharedMaterial = material;
    
  }

  protected abstract void SetVertices ();
  protected abstract void SetTriangles ();
  protected abstract void SetNormals ();
  protected abstract void SetTangents ();
  protected abstract void SetUvs ();
  protected abstract void SetVertexColors ();
  protected abstract void SetMeshNums ();
  public void InitMesh () {
    this.vertices = new List<Vector3> ();
    this.triangles = new List<int> ();
    this.normals = new List<Vector3> ();
    this.tangents = new List<Vector4> ();
    this.uvs = new List<Vector2> ();
    this.vertexColors = new List<Color32> ();
    numVertices = 0;
    numTriangles = 0;
  }
  public bool ValidateMesh () {
    string errorStr = "";
    errorStr += vertices.Count == numVertices? "": "Should be " + numVertices + "vertices, but there are " + vertices.Count + ".";
    errorStr += triangles.Count == numTriangles? "": "Should be " + numTriangles + "triangles, but there are " + triangles.Count + ".";
    errorStr += tangents.Count == numVertices || tangents.Count == 0 ? "" : "Should be " + numVertices + "tangents, but there are " + tangents.Count + ".";
    errorStr += normals.Count == numVertices || normals.Count == 0 ? "" : "Should be " + numVertices + "normals, but there are " + normals.Count + ".";
    errorStr += uvs.Count == numVertices || uvs.Count == 0 ? "" : "Should be " + numVertices + "uvs, but there are " + uvs.Count + ".";
    errorStr += vertexColors.Count == numVertices || vertexColors.Count == 0 ? "" : "Should be " + numVertices + "vertexColors, but there are " + vertexColors.Count + ".";
    bool isValid = string.IsNullOrEmpty (errorStr);
    if (!isValid)
      Debug.LogError ("Not drawing mesh. " + errorStr);
    return isValid;
  }
  public void CreateMesh () {
    mesh = new Mesh ();
    this.InitMesh ();
    this.SetMeshNums ();
    SetVertices ();
    SetTriangles ();
    SetNormals ();
    SetTangents ();
    SetUvs ();
    SetVertexColors ();
    if (ValidateMesh ()) {
      mesh.name = gameObject.name;
      mesh.SetVertices (vertices);
      mesh.SetTriangles (triangles, 0);
      if (normals.Count == 0) {
        mesh.RecalculateNormals ();
        normals.AddRange (mesh.normals);
      }
      mesh.SetNormals (normals);
      mesh.SetUVs (0, uvs);
      mesh.SetTangents (tangents);
      mesh.SetColors (vertexColors);

      meshFilter.mesh = mesh;
      try {
        meshCollider.sharedMesh = mesh;
      } catch (System.Exception e) {
        Debug.Log (e);
        throw;
      }
    }
  }

  protected void SetGeneralNormals () {
    int numGeometricTriangles = numTriangles / 3;
    Vector3[] norm = new Vector3[numVertices];
    int index = 0;
    for (int i = 0; i < numGeometricTriangles; i++) {
      int triangleA = triangles[index];
      int triangleB = triangles[index + 1];
      int triangleC = triangles[index + 2];

      Vector3 directionA = vertices[triangleB] - vertices[triangleA];
      Vector3 directionB = vertices[triangleC] - vertices[triangleA];

      Vector3 normal = Vector3.Cross (directionA, directionB);
      norm[triangleA] += normal;
      norm[triangleB] += normal;
      norm[triangleC] += normal;
      index += 3;
    }
    for (int i = 0; i < numVertices; i++) {
      normals.Add (norm[i].normalized);
    }
  }
  protected void SetGeneralTangents () {
    if (uvs.Count == 0 || normals.Count == 0) {
      Debug.LogWarning("Set UVs and Normals before adding tangents.");
      return;
    }
    int numGeometricTriangles = numTriangles / 3;
    Vector3[] tans = new Vector3[numVertices];
    Vector3[] bitans = new Vector3[numVertices];
    int index = 0;
    for (int i = 0; i < numGeometricTriangles; i++) {
      int triangleA = triangles[index];
      int triangleB = triangles[index + 1];
      int triangleC = triangles[index + 2];
      Vector2 uvA = uvs[triangleA];
      Vector2 uvB = uvs[triangleB];
      Vector2 uvC = uvs[triangleC];
      Vector3 directionA = vertices[triangleB] - vertices[triangleA];
      Vector3 directionB = vertices[triangleC] - vertices[triangleA];
      Vector2 uvDifferentA = new Vector2 (uvB.x - uvA.x, uvC.x - uvA.x);
      Vector2 uvDifferentB = new Vector2 (uvB.y - uvA.y, uvC.y - uvA.y);
      float determinant = 1f / (uvDifferentA.x * uvDifferentB.y - uvDifferentA.y * uvDifferentB.x);
      Vector3 sDirection = determinant * (new Vector3 (
        uvDifferentB.y * directionA.x - uvDifferentB.x * directionB.x,
        uvDifferentB.y * directionA.y - uvDifferentB.x * directionB.y,
        uvDifferentB.y * directionA.z - uvDifferentB.x * directionB.z
      ));
      Vector3 tDirection = determinant * (new Vector3 (
        uvDifferentA.x * directionB.x - uvDifferentA.y * directionA.x,
        uvDifferentA.x * directionB.y - uvDifferentA.y * directionA.y,
        uvDifferentA.x * directionB.z - uvDifferentA.y * directionA.z
      ));
      tans[triangleA] += sDirection;
      tans[triangleB] += sDirection;
      tans[triangleC] += sDirection;

      bitans[triangleA] += tDirection;
      bitans[triangleB] += tDirection;
      bitans[triangleC] += tDirection;
      index += 3;
    }
    for (int i = 0; i < numVertices; i++) {
      Vector3 normal = normals[i];
      Vector3 tan = tans[i];
      //Gram-Schimdt Algorithm
      Vector3 tangent3 = (tan - Vector3.Dot (normal, tan) * normal).normalized;
      Vector4 tangent = tangent3;
      tangent.w = Vector3.Dot (Vector3.Cross (normal, tan), bitans[i]) < 0f? - 1f : 1f;
      tangents.Add (tangent);
    }
  }
}