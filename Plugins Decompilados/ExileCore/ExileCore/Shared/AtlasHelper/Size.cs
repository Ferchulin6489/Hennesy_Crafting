// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.AtlasHelper.Size
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;

namespace ExileCore.Shared.AtlasHelper
{
  public class Size
  {
    [JsonProperty("w")]
    public int W { get; set; }

    [JsonProperty("h")]
    public int H { get; set; }
  }
}
