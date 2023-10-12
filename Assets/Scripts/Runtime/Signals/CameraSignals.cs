using Runtime.Enums;
using Runtime.Extensions;
using UnityEditor.Rendering.LookDev;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CameraSignals : MonoSingleton<CameraSignals>
    {
        public UnityAction<CameraStates> onChangeCameraState = delegate { };
        public UnityAction<CameraTargetState> onSetCinemachineTarget = delegate { };
    }
    
}