using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VController {
    [System.Serializable]
    public enum Keys
    {
        P,
        FP,
        BP,
        UP,
        DP,
        Block,
        Dash,
        Taunt,
        Teabag
    }

    public KeyCode P;
    public KeyCode Taunt;
    public KeyCode Dash;
    public KeyCode Block;
    public KeyCode DP;
    public KeyCode Teabag;
    public KeyCode Jump;
    public string hor;
    public string ver;
    public string dpadhor;

    public void Setup(VirtualController controller)
    {
        P = controller.P;
        Taunt = controller.Taunt;
        Dash = controller.Dash;
        Block = controller.Block;
        DP = controller.DP;
        Teabag = controller.Teabag;
        hor = controller.hor;
        ver = controller.ver;
        dpadhor = controller.dpadhor;
        Jump = controller.Jump;
    }

}
