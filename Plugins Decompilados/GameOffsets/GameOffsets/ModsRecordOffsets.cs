// Decompiled with JetBrains decompiler
// Type: GameOffsets.ModsRecordOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ModsRecordOffsets
  {
    [FieldOffset(0)]
    public NativeStringU Key;
    [FieldOffset(8)]
    public int Hash;
    [FieldOffset(20)]
    public long TypeName;
    [FieldOffset(28)]
    public int MinLevel;
    [FieldOffset(32)]
    public long StatNames1;
    [FieldOffset(48)]
    public long StatNames2;
    [FieldOffset(64)]
    public long StatNames3;
    [FieldOffset(80)]
    public long StatName4;
    [FieldOffset(96)]
    public int Domain;
    [FieldOffset(100)]
    public long UserFriendlyName;
    [FieldOffset(108)]
    public int AffixType;
    [FieldOffset(112)]
    public (long Count, long Ptr) Group;
    [FieldOffset(120)]
    public long Something;
    [FieldOffset(128)]
    public int StatRange1;
    [FieldOffset(132)]
    public int StatRange2;
    [FieldOffset(136)]
    public int StatRange3;
    [FieldOffset(140)]
    public int StatRange4;
    [FieldOffset(144)]
    public int StatRange5;
    [FieldOffset(148)]
    public int StatRange6;
    [FieldOffset(152)]
    public int StatRange7;
    [FieldOffset(156)]
    public int StatRange8;
    [FieldOffset(160)]
    public long Tags;
    [FieldOffset(168)]
    public long ta;
    [FieldOffset(176)]
    public int TagChances;
    [FieldOffset(184)]
    public long tc;
    [FieldOffset(192)]
    public long BuffDefinitionsPtr;
    [FieldOffset(200)]
    public long BuffDefinitions;
    [FieldOffset(208)]
    public int BuffValue;
    [FieldOffset(212)]
    public long tgcCount;
    [FieldOffset(204)]
    public long tgcPtr;
    [FieldOffset(436)]
    public byte IsEssence;
    [FieldOffset(461)]
    public long Tier;
  }
}
