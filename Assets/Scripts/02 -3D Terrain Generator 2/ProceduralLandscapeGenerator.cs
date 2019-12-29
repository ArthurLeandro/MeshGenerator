using UnityEngine;

public class ProceduralLandscapeGenerator : GenericMeshGenerator {
    [SerializeField] private int xResolution = 20;
    [SerializeField] private int zResolution = 20;
    [SerializeField] float meshScale = 1;
    [SerializeField] float xScale = 1;
    [SerializeField] float yScale = 1;
    [SerializeField] float meshHeight = 1;
    [SerializeField, Range (1, 8)] int octaves = 1;
    [SerializeField] int lacunarity = 2;
    [SerializeField, Range (0, 1)] float gain = 0.5f;
    [SerializeField] float perlinScale = 1;
    [SerializeField] private float uvScale = 1;
    protected override void SetMeshNums () {
      numVertices = (xResolution + 1) * (zResolution + 1);
      numTriangles = 6 * xResolution * zResolution;
    }
    protected override void SetVertices () {
      float xx, y, zz = 0;
      Vector3[] vs = new Vector3[numVertices];
      NoiseGenerator noiseGenerator = new NoiseGenerator (octaves, lacunarity, gain, perlinScale);
      for (int z = 0; z <= zResolution; z++) {
        for (int x = 0; x <= xResolution; x++) {
          xx = ((float) x / xResolution) * meshScale;
          zz = ((float) x / zResolution) * meshScale;
          y = yScale * noiseGenerator.GetFractalNoise (xx, zz);
          vertices.Add (new Vector3 (xx, y, zz));
        }
      }
    }

    protected override void SetTriangles () {
      int triCount=0;
      for (int z = 0; z < zResolution; z++) {
        for (int x = 0; x < xResolution; x++) {
          triangles.Add (triCount);
          triangles.Add (triCount + xResolution + 1);
          triangles.Add (triCount + 1);

          triangles.Add (triCount);
          triangles.Add (triCount + xResolution+1);
          triangles.Add (triCount + xResolution + 2);
          triCount++;
        }
        triCount++;
      }
    }
      protected override void SetUvs () {
        // Vector2[] uvsArray = new Vector2[numVertices];
        // for (int i = 0; i < resolution; i++) {
        //   if (uvFollowSurface) {
        //     uvsArray[i] = new Vector2 (i * numTexturesPerSquare / uvScale, 1);
        //     uvsArray[i + resolution] = new Vector2 (i * numTexturesPerSquare / uvScale, 0);
        //   } else {
        //     uvsArray[i] = new Vector2 (vertices[i].x / uvScale, vertices[i].y);
        //     uvsArray[i + resolution] = new Vector2 (vertices[i].x, vertices[i + resolution].y);
        //   }

        // }
        // uvs.AddRange (uvsArray);
      }
      protected override void SetNormals () {
        SetGeneralNormals ();
      }
      protected override void SetTangents () {
        SetGeneralTangents ();
      }
      protected override void SetVertexColors () { }
    }