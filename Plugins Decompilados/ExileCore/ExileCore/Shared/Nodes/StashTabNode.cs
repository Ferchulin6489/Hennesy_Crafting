// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.StashTabNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using Newtonsoft.Json;

namespace ExileCore.Shared.Nodes
{
  public class StashTabNode
  {
    public const string EMPTYNAME = "-NoName-";

    public StashTabNode()
    {
    }

    public StashTabNode(string name, int visibleIndex, int id, InventoryTabFlags flag)
    {
      this.Name = name;
      this.VisibleIndex = visibleIndex;
      this.Id = id;
      this.IsRemoveOnly = (flag & InventoryTabFlags.RemoveOnly) == InventoryTabFlags.RemoveOnly;
    }

    public string Name { get; set; } = "-NoName-";

    public int VisibleIndex { get; set; } = -1;

    [JsonIgnore]
    public bool Exist { get; set; }

    [JsonIgnore]
    internal int Id { get; set; } = -1;

    [JsonIgnore]
    public bool IsRemoveOnly { get; set; }
  }
}
