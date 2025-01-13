using System;
using UnityEngine.LowLevel;

namespace KlutterTools {

public static class PlayerLoopHelper
{
    public static void AddToSubSystem<P, T>
      (PlayerLoopSystem.UpdateFunction updateFunc)
    {
        var loop = PlayerLoop.GetCurrentPlayerLoop();

        for (var i = 0; i < loop.subSystemList.Length; i++) 
        {
            ref var subsys = ref loop.subSystemList[i];
            if (subsys.type != typeof(P)) continue;

            var target = new PlayerLoopSystem 
                { type = typeof(T), updateDelegate = updateFunc };

            var len = subsys.subSystemList.Length;
            Array.Resize(ref subsys.subSystemList, len + 1);
            subsys.subSystemList[len] = target;

            PlayerLoop.SetPlayerLoop(loop);
            return;
        }

        throw new InvalidOperationException
          ($"Can't find player sub system {typeof(P)}");
    }
}

} // namespace KlutterTools
