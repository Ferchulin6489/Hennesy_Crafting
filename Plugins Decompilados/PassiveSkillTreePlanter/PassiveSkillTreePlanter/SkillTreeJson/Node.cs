// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.SkillTreeJson.Node
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using System.Collections.Generic;

namespace PassiveSkillTreePlanter.SkillTreeJson
{
  public class Node
  {
    public long Skill { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public bool IsNotable { get; set; }

    public List<PassiveSkillTreePlanter.SkillTreeJson.Recipe> Recipe { get; set; }

    public List<string> Stats { get; set; }

    public long Group { get; set; }

    public int Orbit { get; set; }

    public long OrbitIndex { get; set; }

    public List<ushort> Out { get; set; }

    public List<long> In { get; set; }

    public List<string> ReminderText { get; set; }

    public bool IsMastery { get; set; }

    public long GrantedStrength { get; set; }

    public string AscendancyName { get; set; }

    public long GrantedDexterity { get; set; }

    public bool IsAscendancyStart { get; set; }

    public bool IsMultipleChoice { get; set; }

    public long GrantedIntelligence { get; set; }

    public bool IsJewelSocket { get; set; }

    public ExpansionJewel ExpansionJewel { get; set; }

    public long GrantedPassivePoints { get; set; }

    public bool IsKeystone { get; set; }

    public List<string> FlavourText { get; set; }

    public bool IsProxy { get; set; }

    public bool IsMultipleChoiceOption { get; set; }

    public bool IsBlighted { get; set; }

    public int? ClassStartIndex { get; set; }
  }
}
