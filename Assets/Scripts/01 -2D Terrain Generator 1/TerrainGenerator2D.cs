using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TerrainGenerator2D : GenericMeshGenerator {
  [SerializeField] private int resolution = 20;
  [SerializeField] float xScale = 1;
  [SerializeField] float yScale = 1;
  [SerializeField] float meshHeight = 1;
  [SerializeField, Range (1, 8)] int octaves = 1;
  [SerializeField] int lacunarity = 2;
  [SerializeField, Range (0, 1)] float gain = 0.5f;
  [SerializeField] int seed = 0;
  [SerializeField] float perlinScale = 1;
  [SerializeField] private float uvScale = 1;
  [SerializeField] int numTexturesPerSquare = 1;
  [SerializeField] bool uvFollowSurface = false;
  [SerializeField] int sortingOrder = 0;

  protected override void SetMeshNums () {
    numVertices = 2 * resolution;
    numTriangles = 6 * (resolution - 1);
  }

  protected override void SetVertices () {
    float x, y = 0;
    Vector3[] vs = new Vector3[numVertices];
    Random.InitState (seed);
    NoiseGenerator noise = new NoiseGenerator (octaves, lacunarity, gain, perlinScale);

    for (int i = 0; i < resolution; i++) {
      x = ((float) i / resolution) * xScale;
      y = yScale * noise.GetFractalNoise (x, 0);
      vs[i] = new Vector3 (x, y, 0);
      vs[i + resolution] = new Vector3 (x, y - meshHeight, 0);
    }
    vertices.AddRange (vs);
  }

  protected override void SetTriangles () {
    for (int i = 0; i < resolution - 1; i++) {
      triangles.Add (i);
      triangles.Add (i + resolution + 1);
      triangles.Add (i + resolution);

      triangles.Add (i);
      triangles.Add (i + 1);
      triangles.Add (i + resolution + 1);
    }
  }

  protected override void SetUvs () {
    this.meshRenderer.sortingOrder = sortingOrder;
    Vector2[] uvsArray = new Vector2[numVertices];
    for (int i = 0; i < resolution; i++) {
      if (uvFollowSurface) {
        uvsArray[i] = new Vector2 (i * numTexturesPerSquare / uvScale, 1);
        uvsArray[i + resolution] = new Vector2 (i * numTexturesPerSquare / uvScale, 0);
      } else {
        uvsArray[i] = new Vector2 (vertices[i].x / uvScale, vertices[i].y / uvScale);
        uvsArray[i + resolution] = new Vector2 (vertices[i].x / uvScale, vertices[i + resolution].y / uvScale);
      }
    }
    uvs.AddRange (uvsArray);
  }

  protected override void SetNormals () {
    SetGeneralNormals ();
  }
  protected override void SetTangents () {
    SetGeneralTangents ();
  }
  protected override void SetVertexColors () { }

}