// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.AtlasHelper.Meta
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using System;

namespace ExileCore.Shared.AtlasHelper
{
  public class Meta
  {
    [JsonProperty("app")]
    public Uri App { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("format")]
    public string Format { get; set; }

    [JsonProperty("size")]
    public Size Size { get; set; }

    [JsonProperty("scale")]
    public long Scale { get; set; }
  }
}
