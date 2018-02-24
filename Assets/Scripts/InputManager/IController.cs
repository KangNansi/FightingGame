using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController {

    void SetSens(float sens);

    bool GetPDown();

    /*public bool GetFPDown()
    {
        return Input.GetKeyDown(FP);
    }

    public bool GetBPDown()
    {
        return Input.GetKeyDown(BP);
    }*/

    bool GetUPDown();

    bool GetDPDown();

    bool GetHKDown();

    bool GetJumpDown();

    bool GetKeyDown(VirtualController.Keys k);

    bool GetDashDown();

    bool GetBackDashDown();

    bool GetBlockDown();

    bool GetBlockUp();

    float GetHorizontal();

    float GetHorizontalS();

    float GetVertical();
}
