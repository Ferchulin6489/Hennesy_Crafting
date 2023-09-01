// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.SortContractResolver
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExileCore.Shared.Nodes
{
  public sealed class SortContractResolver : DefaultContractResolver
  {
    private const int MAX_PROPERTIES_PER_CONTRACT = 1000;

    protected override IList<JsonProperty> CreateProperties(
      Type type,
      MemberSerialization memberSerialization)
    {
      return (IList<JsonProperty>) (this.GetSerializableMembers(type) ?? throw new JsonSerializationException("Null collection of serializable members returned.")).Select<MemberInfo, JsonProperty>((Func<MemberInfo, JsonProperty>) (member => this.CreateProperty(member, memberSerialization))).Where<JsonProperty>((Func<JsonProperty, bool>) (x => x != null)).OrderBy<JsonProperty, int>((Func<JsonProperty, int>) (x => 1000 * SortContractResolver.GetTypeDepth(x.DeclaringType) + x.Order.GetValueOrDefault())).ToList<JsonProperty>();
    }

    private static int GetTypeDepth(Type type)
    {
      int typeDepth = 0;
      while ((type = type.BaseType) != (Type) null)
        ++typeDepth;
      return typeDepth;
    }
  }
}
