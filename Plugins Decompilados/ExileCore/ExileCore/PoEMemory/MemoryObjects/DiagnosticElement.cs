// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.DiagnosticElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using ProcessMemoryUtilities.Managed;
using System;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class DiagnosticElement : RemoteMemoryObject
  {
    private readonly CachedValue<DiagnosticElementOffsets> _cachedValue;
    private readonly CachedValue<DiagnosticElementArrayOffsets> _cachedValue2;
    private readonly FrameCache<float[]> Values;

    public DiagnosticElement()
    {
      this._cachedValue = (CachedValue<DiagnosticElementOffsets>) new FrameCache<DiagnosticElementOffsets>((Func<DiagnosticElementOffsets>) (() => this.M.Read<DiagnosticElementOffsets>(this.Address)));
      this._cachedValue2 = (CachedValue<DiagnosticElementArrayOffsets>) new FrameCache<DiagnosticElementArrayOffsets>((Func<DiagnosticElementArrayOffsets>) (() => this.M.Read<DiagnosticElementArrayOffsets>(this.DiagnosticElementStruct.DiagnosticArray)));
      this.Values = new FrameCache<float[]>((Func<float[]>) (() =>
      {
        float[] buffer = new float[80];
        NativeWrapper.ReadProcessMemoryArray<float>(this.M.OpenProcessHandle, (IntPtr) this.DiagnosticElementStruct.DiagnosticArray, buffer, 0, 80);
        return buffer;
      }));
    }

    private DiagnosticElementOffsets DiagnosticElementStruct => this._cachedValue.Value;

    private DiagnosticElementArrayOffsets DiagnosticElementArrayStruct => this._cachedValue2.Value;

    public long DiagnosticArray => this.DiagnosticElementStruct.DiagnosticArray;

    public float[] DiagnosticArrayValues => this.Values.Value;

    public float CurrValue => this.DiagnosticElementArrayStruct.CurrValue;

    public int X => this.DiagnosticElementStruct.X;

    public int Y => this.DiagnosticElementStruct.Y;

    public int Width => this.DiagnosticElementStruct.Width;

    public int Height => this.DiagnosticElementStruct.Height;
  }
}
