using UnityEngine;

[System.Serializable]
public class InputObject {

    public enum TYPE {
        AXIS, KEY
    }
    public TYPE type = TYPE.KEY;

    // Axis properties
    public float axisThreshold = 0.3f;
    public string axisName;
    public bool invert = false;
    private bool down = false;

    public enum KeyType {
        KEY, JOYSTICK1, JOYSTICK2
    }
    public KeyType keyType;
    public KeyCode code;

    private float GetAxis()
    {
        return (invert ? -Input.GetAxis(axisName) : Input.GetAxis(axisName));
    }

    public virtual bool Get() {
        switch (type) {
            case TYPE.KEY:
                return Input.GetKey(code);
            case TYPE.AXIS:
                return GetAxis() > axisThreshold;
        }
        return false;
    }
    public virtual bool GetDown() {
        switch (type) {
            case TYPE.KEY:
                return Input.GetKeyDown(code);
            case TYPE.AXIS:
                return GetAxis() > axisThreshold;
        }
        return false;
    }
    public virtual bool GetUp() {
        switch (type) {
            case TYPE.KEY:
                return Input.GetKeyUp(code);
            case TYPE.AXIS:
                return GetAxis() < axisThreshold;
        }
        return false;
    }

}