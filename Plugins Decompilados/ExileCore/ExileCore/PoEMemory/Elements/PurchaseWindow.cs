// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.PurchaseWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements
{
  public class PurchaseWindow : Element
  {
    private readonly CachedValue<PurchaseWindowOffsets> _cache;

    public PurchaseWindow() => this._cache = (CachedValue<PurchaseWindowOffsets>) this.CreateStructFrameCache<PurchaseWindowOffsets>();

    public Element CloseButton => this[2];

    public VendorStashTabContainer TabContainer => this.GetObject<VendorStashTabContainer>(this._cache.Value.StashTabContainerPtr);
  }
}
