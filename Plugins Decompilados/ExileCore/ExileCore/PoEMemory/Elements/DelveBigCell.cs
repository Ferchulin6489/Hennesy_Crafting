// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.DelveBigCell
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
  public class DelveBigCell : Element
  {
    private readonly CachedValue<IList<DelveCell>> _cachedValue;
    private RectangleF rect = RectangleF.Empty;
    private string text;
    private long? type;

    public DelveBigCell() => this._cachedValue = (CachedValue<IList<DelveCell>>) new ConditionalCache<IList<DelveCell>>((Func<IList<DelveCell>>) (() => (IList<DelveCell>) this.Children.Select<Element, DelveCell>((Func<Element, DelveCell>) (x => x.AsObject<DelveCell>())).ToList<DelveCell>()), (Func<bool>) (() =>
    {
      if (!(this.GetClientRect() != this.rect))
        return false;
      this.rect = this.GetClientRect();
      return true;
    }));

    public IList<DelveCell> Cells => this._cachedValue.Value;

    public long TypePtr => this.type ?? (this.type = new long?(this.M.Read<long>(this.Address + 336L))).Value;

    public override string Text => this.text = this.text ?? this.M.ReadStringU(this.M.Read<long>(this.TypePtr));
  }
}
