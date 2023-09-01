// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.DelveElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class DelveElement : Element
  {
    private readonly CachedValue<IList<DelveBigCell>> _cachedValue;
    private RectangleF rect = RectangleF.Empty;

    public DelveElement() => this._cachedValue = (CachedValue<IList<DelveBigCell>>) new ConditionalCache<IList<DelveBigCell>>((Func<IList<DelveBigCell>>) (() => (IList<DelveBigCell>) this.Children.Select<Element, DelveBigCell>((Func<Element, DelveBigCell>) (x => x.AsObject<DelveBigCell>())).ToList<DelveBigCell>()), (Func<bool>) (() =>
    {
      if (!(this.GetClientRect() != this.rect))
        return false;
      this.rect = this.GetClientRect();
      return true;
    }));

    public IList<DelveBigCell> Cells => this._cachedValue.Value;
  }
}
