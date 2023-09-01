// Decompiled with JetBrains decompiler
// Type: ExileCore.DebugMsgDescription
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System;

namespace ExileCore
{
  public class DebugMsgDescription
  {
    public string Msg { get; set; }

    public DateTime Time { get; set; }

    public System.Numerics.Vector4 ColorV4 { get; set; }

    public Color Color { get; set; }

    public int Count { get; set; }
  }
}
