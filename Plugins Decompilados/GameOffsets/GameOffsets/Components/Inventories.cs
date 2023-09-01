// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.Inventories
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct Inventories
  {
    public const int StartingPointOffset = 64;
    public const int DataStructureLength = 96;
    [FieldOffset(0)]
    public ComponentHeader Header;
    [FieldOffset(64)]
    public long LeftHandPtr;
    [FieldOffset(160)]
    public long RightHandPtr;
    [FieldOffset(256)]
    public long ChestPtr;
    [FieldOffset(352)]
    public long HelmPtr;
    [FieldOffset(448)]
    public long GlovesPtr;
    [FieldOffset(544)]
    public long BootsPtr;
    [FieldOffset(640)]
    public long UnknownPtr;
    [FieldOffset(736)]
    public long LeftRingPtr;
    [FieldOffset(832)]
    public long RightRingPtr;
    [FieldOffset(928)]
    public long BeltPtr;
  }
}
