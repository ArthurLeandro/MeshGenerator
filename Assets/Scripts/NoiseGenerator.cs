using System.Collections;
using UnityEngine;

public class NoiseGenerator {
  int octaves;
  float lacunarity;
  float gain;
  float perlinScale;
  public NoiseGenerator(){}
  public NoiseGenerator(int _octaves, float _lacunarity, float _gain,float _perlinScale){
    this.octaves = _octaves;
    this.lacunarity = _lacunarity;
    this.gain=_gain;
    this.perlinScale = _perlinScale;
  }
  public float GetValueNoise(){
    return Random.value;
  }
  public float GetPerlinNoise(float x,float z){
    return (2*Mathf.PerlinNoise(x,z)-1);
  }
  public float GetFractalNoise(float x , float z){
    float fractalNoise = 0f;
    float frequency = 1;
    float amplitude=1;
    for (int i = 0; i < octaves; i++){
      float xValue = x*frequency*perlinScale;
      float zValue = z*frequency*perlinScale;
      fractalNoise +=amplitude*GetPerlinNoise(xValue,zValue);
      frequency *=lacunarity;
      amplitude*=gain;
    }
    return fractalNoise;
  }
}