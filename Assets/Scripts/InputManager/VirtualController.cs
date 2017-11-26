using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualController {

    public enum Keys
    {
        P,
        FP,
        BP,
        UP,
        DP,
        HK,
        Dash,
        Taunt
    }

    KeyCode P;
    KeyCode Taunt;
    KeyCode Dash;
    KeyCode UP;
    KeyCode DP;
    KeyCode HK;
    string hor;
    string ver;

    public float sens = 1f;

    public VirtualController(int joystick)
    {
        if(joystick == 1)
        {
            P = KeyCode.Joystick1Button2;
            Taunt = KeyCode.Joystick1Button3;
            //BP = KeyCode.Joystick1Button4;
            Dash = KeyCode.Joystick1Button0;
            DP = KeyCode.Joystick1Button1;
            HK = KeyCode.Joystick1Button5;
            hor = "Horizontal1";
            ver = "Vertical1";
        }
        else
        {
            P = KeyCode.Joystick2Button2;
            Taunt = KeyCode.Joystick2Button3;
            //BP = KeyCode.Joystick2Button4;
            Dash = KeyCode.Joystick2Button0;
            DP = KeyCode.Joystick2Button1;
            HK = KeyCode.Joystick2Button5;
            hor = "Horizontal2";
            ver = "Vertical2";
        }
    }

    public bool GetPDown()
    {
        return Input.GetKeyDown(P);
    }

    /*public bool GetFPDown()
    {
        return Input.GetKeyDown(FP);
    }

    public bool GetBPDown()
    {
        return Input.GetKeyDown(BP);
    }*/

    public bool GetUPDown()
    {
        return Input.GetKeyDown(UP);
    }

    public bool GetDPDown()
    {
        return Input.GetKeyDown(DP);
    }

    public bool GetHKDown()
    {
        return Input.GetKeyDown(HK);
    }

    public bool GetKeyDown(Keys k)
    {
        switch (k)
        {
            case Keys.P: return (GetPDown()&&GetHorizontalS() > -0.3f && GetHorizontalS() < 0.3f && GetVertical() < 0.3f && GetVertical() > -0.3f);
            case Keys.FP: return (GetPDown() && GetHorizontalS() > 0.3f);
            case Keys.BP: return (GetPDown() && GetHorizontalS() < -0.3f);
            case Keys.UP: return (GetPDown() && GetVertical() < -0.3f);
            case Keys.DP: return (GetPDown() && GetVertical() > 0.3f);
            case Keys.HK: return GetHKDown();
            case Keys.Dash: return Input.GetKeyDown(Dash);
            case Keys.Taunt: return Input.GetKeyDown(Taunt);
        }
        return false;
    }

    public float GetHorizontal()
    {
        return Input.GetAxisRaw(hor);
    }

    public float GetHorizontalS()
    {
        return Input.GetAxisRaw(hor) * sens;
    }

    public float GetVertical()
    {
        return Input.GetAxisRaw(ver);
    }

    public static VirtualController GetController(int joystick)
    {
        return new VirtualController(joystick);
    }
}
