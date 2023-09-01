// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Pattern
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExileCore.PoEMemory
{
  public class Pattern : IPattern
  {
    public Pattern(byte[] pattern, string mask, string name, int startOffset = 0)
    {
      this.Bytes = pattern;
      this.Mask = Regex.Replace(mask, "\\s+", "");
      this.Name = name;
      this.StartOffset = startOffset;
    }

    public Pattern(string pattern, string mask, string name, int startOffset = 0)
    {
      this.Bytes = ((IEnumerable<string>) pattern.Split(new string[1]
      {
        "\\x"
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, byte>((Func<string, byte>) (y => byte.Parse(y, NumberStyles.HexNumber))).ToArray<byte>();
      this.Mask = mask;
      this.Name = name;
      this.StartOffset = startOffset;
    }

    public string Name { get; }

    public byte[] Bytes { get; }

    public string Mask { get; }

    public int StartOffset { get; }
  }
}
