// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.PoeChatElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class PoeChatElement : Element
  {
    public long TotalMessageCount => this.ChildCount;

    public EntityLabel this[int index] => (long) index < this.TotalMessageCount ? this.GetChildAtIndex(index).AsObject<EntityLabel>() : (EntityLabel) null;

    public List<Element> MessageElements => this.GetChildrenAs<Element>();

    public List<string> Messages => this.MessageElements.Select<Element, string>((Func<Element, string>) (x => x.Text)).ToList<string>();
  }
}
