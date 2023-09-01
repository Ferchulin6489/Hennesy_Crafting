// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Offsets
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory
{
  public class Offsets
  {
    public static Offsets Regular = new Offsets()
    {
      IgsOffset = 0,
      IgsDelta = 0,
      ExeName = "PathOfExile"
    };
    public static Offsets Korean = new Offsets()
    {
      IgsOffset = 0,
      IgsDelta = 0,
      ExeName = "Pathofexile_KG"
    };
    public static Offsets Steam = new Offsets()
    {
      IgsOffset = 40,
      IgsDelta = 0,
      ExeName = "PathOfExileSteam"
    };
    private static readonly Pattern basePtrPattern = new Pattern(new byte[33]
    {
      (byte) 144,
      (byte) 72,
      (byte) 3,
      (byte) 216,
      (byte) 72,
      (byte) 139,
      (byte) 3,
      (byte) 72,
      (byte) 133,
      (byte) 192,
      (byte) 117,
      (byte) 0,
      (byte) 72,
      (byte) 139,
      (byte) 29,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 72,
      (byte) 139,
      (byte) 5,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 72,
      (byte) 133,
      (byte) 192,
      (byte) 116,
      (byte) 0,
      (byte) 102,
      (byte) 144
    }, "xxxxxxxxxxx?xxx????xxx????xxxx?xx", "BasePtr");
    private static readonly Pattern fileRootPattern = new Pattern(new byte[13]
    {
      (byte) 72,
      (byte) 139,
      (byte) 8,
      (byte) 72,
      (byte) 141,
      (byte) 61,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 139,
      (byte) 4,
      (byte) 14
    }, "xxxxxx????xxx", "File Root", 19726336);
    private static readonly Pattern areaChangePattern = new Pattern(new byte[23]
    {
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 232,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      (byte) 5,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 73,
      (byte) 141,
      (byte) 86,
      (byte) 64,
      (byte) 72,
      (byte) 139
    }, "x?????x????xx????xxxxxx", "Area change", 6881280);
    private static readonly Pattern isLoadingScreenPattern = new Pattern(new byte[18]
    {
      (byte) 72,
      (byte) 137,
      (byte) 5,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 72,
      (byte) 139,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 72,
      (byte) 137,
      (byte) 0,
      (byte) 72,
      (byte) 139,
      (byte) 198
    }, "xxx????xx???xx?xxx", "Loading");
    private static readonly Pattern GameStatePattern = new Pattern(new byte[23]
    {
      (byte) 72,
      (byte) 131,
      (byte) 236,
      (byte) 32,
      (byte) 72,
      (byte) 139,
      (byte) 241,
      (byte) 51,
      (byte) 237,
      (byte) 72,
      (byte) 57,
      (byte) 45,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 15,
      (byte) 133,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 65
    }, "xxx?xxxxxxxx????xx????x", "Game State");
    private static readonly Pattern DiagnosticInfoTypePattern = new Pattern(new byte[11]
    {
      (byte) 69,
      (byte) 51,
      (byte) 0,
      (byte) 68,
      (byte) 137,
      (byte) 0,
      (byte) 36,
      (byte) 64,
      (byte) 72,
      (byte) 139,
      (byte) 5
    }, "xx?xx?xxxxx", "DiagnosticInfoType", 600000);
    private static readonly StringPattern BlackBarSizePattern = new StringPattern("2B ?? ^ ?? ?? ?? ?? ?? F 57 ?? ?? 8B", "BlackBarSize");
    private static readonly StringPattern TerrainRotationSelectorPattern = new StringPattern("?? 8D ?? ^ ?? ?? ?? ?? ?? 0F B6 ?? ?? ?? ?? ?? ?? ?? 8B ?? ?? 3B ?? 89 ?? ?? ?? ?? ?? ?? ?? ?? ?? 0F 47 ?? ?? 8D ?? ?? ?? ?? ??", "Terrain Rotation Selector");
    private static readonly StringPattern TerrainRotationHelperPattern = new StringPattern("?? 8D ?? ?? ?? ?? ?? ?? 0F B6 ?? ?? ?? ?? ?? ?? ?? 8B ?? ?? 3B ?? 89 ?? ?? ?? ?? ?? ?? ?? ?? ?? 0F 47 ?? ?? 8D ?? ^ ?? ?? ?? ??", "Terrain Rotator Helper");

    public long AreaChangeCount { get; private set; }

    public long Base { get; private set; }

    public string ExeName { get; private set; }

    public long FileRoot { get; private set; }

    public int IgsDelta { get; private set; }

    public int IgsOffset { get; private set; }

    public int IgsOffsetDelta => this.IgsOffset + this.IgsDelta;

    public long isLoadingScreenOffset { get; private set; }

    public long GameStateOffset { get; private set; }

    public long DiagnosticInfoTypeOffset { get; private set; }

    public Dictionary<OffsetsName, long> DoPatternScans(IMemory m)
    {
      // ISSUE: variable of a compiler-generated type
      Offsets.\u003C\u003Ec__DisplayClass50_0 cDisplayClass500;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.m = m;
      IPattern[] second = new IPattern[7]
      {
        (IPattern) Offsets.fileRootPattern,
        (IPattern) Offsets.areaChangePattern,
        (IPattern) Offsets.GameStatePattern,
        (IPattern) Offsets.DiagnosticInfoTypePattern,
        (IPattern) Offsets.BlackBarSizePattern,
        (IPattern) Offsets.TerrainRotationSelectorPattern,
        (IPattern) Offsets.TerrainRotationHelperPattern
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.patternAddresses = ((IEnumerable<long>) cDisplayClass500.m.FindPatterns(second)).Zip<long, IPattern>((IEnumerable<IPattern>) second).ToDictionary<(long, IPattern), IPattern, long>((Func<(long, IPattern), IPattern>) (x => x.Second), (Func<(long, IPattern), long>) (x => x.First));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.patternOffsets = new Dictionary<Pattern, int>()
      {
        [Offsets.fileRootPattern] = 6,
        [Offsets.areaChangePattern] = 13,
        [Offsets.GameStatePattern] = 12,
        [Offsets.DiagnosticInfoTypePattern] = 11
      };
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.result = new Dictionary<OffsetsName, long>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.baseAddress = cDisplayClass500.m.Process.MainModule.BaseAddress.ToInt64();
      this.FileRoot = Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.fileRootPattern, OffsetsName.FileRoot, ref cDisplayClass500);
      this.AreaChangeCount = Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.areaChangePattern, OffsetsName.AreaChangeCount, ref cDisplayClass500);
      this.GameStateOffset = Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.GameStatePattern, OffsetsName.GameStateOffset, ref cDisplayClass500);
      this.DiagnosticInfoTypeOffset = Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.DiagnosticInfoTypePattern, OffsetsName.DiagnosticInfoTypeOffset, ref cDisplayClass500);
      Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.BlackBarSizePattern, OffsetsName.BlackBarSize, ref cDisplayClass500);
      Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.TerrainRotationSelectorPattern, OffsetsName.TerrainRotationSelector, ref cDisplayClass500);
      Offsets.\u003CDoPatternScans\u003Eg__ReadRelativeAddress\u007C50_2((IPattern) Offsets.TerrainRotationHelperPattern, OffsetsName.TerrainRotationHelper, ref cDisplayClass500);
      // ISSUE: reference to a compiler-generated field
      return cDisplayClass500.result;
    }
  }
}
