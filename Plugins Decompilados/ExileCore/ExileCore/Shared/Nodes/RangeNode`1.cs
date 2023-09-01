// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.RangeNode`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using SharpDX;
using System;

namespace ExileCore.Shared.Nodes
{
  public sealed class RangeNode<T> where T : struct
  {
    private T _value;

    public RangeNode()
    {
    }

    public RangeNode(T value, T min, T max)
    {
      this.Value = value;
      this.Min = min;
      this.Max = max;
    }

    public T Value
    {
      get => this._value;
      set
      {
        if (value.Equals((object) this._value))
          return;
        this._value = value;
        try
        {
          EventHandler<T> onValueChanged = this.OnValueChanged;
          if (onValueChanged == null)
            return;
          onValueChanged((object) this, value);
        }
        catch (Exception ex)
        {
          DebugWindow.LogMsg("Error in function that subscribed for: RangeNode.OnValueChanged", 10f, Color.Red);
        }
      }
    }

    [JsonIgnore]
    public T Min { get; set; }

    [JsonIgnore]
    public T Max { get; set; }

    public event EventHandler<T> OnValueChanged;

    public static implicit operator T(RangeNode<T> node) => node.Value;
  }
}
