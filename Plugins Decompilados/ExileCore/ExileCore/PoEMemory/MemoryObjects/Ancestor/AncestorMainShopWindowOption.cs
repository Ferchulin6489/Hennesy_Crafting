// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Ancestor.AncestorMainShopWindowOption
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory.Ancestor;
using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.MemoryObjects.Ancestor
{
  public class AncestorMainShopWindowOption : Element
  {
    private readonly CachedValue<AncestorShopWindowOffsets> _cachedValue;

    public AncestorMainShopWindowOption() => this._cachedValue = (CachedValue<AncestorShopWindowOffsets>) this.CreateStructFrameCache<AncestorShopWindowOffsets>();

    public AncestralTrialUnit Unit => this.TheGame.Files.AncestralTrialUnits.GetByAddress(this._cachedValue.Value.UnitPtr);

    public AncestralTrialItem Item => this.TheGame.Files.AncestralTrialItems.GetByAddress(this._cachedValue.Value.ItemPtr);
  }
}
