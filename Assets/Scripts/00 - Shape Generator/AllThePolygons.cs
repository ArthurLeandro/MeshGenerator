using System.Collections;
using UnityEngine;

public class AllThePolygons : GenericMeshGenerator {
  [SerializeField, Range (3, 50)] private int numSides = 3;
  [SerializeField] private float radius;

  [SerializeField] private float xTiling = 1;
  [SerializeField] private float yTiling = 1;

  [SerializeField] private float xScroll = 1;
  [SerializeField] private float yScroll = 1;

  [SerializeField] private float angle = 0;

  protected override void SetMeshNums () {
    numVertices = numSides;
    numTriangles = 3 * (numSides - 2); //there are (numSides-2) physical triangles in a regular polygon. 3 ints describe each physical triangle, so multiple this by 3
  }

  protected override void SetVertices () {
    //coordinates of a regular polygon
    for (int i = 0; i < numSides; i++) {
      float angle = 2 * Mathf.PI * i / numSides;
      vertices.Add (new Vector3 (radius * Mathf.Cos (angle), radius * Mathf.Sin (angle), 0));
    }
  }

  protected override void SetTriangles () {
    for (int i = 1; i < numSides - 1; i++) {
      //each physical triangle starts at the zeroeth vertex
      triangles.Add (0);
      triangles.Add (i + 1);
      triangles.Add (i);
    }
  }

  protected override void SetUvs () {
    for (int i = 0; i < numVertices; i++) {
      float uvX = xTiling * vertices[i].x + xScroll;
      float uvY = yTiling * vertices[i].y + yScroll;
      Vector2 uv = new Vector2 (uvX, uvY);

      uvs.Add (Quaternion.AngleAxis (angle, Vector3.forward) * uv);
    }
  }

  protected override void SetNormals () {
    Vector3 normal = new Vector3 (0, 0, -1);
    for (int i = 0; i < numVertices; i++) {
      normals.Add (normal);
    }
  }

  protected override void SetTangents () {
    Vector3 tangent3 = new Vector3 (1, 0, 0); //because this is how the UVs are oriented at angle = 0
    //Rotate clockwise as alpha increases
    Vector3 rotatedTangent = Quaternion.AngleAxis (angle, -Vector3.forward) * tangent3;
    Vector4 tangent = rotatedTangent;
    tangent.w = -1; //left hand rule

    for (int i = 0; i < numVertices; i++) {
      tangents.Add (tangent);
    }
  }

  protected override void SetVertexColors () { }
}