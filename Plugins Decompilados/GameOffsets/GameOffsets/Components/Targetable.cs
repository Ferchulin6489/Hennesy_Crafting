// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.Targetable
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct Targetable
  {
    [FieldOffset(0)]
    public ComponentHeader Header;
    [FieldOffset(40)]
    public long UnknownPtr0;
    [FieldOffset(48)]
    public byte IsTargetable;
    [FieldOffset(128)]
    public byte IsHighlightable;
    [FieldOffset(128)]
    public byte IsTargetted;
    [FieldOffset(51)]
    public byte UnknownBool0;
    [FieldOffset(52)]
    public byte UnknownBool1;
    [FieldOffset(53)]
    public byte UnknownBool2;
    [FieldOffset(54)]
    public byte UnknownBool3;
    [FieldOffset(55)]
    public byte UnknownBool4;
    [FieldOffset(56)]
    public int UnknownInt0;
    [FieldOffset(60)]
    public int UnknownInt1;
    [FieldOffset(64)]
    public int UnknownInt2;
    [FieldOffset(68)]
    public int UnknownInt3;
  }
}
