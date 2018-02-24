using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VirtualController : ScriptableObject, IController {

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
        BackDash,
        Taunt,
        Teabag,
        Jump,
        Attack
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

    public float sens = 1f;

    public void SetSens(float s)
    {
        sens = s;
    }

    public VirtualController(int joystick)
    {
        if(joystick == 0)
        {
            P = KeyCode.K;
            Taunt = KeyCode.O;
            //BP = KeyCode.Joystick1Button4;
            Dash = KeyCode.L;
            Block = KeyCode.M;
            Teabag = KeyCode.W;
            hor = "Horizontal1";
            ver = "Vertical1";
            dpadhor = "DHorizontal1";
        }
        else if(joystick == 1)
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

    public void Setup(VController c)
    {
        P = c.P;
        Taunt = c.Taunt;
        Dash = c.Dash;
        Block = c.Block;
        DP = c.DP;
        Teabag = c.Teabag;
        hor = c.hor;
        ver = c.ver;
        dpadhor = c.dpadhor;
        Jump = c.Jump;
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

    public bool GetJumpDown()
    {
        return Input.GetKeyDown(Jump);
    }

    public bool GetKeyDown(Keys k)
    {
        switch (k)
        {
            case Keys.P: return (GetPDown() && GetHorizontalS() > -0.3f && GetHorizontalS() < 0.3f && GetVertical() < 0.3f && GetVertical() > -0.3f);
            case Keys.FP: return (GetPDown() && GetHorizontalS() > 0.3f);
            case Keys.BP: return (GetPDown() && GetHorizontalS() < -0.3f);
            case Keys.UP: return (GetPDown() && GetVertical() < -0.3f);
            case Keys.DP: return (GetPDown() && GetVertical() > 0.3f);
            case Keys.Dash: return GetDashDown();
            case Keys.BackDash: return GetBackDashDown();
            case Keys.Taunt: return Input.GetKeyDown(Taunt);
            case Keys.Block: return Input.GetKey(Block);
            case Keys.Teabag: return Input.GetKeyDown(Teabag);
            case Keys.Attack: return GetPDown();
        }
        return false;
    }

    bool SameSens(float a, float b)
    {
        if((a<0 && b<0) || (a>0 && b > 0)){
            return true;
        }
        return false;
    }

    public bool GetDashDown()
    {
        if(SameSens(GetHorizontal(), sens) && Input.GetKeyDown(Dash))
        {
            return true;
        }
        return false;
    }

    public bool GetBackDashDown()
    {
        if(!SameSens(GetHorizontal(), sens) && Input.GetKeyDown(Dash))
        {
            return true;
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
