using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NoiseMaterial
{
    public FastNoise noise = new FastNoise();
    public float strength = 1.0f;
    public float pureStrength = 1.0f;
    public bool invert = false;
    public float rotation = 0;
    public Vector2 scale = new Vector2(1, 1);
    public float exageration = 1.0f;

    public Color color = Color.white;

    public float GetValue(float x, float y)
    {
        Vector2 finalPos = new Vector2(x, y);
        finalPos = Quaternion.AngleAxis(rotation, Vector3.forward) * finalPos;
        finalPos.Scale(scale);
        float r = noise.GetNoise(finalPos.x, finalPos.y) * strength;
        if (invert) r = 1 - r;

        r -= 0.5f;
        r *= exageration;
        r += 0.5f;

        r = Mathf.Clamp(r, 0, 1);
        return r * pureStrength;
    }
}

public class NoiseBox : ScriptableObject {

    public List<NoiseMaterial> noises = new List<NoiseMaterial>();
    public bool colored = true;

    public float GetValue(float x, float y)
    {
        float result = 0;
        float total = 0;
        foreach(NoiseMaterial n in noises)
        {
            result += n.GetValue(x, y);
            total += n.strength;
        }
        return result / total;
    }

    public Color GetColor(float x, float y)
    {
        Color color = new Color();
        float total = 0;
        foreach(NoiseMaterial n in noises)
        {
            color += n.GetValue(x, y) * n.color;
            total += n.strength;
        }

        return color/total;
    }
}
