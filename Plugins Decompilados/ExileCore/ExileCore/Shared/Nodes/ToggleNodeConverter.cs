// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.ToggleNodeConverter
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ExileCore.Shared.Nodes
{
  public class ToggleNodeConverter : CustomCreationConverter<ToggleNode>
  {
    public override bool CanWrite => true;

    public override bool CanRead => true;

    public override ToggleNode Create(Type objectType) => new ToggleNode(false);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      try
      {
        return (object) new ToggleNode(serializer.Deserialize<bool>(reader));
      }
      catch
      {
        return (object) new ToggleNode(false);
      }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, (object) (bool) (!(value is ToggleNode toggleNode) ? 0 : (toggleNode.Value ? 1 : 0)));
  }
}
