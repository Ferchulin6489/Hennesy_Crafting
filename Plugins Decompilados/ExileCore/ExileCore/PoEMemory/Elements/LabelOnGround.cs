// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.LabelOnGround
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using System;

namespace ExileCore.PoEMemory.Elements
{
  public class LabelOnGround : RemoteMemoryObject
  {
    private readonly Lazy<string> debug;
    private readonly Lazy<long> labelInfo;

    public LabelOnGround()
    {
      this.labelInfo = new Lazy<long>(new Func<long>(this.GetLabelInfo));
      this.debug = new Lazy<string>((Func<string>) (() =>
      {
        if (!this.ItemOnGround.HasComponent<WorldItem>())
          return this.ItemOnGround.Path;
        return this.ItemOnGround.GetComponent<WorldItem>().ItemEntity?.GetComponent<Base>()?.Name;
      }));
    }

    public bool IsVisible
    {
      get
      {
        Element label = this.Label;
        return label != null && label.IsVisible;
      }
    }

    public bool IsVisibleLocal
    {
      get
      {
        Element label = this.Label;
        return label != null && label.IsVisibleLocal;
      }
    }

    public Entity ItemOnGround => this.ReadObjectAt<Entity>(24);

    public Element Label => this.ReadObjectAt<Element>(16);

    public bool CanPickUp => true;

    public TimeSpan TimeLeft => TimeSpan.Zero;

    public TimeSpan MaxTimeForPickUp => TimeSpan.Zero;

    private long GetLabelInfo() => this.Label == null || this.Label.Address == 0L ? 0L : this.M.Read<long>(this.Label.Address + 952L);

    public override string ToString() => this.debug.Value;
  }
}
