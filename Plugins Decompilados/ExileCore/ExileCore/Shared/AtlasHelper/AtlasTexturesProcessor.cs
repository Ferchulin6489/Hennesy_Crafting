// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.AtlasHelper.AtlasTexturesProcessor
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Newtonsoft.Json;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared.AtlasHelper
{
  public sealed class AtlasTexturesProcessor
  {
    private readonly Dictionary<string, AtlasTexture> _atlasTextures = new Dictionary<string, AtlasTexture>();
    private static readonly AtlasTexture MISSING_TEXTURE = new AtlasTexture("missing_texture.png", new RectangleF(0.0f, 0.0f, 1f, 1f), "missing_texture.png");
    private readonly string _atlasPath;

    public AtlasTexturesProcessor(string atlasPath) => this._atlasPath = atlasPath;

    public AtlasTexturesProcessor(string configPath, string atlasPath)
    {
      this._atlasPath = atlasPath;
      this.LoadConfig(configPath, atlasPath);
    }

    private void LoadConfig(string configPath, string atlasPath)
    {
      this._atlasTextures.Clear();
      AtlasConfigData atlasConfigData = JsonConvert.DeserializeObject<AtlasConfigData>(File.ReadAllText(configPath));
      System.Numerics.Vector2 vector2 = new System.Numerics.Vector2((float) atlasConfigData.Meta.Size.W, (float) atlasConfigData.Meta.Size.H);
      foreach (KeyValuePair<string, FrameValue> frame in atlasConfigData.Frames)
      {
        string str = frame.Key.Replace(".png", string.Empty);
        if (string.IsNullOrEmpty(str))
          DebugWindow.LogError("Sprite '" + Path.GetFileNameWithoutExtension(configPath) + "' contain a texture with empty/null name.", 20f);
        else if (this._atlasTextures.ContainsKey(str))
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
          interpolatedStringHandler.AppendLiteral("Sprite '");
          interpolatedStringHandler.AppendFormatted(Path.GetFileNameWithoutExtension(configPath));
          interpolatedStringHandler.AppendLiteral("' already have a texture with name ");
          interpolatedStringHandler.AppendFormatted(str);
          interpolatedStringHandler.AppendLiteral(". Duplicates is not allowed!");
          DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 20f);
        }
        else
        {
          RectangleF textureUv = new RectangleF((float) frame.Value.Frame.X / vector2.X, (float) frame.Value.Frame.Y / vector2.Y, (float) frame.Value.Frame.W / vector2.X, (float) frame.Value.Frame.H / vector2.Y);
          AtlasTexture atlasTexture = new AtlasTexture(str, textureUv, atlasPath);
          this._atlasTextures.Add(str, atlasTexture);
        }
      }
    }

    public AtlasTexture GetTextureByName(string textureName)
    {
      AtlasTexture missingTexture;
      if (!this._atlasTextures.TryGetValue(textureName.Replace(".png", string.Empty), out missingTexture))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
        interpolatedStringHandler.AppendLiteral("Texture with name'");
        interpolatedStringHandler.AppendFormatted(textureName);
        interpolatedStringHandler.AppendLiteral("' is not found in texture atlas ");
        interpolatedStringHandler.AppendFormatted(this._atlasPath);
        interpolatedStringHandler.AppendLiteral(".");
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 20f);
        missingTexture = AtlasTexturesProcessor.MISSING_TEXTURE;
      }
      return missingTexture;
    }
  }
}
