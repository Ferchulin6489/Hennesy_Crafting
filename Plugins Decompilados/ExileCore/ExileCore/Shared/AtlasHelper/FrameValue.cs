// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.AtlasHelper.FrameValue
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;

namespace ExileCore.Shared.AtlasHelper
{
  public class FrameValue
  {
    [JsonProperty("frame")]
    public SpriteSourceSizeClass Frame { get; set; }

    [JsonProperty("rotated")]
    public bool Rotated { get; set; }

    [JsonProperty("trimmed")]
    public bool Trimmed { get; set; }

    [JsonProperty("spriteSourceSize")]
    public SpriteSourceSizeClass SpriteSourceSize { get; set; }

    [JsonProperty("sourceSize")]
    public Size SourceSize { get; set; }

    [JsonProperty("pivot")]
    public Pivot Pivot { get; set; }
  }
}
