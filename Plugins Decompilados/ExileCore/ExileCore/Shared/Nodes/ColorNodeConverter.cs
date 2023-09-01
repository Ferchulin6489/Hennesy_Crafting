// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.ColorNodeConverter
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared.Nodes
{
  public class ColorNodeConverter : CustomCreationConverter<ColorNode>
  {
    public override bool CanWrite => true;

    public override bool CanRead => true;

    public override ColorNode Create(Type objectType) => new ColorNode();

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      uint result;
      return !uint.TryParse(reader.Value.ToString(), NumberStyles.HexNumber, (IFormatProvider) null, out result) ? (object) this.Create(objectType) : (object) new ColorNode(result);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      ColorNode colorNode = (ColorNode) value;
      JsonSerializer jsonSerializer = serializer;
      JsonWriter jsonWriter = writer;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<int>(colorNode.Value.ToAbgr(), "x8");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      jsonSerializer.Serialize(jsonWriter, (object) stringAndClear);
    }
  }
}
