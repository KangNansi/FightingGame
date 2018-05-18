using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SpriteExtension {
    public static Rect GetSpriteRect(this Sprite s) {
        return new Rect(s.rect.x / (float)s.texture.width,
            s.rect.y / (float)s.texture.height,
            s.rect.width / (float)s.texture.width,
            s.rect.height / (float)s.texture.height);
    }
}
