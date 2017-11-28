using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSwap : MonoBehaviour {
    public Color In0;
    public Color Out0;
    public Color Out0m;

    public Color In1;
    public Color Out1;
    public Color Out1m;

    public Color In2;
    public Color Out2;
    public Color Out2m;

    public Color In3;
    public Color Out3;
    public Color Out3m;

    public Color In4;
    public Color Out4;
    public Color Out4m;

    public Material _mat;
    public Shader shader;

    void OnEnable() {
        if(_mat == null) {
            _mat = new Material(shader);
        }
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        
        _mat.SetColor("_In0", In0);
        _mat.SetColor("_Out0",Out0);

        _mat.SetColor("_In1", In1);
        _mat.SetColor("_Out1",Out1);

        _mat.SetColor("_In2", In2);
        _mat.SetColor("_Out2", Out2);

        _mat.SetColor("_In3", In3);
        _mat.SetColor("_Out3", Out3);

        _mat.SetColor("_In4", In4);
        _mat.SetColor("_Out4", Out4);
        
        Graphics.Blit(src, dst, _mat);
    }

}
