// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.FileNodeConverter
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ExileCore.Shared.Nodes
{
  public class FileNodeConverter : CustomCreationConverter<FileNode>
  {
    public override bool CanWrite => true;

    public override bool CanRead => true;

    public override FileNode Create(Type objectType) => (FileNode) string.Empty;

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      return (object) new FileNode(serializer.Deserialize<string>(reader));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (!(value is FileNode fileNode))
        return;
      serializer.Serialize(writer, (object) fileNode.Value);
    }
  }
}
