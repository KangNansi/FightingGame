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
        Block,
        Dash,
        Taunt,
        Teabag
    }

    KeyCode P;
    KeyCode Taunt;
    KeyCode Dash;
    KeyCode Block;
    KeyCode DP;
    KeyCode Teabag;
    string hor;
    string ver;
    string dpadhor;

    public float sens = 1f;

    public VirtualController(int joystick)
    {
        if(joystick == 1)
        {
            P = KeyCode.Joystick1Button2;
            Taunt = KeyCode.Joystick1Button3;
            //BP = KeyCode.Joystick1Button4;
            Dash = KeyCode.Joystick1Button1;
            Block = KeyCode.Joystick1Button5;
            Teabag = KeyCode.Joystick1Button5;
            hor = "Horizontal1";
            ver = "Vertical1";
            dpadhor = "DHorizontal1";
        }
        else
        {
            P = KeyCode.Joystick2Button2;
            Taunt = KeyCode.Joystick2Button3;
            //BP = KeyCode.Joystick2Button4;
            Dash = KeyCode.Joystick2Button1;
            Block = KeyCode.Joystick2Button5;
            Teabag = KeyCode.Joystick2Button5;
            hor = "Horizontal2";
            ver = "Vertical2";
            dpadhor = "DHorizontal2";
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
        return Input.GetKeyDown(Block);
    }

    public bool GetDPDown()
    {
        return Input.GetKeyDown(DP);
    }

    public bool GetHKDown()
    {
        return Input.GetKeyDown(Teabag);
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
            case Keys.Dash: return Input.GetKeyDown(Dash);
            case Keys.Taunt: return Input.GetKeyDown(Taunt);
            case Keys.Block: return Input.GetKey(Block);
            case Keys.Teabag: return Input.GetAxisRaw(dpadhor)<-0.3f;
        }
        return false;
    }

    public bool GetBlockDown()
    {
        return Input.GetKeyDown(Block);
    }

    public bool GetBlockUp()
    {
        return Input.GetKeyUp(Block);
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
