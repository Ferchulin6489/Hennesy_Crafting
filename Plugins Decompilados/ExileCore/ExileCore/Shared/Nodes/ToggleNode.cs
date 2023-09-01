// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.ToggleNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared.Nodes
{
  public sealed class ToggleNode
  {
    [JsonIgnore]
    private bool value;

    public ToggleNode()
    {
    }

    public ToggleNode(bool value) => this.Value = value;

    public bool Value
    {
      get => this.value;
      set
      {
        if (this.value == value)
          return;
        this.value = value;
        try
        {
          EventHandler<bool> onValueChanged = this.OnValueChanged;
          if (onValueChanged == null)
            return;
          onValueChanged((object) this, value);
        }
        catch (Exception ex)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 2);
          interpolatedStringHandler.AppendLiteral("Error in function that subscribed for: ToggleNode.OnValueChanged. ");
          interpolatedStringHandler.AppendFormatted(Environment.NewLine);
          interpolatedStringHandler.AppendLiteral(" ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 10f);
        }
      }
    }

    public event EventHandler<bool> OnValueChanged;

    public void SetValueNoEvent(bool newValue) => this.value = newValue;

    public static implicit operator bool(ToggleNode node) => node.Value;
  }
}
