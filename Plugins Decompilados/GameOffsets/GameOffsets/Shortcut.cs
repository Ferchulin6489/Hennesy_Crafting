// Decompiled with JetBrains decompiler
// Type: GameOffsets.Shortcut
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Size = 12, Pack = 1)]
  public struct Shortcut
  {
    [FieldOffset(0)]
    public ConsoleKey MainKey;
    [FieldOffset(4)]
    public ShortcutModifier Modifier;
    [FieldOffset(8)]
    public ShortcutUsage Usage;

    public string ModifierText
    {
      get
      {
        if (this.Modifier == ShortcutModifier.None)
          return (string) null;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
        interpolatedStringHandler.AppendFormatted<ShortcutModifier>(this.Modifier);
        interpolatedStringHandler.AppendLiteral(" + ");
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
      interpolatedStringHandler.AppendFormatted(this.ModifierText);
      interpolatedStringHandler.AppendFormatted<ConsoleKey>(this.MainKey);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<ShortcutUsage>(this.Usage);
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
