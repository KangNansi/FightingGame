#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class AkCallbackInfo : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkCallbackInfo(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(AkCallbackInfo obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~AkCallbackInfo() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkCallbackInfo(swigCPtr);
        }
        swigCPtr = IntPtr.Zero;
      }
      GC.SuppressFinalize(this);
    }
  }

  public IntPtr pCookie { get { return AkSoundEnginePINVOKE.CSharp_AkCallbackInfo_pCookie_get(swigCPtr);
 }
  }

  public ulong gameObjID { get { return AkSoundEnginePINVOKE.CSharp_AkCallbackInfo_gameObjID_get(swigCPtr);
 } 
  }

  public AkCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkCallbackInfo(), true) {

  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.