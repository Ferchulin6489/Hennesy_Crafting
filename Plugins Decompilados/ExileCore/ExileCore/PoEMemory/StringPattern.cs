// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.StringPattern
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExileCore.PoEMemory
{
  public class StringPattern : IPattern
  {
    private string _mask;

    public StringPattern(string pattern, string name)
    {
      List<string> list = ((IEnumerable<string>) pattern.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
      int index = list.FindIndex((Predicate<string>) (x => x == "^"));
      if (index == -1)
        index = 0;
      else
        list.RemoveAt(index);
      this.PatternOffset = index;
      this.Bytes = list.Select<string, byte>((Func<string, byte>) (x => !(x == "??") ? byte.Parse(x, NumberStyles.HexNumber) : (byte) 0)).ToArray<byte>();
      this.Mask = list.Select<string, bool>((Func<string, bool>) (x => x != "??")).ToArray<bool>();
      Span<bool> span1 = this.Mask.AsSpan<bool>();
      Span<byte> span2 = this.Bytes.AsSpan<byte>();
      this.Name = name;
      while (span1.Length > 0 && !span1[0])
      {
        this.PatternOffset = this.PatternOffset - 1;
        ref Span<bool> local1 = ref span1;
        span1 = local1.Slice(1, local1.Length - 1);
        ref Span<byte> local2 = ref span2;
        span2 = local2.Slice(1, local2.Length - 1);
      }
      while (span1.Length > 0)
      {
        ref Span<bool> local3 = ref span1;
        if (!local3[local3.Length - 1])
        {
          ref Span<bool> local4 = ref span1;
          span1 = local4.Slice(0, local4.Length - 1);
          ref Span<byte> local5 = ref span2;
          span2 = local5.Slice(0, local5.Length - 1);
        }
        else
          break;
      }
      this.Mask = span1.ToArray();
      this.Bytes = span2.ToArray();
    }

    public string Name { get; }

    public byte[] Bytes { get; }

    public bool[] Mask { get; }

    public int StartOffset { get; init; }

    public int PatternOffset { get; }

    string IPattern.Mask => this._mask ?? (this._mask = new string(((IEnumerable<bool>) this.Mask).Select<bool, char>((Func<bool, char>) (x => !x ? '?' : 'x')).ToArray<char>()));
  }
}
