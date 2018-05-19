using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableScript/Wwise Event")]
public class WwiseEventScriptableScript : ScriptableScript
{
    public string soundEvent;

    public override void Start()
    {
        base.Start();
        AkSoundEngine.PostEvent(soundEvent, GlobalScriptMachine.Instance.gameObject);
    }
}
