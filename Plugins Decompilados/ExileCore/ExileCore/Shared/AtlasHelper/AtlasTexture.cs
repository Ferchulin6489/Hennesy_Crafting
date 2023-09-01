// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.AtlasHelper.AtlasTexture
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System.IO;

namespace ExileCore.Shared.AtlasHelper
{
  public class AtlasTexture
  {
    internal AtlasTexture(string textureName, RectangleF textureUv, string atlasFilePath)
    {
      this.TextureName = textureName;
      this.TextureUV = textureUv;
      this.AtlasFilePath = atlasFilePath;
      this.AtlasFileName = Path.GetFileName(atlasFilePath);
    }

    public string TextureName { get; }

    public string AtlasFilePath { get; }

    public string AtlasFileName { get; }

    public RectangleF TextureUV { get; }
  }
}
