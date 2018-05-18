using UnityEngine;
using System.Collections;

public class LZUIVictoryBoxContainer : MonoBehaviour {
    public LZUIVictoryBox boxPrefab;

    private LZUIVictoryBox[] boxes;
    private int victoryCount = 0;

    public void Setup(int nbVictory) {
        boxes = new LZUIVictoryBox[nbVictory];
        for (int i = transform.childCount-1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < nbVictory; i++) {
            boxes[i] = Instantiate(boxPrefab, transform);
            boxes[i].Reinit();
        }

        victoryCount = 0;
    }

    public void AddVictory() {
        if(victoryCount >= boxes.Length) {
            return;
        }
        boxes[victoryCount].Show();
        victoryCount++;
    }
}
