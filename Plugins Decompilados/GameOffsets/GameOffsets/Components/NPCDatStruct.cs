﻿// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.NPCDatStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct NPCDatStruct
  {
    [FieldOffset(0)]
    public long IdStringPtr;
    [FieldOffset(8)]
    public long NamePtr;
    [FieldOffset(16)]
    public long MetadataPtr;
    [FieldOffset(24)]
    public int Unknown0;
    [FieldOffset(36)]
    public long NpcMasterPtr;
    [FieldOffset(44)]
    public long ShortNamePtr;
    [FieldOffset(52)]
    public int Act;
  }
}
