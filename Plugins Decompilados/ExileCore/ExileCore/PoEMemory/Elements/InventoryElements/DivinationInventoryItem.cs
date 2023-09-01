// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.InventoryElements.DivinationInventoryItem
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;

namespace ExileCore.PoEMemory.Elements.InventoryElements
{
  public class DivinationInventoryItem : NormalInventoryItem
  {
    public override int InventPosX => 0;

    public override int InventPosY => 0;

    public override RectangleF GetClientRect()
    {
      Element parent = this.Parent;
      RectangleF clientRect = parent != null ? parent.GetClientRect() : RectangleF.Empty;
      if (clientRect == RectangleF.Empty)
        return clientRect;
      long? address = this.Parent?.Parent?.Parent?.Parent?[2].Address;
      if (!address.HasValue)
        return clientRect;
      float num = (float) this.M.Read<int>(address.Value + 2660L) * 107.5f;
      clientRect.Y -= num;
      return clientRect;
    }
  }
}
