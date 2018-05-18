using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    public float time;
    
    // Use this for initialization
    IEnumerator Start() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {

    }
}
