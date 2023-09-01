// Decompiled with JetBrains decompiler
// Type: GameOffsets.ServerStashTabOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ServerStashTabOffsets
  {
    public const int StructSize = 104;
    [FieldOffset(8)]
    public NativeStringU Name;
    [FieldOffset(44)]
    public uint Color;
    [FieldOffset(52)]
    public uint OfficerFlags;
    [FieldOffset(52)]
    public uint TabType;
    [FieldOffset(56)]
    public ushort DisplayIndex;
    [FieldOffset(60)]
    public uint MemberFlags;
    [FieldOffset(61)]
    public byte Flags;
  }
}
