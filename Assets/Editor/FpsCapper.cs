using UnityEditor;
using UnityEngine;
using System.Threading;

namespace Klak.EditorTools {

// Preference accessor
public static class FpsCapperPreference
{
    public const string EnableKey = "FpsCapper.Enable";
    public const string TargetFrameRateKey = "FpsCapper.TargetFrameRate";

    public static bool Enable
      { get => EditorPrefs.GetBool(EnableKey, false);
        set => EditorPrefs.SetBool(EnableKey, value); }

    public static int TargetFrameRate
      { get => EditorPrefs.GetInt(TargetFrameRateKey, 60);
        set => EditorPrefs.SetInt(TargetFrameRateKey, value); }
}

// Preference provider (GUI)
sealed class FpsCapperPreferenceProvider : SettingsProvider
{
    public FpsCapperPreferenceProvider()
      : base("Preferences/FPS Capper", SettingsScope.User) {}

    public override void OnGUI(string search)
    {
        EditorGUI.BeginChangeCheck();

        var enable = FpsCapperPreference.Enable;
        var fps = FpsCapperPreference.TargetFrameRate;

        enable = EditorGUILayout.Toggle("Enable", enable);
        fps = EditorGUILayout.IntField("Target Frame Rate", fps);

        if (EditorGUI.EndChangeCheck())
        {
            FpsCapperPreference.Enable = enable;
            FpsCapperPreference.TargetFrameRate = fps;
        }

        base.OnGUI(search);
    }

    [SettingsProvider]
    public static SettingsProvider PreferenceSettingsProvider()
      => new FpsCapperPreferenceProvider();
}

// FPS Capper player-loop system
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
        IntervalMsec =
          1000 / Mathf.Max(5, FpsCapperPreference.TargetFrameRate);

        // Rejection cases
        if (_sync == null) return;
        if (!FpsCapperPreference.Enable) return;
        if (FpsCapperPreference.TargetFrameRate < 1) return;
        if (Time.captureDeltaTime != 0) return;

        // Synchronization with the interval thread
        _sync.WaitOne();
    }

    // Static constructor
    static FpsCapperSystem()
    {
        // Interval thread launch
        new Thread(IntervalThread).Start();

        // Custom subsystem installation
        PlayerLoopHelper.AddToSubSystem
          <UnityEngine.PlayerLoop.EarlyUpdate, FpsCapperSystem>
          (UpdateSystem);
    }
}

} // namespace Klak.EditorTools 
