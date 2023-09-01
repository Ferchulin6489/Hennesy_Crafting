// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.InventoryElements.NormalInventoryItem
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Elements.InventoryElements
{
  public class NormalInventoryItem : Element
  {
    private Entity _item;
    private readonly Lazy<NormalInventoryItemOffsets> cachedValue;

    public NormalInventoryItem() => this.cachedValue = new Lazy<NormalInventoryItemOffsets>((Func<NormalInventoryItemOffsets>) (() => this.M.Read<NormalInventoryItemOffsets>(this.Address)));

    [Obsolete]
    public virtual int InventPosX => 0;

    [Obsolete]
    public virtual int InventPosY => 0;

    public virtual int ItemWidth => this.cachedValue.Value.Width;

    public virtual int ItemHeight => this.cachedValue.Value.Height;

    public Entity Item => this._item ?? (this._item = this.GetObject<Entity>(this.cachedValue.Value.Item));

    public ToolTipType toolTipType => ToolTipType.InventoryItem;
  }
}
