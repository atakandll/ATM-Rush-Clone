using System;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
       public UnityAction<byte> onLevelInitialize = delegate(byte arg0) {  };
       public UnityAction onClearActiveLevel = delegate() {  };
       public Func<byte> onGetLevelID = delegate { return 0; };
       public UnityAction onNextLevel = delegate() {  };
       public UnityAction onRestartLevel = delegate() {  };
       
    }
}