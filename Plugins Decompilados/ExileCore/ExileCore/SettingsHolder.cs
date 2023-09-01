// Decompiled with JetBrains decompiler
// Type: ExileCore.SettingsHolder
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ExileCore
{
  public class SettingsHolder : ISettingsHolder
  {
    public SettingsHolder() => this.Tooltip = "";

    public string Name { get; set; } = "";

    public string Tooltip { get; set; }

    public string Unique
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
        interpolatedStringHandler.AppendFormatted(this.Name);
        interpolatedStringHandler.AppendLiteral("##");
        interpolatedStringHandler.AppendFormatted<int>(this.ID);
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    public int ID { get; set; } = -1;

    public Action DrawDelegate { get; set; }

    public IList<ISettingsHolder> Children { get; } = (IList<ISettingsHolder>) new List<ISettingsHolder>();

    public Func<bool> DisplayCondition { get; set; }

    public bool CollapsedByDefault { get; set; }

    public void Draw()
    {
      Func<bool> displayCondition = this.DisplayCondition;
      if ((displayCondition != null ? (!displayCondition() ? 1 : 0) : 0) != 0)
        return;
      if (this.Children.Count > 0)
      {
        ImGui.Spacing();
        Vector2 cursorScreenPos1 = ImGui.GetCursorScreenPos();
        int num = ImGui.TreeNodeEx(this.Unique + "treeNode", this.CollapsedByDefault ? ImGuiTreeNodeFlags.AllowItemOverlap : ImGuiTreeNodeFlags.AllowItemOverlap | ImGuiTreeNodeFlags.DefaultOpen) ? 1 : 0;
        string tooltip = this.Tooltip;
        if ((tooltip != null ? (tooltip.Length > 0 ? 1 : 0) : 0) != 0)
        {
          ImGui.SameLine();
          ImGui.TextDisabled("(?)");
          if (ImGui.IsItemHovered(ImGuiHoveredFlags.None))
            ImGui.SetTooltip(this.Tooltip);
        }
        if (num != 0)
        {
          ImGui.Unindent();
          ImGui.Indent(10f);
          ImGui.Spacing();
          foreach (ISettingsHolder child in (IEnumerable<ISettingsHolder>) this.Children)
            child.Draw();
          ImGui.Unindent(10f);
          Vector2 cursorScreenPos2 = ImGui.GetCursorScreenPos();
          ImGui.GetWindowDrawList().AddLine(cursorScreenPos1, cursorScreenPos2, ImGui.GetColorU32(ImGuiCol.FrameBgActive));
          ImGui.Spacing();
          ImGui.Spacing();
          ImGui.Indent();
          ImGui.TreePop();
        }
        Action drawDelegate = this.DrawDelegate;
        if (drawDelegate == null)
          return;
        drawDelegate();
      }
      else
      {
        Action drawDelegate = this.DrawDelegate;
        if (drawDelegate != null)
          drawDelegate();
        string tooltip = this.Tooltip;
        if ((tooltip != null ? (tooltip.Length > 0 ? 1 : 0) : 0) == 0)
          return;
        ImGui.SameLine();
        ImGui.TextDisabled("(?)");
        if (!ImGui.IsItemHovered(ImGuiHoveredFlags.None))
          return;
        ImGui.SetTooltip(this.Tooltip);
      }
    }
  }
}
