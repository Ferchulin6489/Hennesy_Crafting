// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.HotkeyNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ImGuiNET;
using Newtonsoft.Json;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ExileCore.Shared.Nodes
{
  public class HotkeyNode
  {
    private static readonly IEnumerable<Keys> SelectableKeys = (IEnumerable<Keys>) ((IEnumerable<Keys>) Enum.GetValues<Keys>()).Except<Keys>((IEnumerable<Keys>) new Keys[2]
    {
      Keys.None,
      Keys.LButton
    }).ToList<Keys>();
    private bool _pressed;
    private bool _unPressed;
    [JsonIgnore]
    public Action OnValueChanged = (Action) (() => { });
    private Keys value;

    public HotkeyNode() => this.value = Keys.Space;

    public HotkeyNode(Keys value) => this.Value = value;

    public Keys Value
    {
      get => this.value;
      set
      {
        if (this.value == value)
          return;
        this.value = value;
        try
        {
          this.OnValueChanged();
        }
        catch
        {
          DebugWindow.LogMsg("Error in function that subscribed for: HotkeyNode.OnValueChanged", 10f, Color.Red);
        }
      }
    }

    public static implicit operator Keys(HotkeyNode node) => node.Value;

    public static implicit operator HotkeyNode(Keys value) => new HotkeyNode(value);

    public bool PressedOnce()
    {
      if (this.value == Keys.None)
        return false;
      if (Input.IsKeyDown(this.value))
      {
        if (this._pressed)
          return false;
        this._pressed = true;
        return true;
      }
      this._pressed = false;
      return false;
    }

    public bool UnpressedOnce()
    {
      if (this.value == Keys.None)
        return false;
      if (Input.GetKeyState(this.value))
        this._unPressed = true;
      else if (this._unPressed)
      {
        this._unPressed = false;
        return true;
      }
      return false;
    }

    public bool DrawPickerButton(string id)
    {
      if (ImGui.Button(id))
        ImGui.OpenPopup(id);
      bool flag = false;
      bool p_open = true;
      if (ImGui.BeginPopupModal(id, ref p_open, ImGuiWindowFlags.NoDecoration))
      {
        if (Input.GetKeyState(Keys.Escape))
        {
          ImGui.CloseCurrentPopup();
          ImGui.EndPopup();
          return false;
        }
        float y = ImGui.GetCursorPos().Y;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
        interpolatedStringHandler.AppendLiteral("Press new key to change '");
        interpolatedStringHandler.AppendFormatted<Keys>(this.Value);
        interpolatedStringHandler.AppendLiteral("' or Esc for exit.");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        float x = ImGui.CalcTextSize(stringAndClear).X;
        ImGui.Text(stringAndClear);
        if (ImGui.Button("Clear key"))
        {
          this.Value = Keys.None;
          flag = true;
          ImGui.CloseCurrentPopup();
        }
        float num = ImGui.GetCursorPos().Y - y;
        ImGui.SetWindowSize(new System.Numerics.Vector2(x + 10f, num + 10f));
        foreach (Keys selectableKey in HotkeyNode.SelectableKeys)
        {
          if (Input.GetKeyState(selectableKey))
          {
            this.Value = selectableKey;
            flag = true;
            ImGui.CloseCurrentPopup();
            break;
          }
        }
        ImGui.EndPopup();
      }
      return flag;
    }
  }
}
