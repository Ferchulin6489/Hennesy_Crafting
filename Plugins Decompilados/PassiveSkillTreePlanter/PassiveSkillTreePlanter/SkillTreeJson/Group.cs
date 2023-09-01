// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.SkillTreeJson.Group
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using System.Collections.Generic;

namespace PassiveSkillTreePlanter.SkillTreeJson
{
  public class Group
  {
    public double X { get; set; }

    public double Y { get; set; }

    public List<long> Orbits { get; set; }

    public List<ushort> Nodes { get; set; }

    public bool IsProxy { get; set; }
  }
}
