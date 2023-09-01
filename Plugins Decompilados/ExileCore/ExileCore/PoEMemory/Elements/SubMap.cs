// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.SubMap
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.PoEMemory.Elements
{
  public class SubMap : Element
  {
    [Obsolete]
    public SharpDX.Vector2 Shift => this.M.Read<SharpDX.Vector2>(this.Address + 616L);

    public System.Numerics.Vector2 ShiftNum => this.M.Read<System.Numerics.Vector2>(this.Address + 616L);

    [Obsolete]
    public SharpDX.Vector2 DefaultShift => this.M.Read<SharpDX.Vector2>(this.Address + 624L);

    public System.Numerics.Vector2 DefaultShiftNum => this.M.Read<System.Numerics.Vector2>(this.Address + 624L);

    public float Zoom => this.M.Read<float>(this.Address + 684L);
  }
}
