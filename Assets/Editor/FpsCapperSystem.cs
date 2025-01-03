using UnityEditor;
using UnityEngine;
using System.Threading;

namespace Klak.KlutterTools {

// FPS Capper player loop sub system
[InitializeOnLoad]
sealed class FpsCapperSystem
{
    // Synchronization object
    static AutoResetEvent _sync;

    // Interval in milliseconds
    static int IntervalMsec;

    // Interval thread function
    static void IntervalThread()
    {
        for (_sync = new AutoResetEvent(true); true;)
        {
            Thread.Sleep(Mathf.Max(1, IntervalMsec));
            _sync.Set();
        }
    }

    // Custom system update function
    static void UpdateSystem()
    {
        // Property update
        IntervalMsec = 1000 / Mathf.Max(5, Preferences.FpsCapperFrameRate);

        // Rejection cases
        if (_sync == null) return;
        if (!Preferences.FpsCapperEnable) return;
        if (Preferences.FpsCapperFrameRate < 1) return;
        if (Time.captureDeltaTime != 0) return;

        // Synchronization with the interval thread
        _sync.WaitOne();
    }

    // Static constructor
    static FpsCapperSystem()
    {
        // Interval thread launch
        new Thread(IntervalThread).Start();

        // Player loop sub system installation
        PlayerLoopHelper.AddToSubSystem
          <UnityEngine.PlayerLoop.EarlyUpdate, FpsCapperSystem>
          (UpdateSystem);
    }
}

} // namespace Klak.KlutterTools 
