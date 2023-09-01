// Decompiled with JetBrains decompiler
// Type: GameOffsets.HeistBlueprintComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct HeistBlueprintComponentOffsets
  {
    public static readonly int WingRecordSize = 80;
    public static readonly int JobRecordSize = 24;
    public static readonly int RoomRecordSize = 24;
    public static readonly int NpcRecordSize = 16;
    [FieldOffset(8)]
    public long Owner;
    [FieldOffset(28)]
    public byte AreaLevel;
    [FieldOffset(30)]
    public byte IsConfirmed;
    [FieldOffset(32)]
    public NativePtrArray Wings;
  }
}
