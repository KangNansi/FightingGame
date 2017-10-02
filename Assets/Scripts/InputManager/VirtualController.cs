using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualController {

    KeyCode LP;
    KeyCode MP;
    KeyCode HP;
    KeyCode LK;
    KeyCode MK;
    KeyCode HK;
    string hor;
    string ver;

    public VirtualController(int joystick)
    {
        if(joystick == 1)
        {
            LP = KeyCode.Joystick1Button2;
            MP = KeyCode.Joystick1Button3;
            HP = KeyCode.Joystick1Button4;
            LK = KeyCode.Joystick1Button0;
            MK = KeyCode.Joystick1Button1;
            HK = KeyCode.Joystick1Button5;
            hor = "Horizontal1";
            ver = "Vertical1";
        }
        else
        {
            LP = KeyCode.Joystick2Button2;
            MP = KeyCode.Joystick2Button3;
            HP = KeyCode.Joystick2Button4;
            LK = KeyCode.Joystick2Button0;
            MK = KeyCode.Joystick2Button1;
            HK = KeyCode.Joystick2Button5;
            hor = "Horizontal2";
            ver = "Vertical2";
        }
    }

    public bool GetLPDown()
    {
        return Input.GetKeyDown(LP);
    }

    public bool GetMPDown()
    {
        return Input.GetKeyDown(MP);
    }

    public bool GetHPDown()
    {
        return Input.GetKeyDown(HP);
    }

    public bool GetLKDown()
    {
        return Input.GetKeyDown(LK);
    }

    public bool GetMKDown()
    {
        return Input.GetKeyDown(MK);
    }

    public bool GetHKDown()
    {
        return Input.GetKeyDown(HK);
    }

    public float GetHorizontal()
    {
        return Input.GetAxisRaw(hor);
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
