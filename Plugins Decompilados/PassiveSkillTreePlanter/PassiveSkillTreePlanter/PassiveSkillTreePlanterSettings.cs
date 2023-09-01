// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.PassiveSkillTreePlanterSettings
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace PassiveSkillTreePlanter
{
  public class PassiveSkillTreePlanterSettings : ISettings
  {
    public string SelectedBuild { get; set; } = string.Empty;

    public string LastSelectedCharacterUrl { get; set; }

    public string LastSelectedAtlasUrl { get; set; }

    public RangeNode<int> LineWidth { get; set; } = new RangeNode<int>(3, 0, 5);

    public ColorNode PickedBorderColor { get; set; } = new ColorNode();

    public ColorNode UnpickedBorderColor { get; set; } = new ColorNode(Color.Green);

    public ColorNode WrongPickedBorderColor { get; set; } = new ColorNode(Color.Red);

    public ToggleNode EnableEzTreeChanger { get; set; } = new ToggleNode(true);

    public ToggleNode SaveChangesAutomatically { get; set; } = new ToggleNode(true);

    public ToggleNode Enable { get; set; } = new ToggleNode(false);
  }
}
