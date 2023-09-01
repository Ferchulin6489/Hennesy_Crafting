// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Attributes.MenuAttribute
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class MenuAttribute : Attribute
  {
    public int index = -1;
    public string MenuName;
    public int parentIndex = -1;
    public string Tooltip;

    public bool CollapsedByDefault { get; set; }

    public MenuAttribute(string menuName) => this.MenuName = menuName;

    public MenuAttribute(string menuName, string tooltip)
      : this(menuName)
    {
      this.Tooltip = tooltip;
    }

    public MenuAttribute(string menuName, int index)
    {
      this.MenuName = menuName;
      this.index = index;
    }

    public MenuAttribute(string menuName, string tooltip, int index)
      : this(menuName, index)
    {
      this.Tooltip = tooltip;
    }

    public MenuAttribute(string menuName, int index, int parentIndex)
    {
      this.MenuName = menuName;
      this.index = index;
      this.parentIndex = parentIndex;
    }

    public MenuAttribute(string menuName, string tooltip, int index, int parentIndex)
      : this(menuName, index, parentIndex)
    {
      this.Tooltip = tooltip;
    }
  }
}
