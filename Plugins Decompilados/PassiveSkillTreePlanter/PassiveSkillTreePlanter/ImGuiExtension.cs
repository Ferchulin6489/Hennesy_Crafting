// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.ImGuiExtension
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using ImGuiNET;
using System.Collections.Generic;

namespace PassiveSkillTreePlanter
{
  public class ImGuiExtension
  {
    public static string ComboBox(
      string sideLabel,
      string currentSelectedItem,
      List<string> objectList,
      out bool didChange,
      ImGuiComboFlags comboFlags = ImGuiComboFlags.HeightRegular)
    {
      if (ImGui.BeginCombo(sideLabel, currentSelectedItem, comboFlags))
      {
        foreach (string label in objectList)
        {
          bool selected = currentSelectedItem == label;
          if (ImGui.Selectable(label, selected))
          {
            didChange = true;
            ImGui.EndCombo();
            return label;
          }
          if (selected)
            ImGui.SetItemDefaultFocus();
        }
        ImGui.EndCombo();
      }
      didChange = false;
      return currentSelectedItem;
    }
  }
}
