using UnityEngine;
using System.Collections;

public class LZUIVictoryBox : MonoBehaviour {
    public GameObject victoryImage;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Show() {
        victoryImage.SetActive(true);
    }

    public void Reinit() {
        victoryImage.SetActive(false);
    }
}
