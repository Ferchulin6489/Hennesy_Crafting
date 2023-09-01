// Decompiled with JetBrains decompiler
// Type: GameOffsets.GemInformation
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct GemInformation
  {
    [FieldOffset(16)]
    public int SocketColor;
    [FieldOffset(40)]
    public int MaxLevel;
    [FieldOffset(44)]
    public int LimitLevel;
    [FieldOffset(56)]
    public long GrantedEffect1;
    [FieldOffset(72)]
    public long GrantedEffect1HardMode;
    [FieldOffset(88)]
    public long GrantedEffect2;
    [FieldOffset(104)]
    public long GrantedEffect2HardMode;
  }
}
