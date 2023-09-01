// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.InventoryElements.EssenceInventoryItem
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;

namespace ExileCore.PoEMemory.Elements.InventoryElements
{
  public class EssenceInventoryItem : NormalInventoryItem
  {
    public override int InventPosX => 0;

    public override int InventPosY => 0;

    public override RectangleF GetClientRect()
    {
      Element parent = this.Parent;
      return parent == null ? RectangleF.Empty : parent.GetClientRect();
    }
  }
}
