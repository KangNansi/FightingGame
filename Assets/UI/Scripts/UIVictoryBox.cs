using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVictoryBox : MonoBehaviour {
    public GameObject victoryImage;

	// Use this for initialization
	void Start () {
        RectTransform rectT = victoryImage.GetComponent<RectTransform>();
        if(rectT != null)
        {
            //rectT.localScale = new Vector3(rectT.lossyScale.x, 1, 1);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        victoryImage.SetActive(true);
    }

    public void Desactivate()
    {
        victoryImage.SetActive(false);
    }
}
