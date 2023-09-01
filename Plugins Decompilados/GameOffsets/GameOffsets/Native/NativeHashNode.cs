// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.NativeHashNode
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct NativeHashNode
  {
    [FieldOffset(0)]
    public readonly long Previous;
    [FieldOffset(8)]
    public readonly long Root;
    [FieldOffset(16)]
    public readonly long Next;
    [FieldOffset(25)]
    public readonly byte IsNull;
    [FieldOffset(32)]
    public int Key;
    [FieldOffset(40)]
    public long Value1;

    public override string ToString() => nameof (NativeHashNode);
  }
}
