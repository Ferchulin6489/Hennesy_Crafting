// Decompiled with JetBrains decompiler
// Type: ExileCore.DefaultMemoryBackend
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using ProcessMemoryUtilities.Managed;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;

namespace ExileCore
{
  public class DefaultMemoryBackend : IMemoryBackend, IDisposable
  {
    private static readonly DebugInformation PerFrameStats = new DebugInformation("Memory reading");
    private long _currentFrameUsedTime;
    private readonly IntPtr _openProcessHandle;

    public DefaultMemoryBackend(IntPtr processHandle) => this._openProcessHandle = processHandle;

    public bool TryReadMemory(IntPtr address, Span<byte> target)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      byte[] numArray = ArrayPool<byte>.Shared.Rent(target.Length);
      try
      {
        int num = NativeWrapper.ReadProcessMemoryArray<byte>(this._openProcessHandle, address, numArray, 0, target.Length) ? 1 : 0;
        numArray.AsSpan<byte>().Slice(0, target.Length).CopyTo(target);
        return num != 0;
      }
      finally
      {
        ArrayPool<byte>.Shared.Return(numArray);
        Interlocked.Add(ref this._currentFrameUsedTime, stopwatch.ElapsedTicks);
      }
    }

    public void NotifyFrame()
    {
      long ticks = Interlocked.Exchange(ref this._currentFrameUsedTime, 0L);
      DefaultMemoryBackend.PerFrameStats.Tick = new TimeSpan(ticks).TotalMilliseconds;
    }

    public void Dispose()
    {
    }
  }
}
