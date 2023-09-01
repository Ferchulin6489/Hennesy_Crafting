// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.TextNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using SharpDX;
using System;

namespace ExileCore.Shared.Nodes
{
  public class TextNode
  {
    [JsonIgnore]
    public Action OnValueChanged = (Action) (() => { });
    private string value;

    public TextNode()
    {
    }

    public TextNode(string value) => this.Value = value;

    public string Value
    {
      get => this.value;
      set
      {
        if (!(this.value != value))
          return;
        this.value = value;
        try
        {
          this.OnValueChanged();
        }
        catch (Exception ex)
        {
          DebugWindow.LogMsg("Error in function that subscribed for: TextNode.OnValueChanged", 10f, Color.Red);
        }
      }
    }

    public void SetValueNoEvent(string newValue) => this.value = newValue;

    public static implicit operator string(TextNode node) => node.Value;

    public static implicit operator TextNode(string value) => new TextNode(value);
  }
}
