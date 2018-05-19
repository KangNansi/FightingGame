using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName = "Wwise Event")]
public class WWiseEventScriptable : LZFight.MiniScript
{

    public override void OnStart()
    {
        base.OnStart();
        Debug.Log(this.name);
        name = this.name.Remove(name.IndexOf('('), 7);
        AkSoundEngine.PostEvent(name, fighter.gameObject);
    }
}
