using UnityEngine;
using System.Collections;

public static class GameObjectExtension
{
    public static void AutoDestroy(this GameObject go, float duration)
    {
        go.AddComponent<AutoDestroy>().time = duration;
    }

    public static void InstantiateAndDestroy(GameObject go, Vector3 position, float duration)
    {
        UnityEngine.Object.Instantiate(go, position, Quaternion.identity).AutoDestroy(duration);
    }

    public static void InstantiateParticleAndDestroy(GameObject go, Vector3 position, bool horInvert = false)
    {
        GameObject newGo = UnityEngine.Object.Instantiate(go, position, Quaternion.identity);
        if (horInvert)
        {
            newGo.transform.localScale = new Vector3(-1, 1, 1);
        }
        float time = newGo.GetComponent<ParticleSystem>().main.duration;
        newGo.AutoDestroy(time);
    }
}
