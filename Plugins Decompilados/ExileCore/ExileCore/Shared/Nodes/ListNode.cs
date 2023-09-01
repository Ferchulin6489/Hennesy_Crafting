// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.ListNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using SharpDX;
using System;
using System.Collections.Generic;

namespace ExileCore.Shared.Nodes
{
  public class ListNode
  {
    [JsonIgnore]
    public Action<string> OnValueSelected = (Action<string>) (_param1 => { });
    [JsonIgnore]
    public Action<string> OnValueSelectedPre = (Action<string>) (_param1 => { });
    private string value;
    [JsonIgnore]
    public List<string> Values = new List<string>();

    public string Value
    {
      get => this.value;
      set
      {
        if (!(this.value != value))
          return;
        try
        {
          this.OnValueSelectedPre(value);
        }
        catch
        {
          DebugWindow.LogMsg("Error in function that subscribed for: ListNode.OnValueSelectedPre", 10f, Color.Red);
        }
        this.value = value;
        try
        {
          this.OnValueSelected(value);
        }
        catch (Exception ex)
        {
          DebugWindow.LogMsg("Error in function that subscribed for: ListNode.OnValueSelected. Error: " + ex.Message, 10f, Color.Red);
        }
      }
    }

    public static implicit operator string(ListNode node) => node.Value;

    public void SetListValues(List<string> values) => this.Values = values;
  }
}
