using System;
using UnityEngine;

namespace KlutterTools.Downloader {

public static class AwaitableExtensions
{
    public static async void Forget(this Awaitable awaitable)
    {
        try
        {
            await awaitable;
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

} // namespace KlutterTools.Downloader
