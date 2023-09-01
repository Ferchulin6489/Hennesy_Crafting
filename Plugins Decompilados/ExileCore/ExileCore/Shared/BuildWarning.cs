// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.BuildWarning
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class BuildWarning
  {
    public string File { get; set; }

    public DateTime Timestamp { get; set; }

    public int LineNumber { get; set; }

    public int ColumnNumber { get; set; }

    public string Code { get; set; }

    public string Message { get; set; }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 6);
      interpolatedStringHandler.AppendLiteral("[");
      interpolatedStringHandler.AppendFormatted<DateTime>(this.Timestamp);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.File);
      interpolatedStringHandler.AppendLiteral("(");
      interpolatedStringHandler.AppendFormatted<int>(this.LineNumber);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted<int>(this.ColumnNumber);
      interpolatedStringHandler.AppendLiteral(")] ");
      interpolatedStringHandler.AppendFormatted(this.Code);
      interpolatedStringHandler.AppendLiteral(": ");
      interpolatedStringHandler.AppendFormatted(this.Message);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
