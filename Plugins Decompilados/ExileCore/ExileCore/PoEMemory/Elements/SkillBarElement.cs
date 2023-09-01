// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.SkillBarElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class SkillBarElement : Element
  {
    public long TotalSkills => this.ChildCount;

    public List<SkillElement> Skills => this.GetChildrenAs<SkillElement>();

    public SkillElement this[int k] => this.Children[k].AsObject<SkillElement>();
  }
}
