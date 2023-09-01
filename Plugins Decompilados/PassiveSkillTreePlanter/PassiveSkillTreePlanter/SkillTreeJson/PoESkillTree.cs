// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.SkillTreeJson.PoESkillTree
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using System.Collections.Generic;

namespace PassiveSkillTreePlanter.SkillTreeJson
{
  public class PoESkillTree
  {
    public List<Class> Classes { get; set; }

    public Dictionary<string, Group> Groups { get; set; }

    public Dictionary<string, Node> Nodes { get; set; }

    public List<long> JewelSlots { get; set; }

    public long Min_X { get; set; }

    public long Min_Y { get; set; }

    public long Max_X { get; set; }

    public long Max_Y { get; set; }

    public Constants Constants { get; set; }
  }
}
