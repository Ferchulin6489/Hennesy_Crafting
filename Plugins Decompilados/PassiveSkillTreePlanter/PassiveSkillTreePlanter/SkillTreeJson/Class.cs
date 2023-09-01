// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.SkillTreeJson.Class
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using System.Collections.Generic;

namespace PassiveSkillTreePlanter.SkillTreeJson
{
  public class Class
  {
    public string Name { get; set; }

    public long BaseStr { get; set; }

    public long BaseDex { get; set; }

    public long BaseInt { get; set; }

    public List<Ascendancy> Ascendancies { get; set; }
  }
}
