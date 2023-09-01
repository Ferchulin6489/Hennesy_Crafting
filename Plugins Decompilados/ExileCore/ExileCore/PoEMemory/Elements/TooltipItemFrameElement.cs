// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.TooltipItemFrameElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements
{
  public class TooltipItemFrameElement : Element
  {
    private readonly CachedValue<TooltipItemFrameElementOffsets> _cachedValue;

    public TooltipItemFrameElement() => this._cachedValue = (CachedValue<TooltipItemFrameElementOffsets>) this.CreateStructFrameCache<TooltipItemFrameElementOffsets>();

    public string CopyText => this.M.ReadStringU(this._cachedValue.Value.CopyTextPtr, 5000);

    public bool IsAdvancedTooltipText => this._cachedValue.Value.IsAdvancedTooltipText;
  }
}
