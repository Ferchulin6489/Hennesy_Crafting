// Decompiled with JetBrains decompiler
// Type: GameOffsets.SkillGemOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct SkillGemOffsets
  {
    [FieldOffset(0)]
    public InitObjectOffsets Head;
    [FieldOffset(32)]
    public long AdvanceInformation;
    [FieldOffset(40)]
    public uint TotalExpGained;
    [FieldOffset(44)]
    public uint Level;
    [FieldOffset(48)]
    public uint ExperiencePrevLevel;
    [FieldOffset(52)]
    public uint ExperienceMaxLevel;
    [FieldOffset(60)]
    public uint QualityType;
  }
}
