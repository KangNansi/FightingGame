using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jauge {
    float max = 100f;
    float value = 0f;

    public Jauge(float initValue = 0f, float maxValue = 100f)
    {
        max = maxValue;
        value = Mathf.Clamp(initValue, 0, maxValue);
    }

	public float GetValue()
    {
        return value;
    }

    public float NormalizedValue
    {
        get
        {
            return value / max;
        }
    }

    public bool isFull()
    {
        return value == max;
    }

    public static Jauge operator +(Jauge j, float v)
    {
        j.value = Mathf.Clamp(j.value + v, 0, j.max);
        return j;
    }

    public void SetValue(float v)
    {
        value = Mathf.Clamp(v, 0, max);
    }

}
