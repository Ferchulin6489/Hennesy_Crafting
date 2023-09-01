// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.MinimapIconInternalStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct MinimapIconInternalStruct
  {
    [FieldOffset(0)]
    public long IconNamePtr;
    [FieldOffset(8)]
    public int MinimapIconRadius;
    [FieldOffset(12)]
    public int LargemapIconRadius;
    [FieldOffset(19)]
    public int MinimapIconPointerMaxDistance;
  }
}
