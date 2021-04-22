using System;
using System.Diagnostics;
using Microsoft.System;

namespace Microsoft.Maui.Essentials
{
	public static partial class MainThread
	{
		static bool PlatformIsMainThread
		{
			get
			{
                return DispatcherQueue.GetForCurrentThread()?.HasThreadAccess ?? false;
			}
		}

		static void PlatformBeginInvokeOnMainThread(Action action)
		{
            var dispatcher = DispatcherQueue.GetForCurrentThread();

            if (dispatcher == null)
                throw new InvalidOperationException("Unable to find main thread.");

            if (!dispatcher.TryEnqueue(DispatcherQueuePriority.Normal, () => action()))
                throw new InvalidOperationException("Unable to queue on the main thread.");
		}
	}
}
