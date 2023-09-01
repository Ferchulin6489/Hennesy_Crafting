// Decompiled with JetBrains decompiler
// Type: ExileCore.Graphics
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.RenderQ;
using ExileCore.Shared.AtlasHelper;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ImGuiNET;
using SharpDX;
using System;


#nullable enable
namespace ExileCore
{
  public class Graphics
  {
    private static readonly RectangleF DefaultUV = new RectangleF(0.0f, 0.0f, 1f, 1f);
    private readonly 
    #nullable disable
    CoreSettings _settings;
    private readonly ImGuiRender ImGuiRender;
    private readonly DX11 _lowLevel;

    public Graphics(DX11 dx11, CoreSettings settings)
    {
      this._lowLevel = dx11;
      this._settings = settings;
      this.ImGuiRender = dx11.ImGuiRender;
    }

    [Obsolete]
    public DX11 LowLevel => this._lowLevel;

    public FontContainer Font => this.ImGuiRender.CurrentFont;

    public FontContainer LastFont => this.ImGuiRender.CurrentFont;

    public System.Numerics.Vector2 DrawText(
      string text,
      System.Numerics.Vector2 position,
      Color color,
      int height)
    {
      return this.DrawText(text, position, color, height, (string) this._settings.Font);
    }

    public System.Numerics.Vector2 DrawText(
      string text,
      System.Numerics.Vector2 position,
      Color color,
      FontAlign align)
    {
      return this.DrawText(text, position, color, (int) this._settings.FontSize, (string) this._settings.Font, align);
    }

    public System.Numerics.Vector2 DrawText(
      string text,
      System.Numerics.Vector2 position,
      Color color,
      string fontName,
      FontAlign align)
    {
      return this.ImGuiRender.DrawText(text, position, color, -1, fontName, align);
    }

    public System.Numerics.Vector2 DrawText(
      string text,
      System.Numerics.Vector2 position,
      Color color,
      int height,
      FontAlign align)
    {
      return this.ImGuiRender.DrawText(text, position, color, height, (string) this._settings.Font, align);
    }

    public System.Numerics.Vector2 DrawText(
      string text,
      System.Numerics.Vector2 position,
      Color color,
      int height,
      string fontName,
      FontAlign align = FontAlign.Left)
    {
      return this.ImGuiRender.DrawText(text, position, color, height, fontName, align);
    }

    public System.Numerics.Vector2 DrawText(string text, System.Numerics.Vector2 position, Color color) => this.DrawText(text, position, color, (int) this._settings.FontSize, (string) this._settings.Font);

    public System.Numerics.Vector2 DrawText(string text, System.Numerics.Vector2 position) => this.DrawText(text, position, Color.White);

    public System.Numerics.Vector2 DrawText(string text, System.Numerics.Vector2 position, FontAlign align) => this.DrawText(text, position, Color.White, (int) this._settings.FontSize, align);

    public IDisposable SetTextScale(float textScale)
    {
      float textScale1 = this.ImGuiRender.TextScale;
      this.ImGuiRender.TextScale *= textScale;
      return (IDisposable) new Graphics.SetTextScaleDisposable(this.ImGuiRender, textScale1);
    }

    public System.Numerics.Vector2 MeasureText(string text) => this.ImGuiRender.MeasureText(text);

    public System.Numerics.Vector2 MeasureText(string text, int height) => this.ImGuiRender.MeasureText(text, height);

    public void DrawLine(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, float borderWidth, Color color) => this.ImGuiRender.LowLevelApi.AddLine(p1, p2, color.ToImgui(), borderWidth);

    public void DrawFrame(
      System.Numerics.Vector2 p1,
      System.Numerics.Vector2 p2,
      Color color,
      float rounding,
      int thickness,
      int flags)
    {
      this.ImGuiRender.LowLevelApi.AddRect(p1, p2, color.ToImgui(), rounding, (ImDrawFlags) flags, (float) thickness);
    }

    public void DrawBox(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, Color color, float rounding = 0.0f) => this.ImGuiRender.LowLevelApi.AddRectFilled(p1, p2, color.ToImgui(), rounding);

    public void DrawQuad(IntPtr textureId, System.Numerics.Vector2 a, System.Numerics.Vector2 b, System.Numerics.Vector2 c, System.Numerics.Vector2 d) => this.ImGuiRender.LowLevelApi.AddImageQuad(textureId, a, b, c, d);

    public void DrawQuad(
      IntPtr textureId,
      System.Numerics.Vector2 a,
      System.Numerics.Vector2 b,
      System.Numerics.Vector2 c,
      System.Numerics.Vector2 d,
      Color color)
    {
      System.Numerics.Vector2 uv1 = new System.Numerics.Vector2();
      System.Numerics.Vector2 uv2 = new System.Numerics.Vector2(1f, 0.0f);
      System.Numerics.Vector2 uv3 = new System.Numerics.Vector2(1f, 1f);
      System.Numerics.Vector2 uv4 = new System.Numerics.Vector2(0.0f, 1f);
      this.ImGuiRender.LowLevelApi.AddImageQuad(textureId, a, b, c, d, uv1, uv2, uv3, uv4, color.ToImgui());
    }

    public void DrawImage(string fileName, RectangleF rectangle) => this.DrawImage(fileName, rectangle, Graphics.DefaultUV, Color.White);

    public void DrawImage(string fileName, RectangleF rectangle, Color color) => this.DrawImage(fileName, rectangle, Graphics.DefaultUV, color);

    public void DrawImage(string fileName, RectangleF rectangle, RectangleF uv, Color color)
    {
      try
      {
        this.ImGuiRender.DrawImage(fileName, rectangle, uv, color);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }

    public void DrawImage(string fileName, RectangleF rectangle, RectangleF uv) => this.DrawImage(fileName, rectangle, uv, Color.White);

    public void DrawImage(AtlasTexture atlasTexture, RectangleF rectangle) => this.DrawImage(atlasTexture, rectangle, Color.White);

    public void DrawImage(AtlasTexture atlasTexture, RectangleF rectangle, Color color) => this.DrawImage(atlasTexture.AtlasFileName, rectangle, atlasTexture.TextureUV, color);

    public void DrawImageGui(string fileName, RectangleF rectangle, RectangleF uv) => this.ImGuiRender.DrawImage(fileName, rectangle, uv);

    public void DrawImageGui(
      string fileName,
      System.Numerics.Vector2 TopLeft,
      System.Numerics.Vector2 BottomRight,
      System.Numerics.Vector2 TopLeft_UV,
      System.Numerics.Vector2 BottomRight_UV)
    {
      this.ImGuiRender.DrawImage(fileName, TopLeft, BottomRight, TopLeft_UV, BottomRight_UV);
    }

    public void DrawBox(RectangleF rect, Color color) => this.DrawBox(rect, color, 0.0f);

    public void DrawBox(RectangleF rect, Color color, float rounding) => this.DrawBox(rect.TopLeft.ToVector2Num(), rect.BottomRight.ToVector2Num(), color, rounding);

    public void DrawFrame(RectangleF rect, Color color, float rounding, int thickness, int flags) => this.DrawFrame(rect.TopLeft.ToVector2Num(), rect.BottomRight.ToVector2Num(), color, rounding, thickness, flags);

    public void DrawFrame(RectangleF rect, Color color, int thickness) => this.DrawFrame(rect.TopLeft.ToVector2Num(), rect.BottomRight.ToVector2Num(), color, 0.0f, thickness, 0);

    public void DrawFrame(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, Color color, int thickness) => this.DrawFrame(p1, p2, color, 0.0f, thickness, 0);

    public bool InitImage(string name, bool textures = true) => this.LowLevel.InitTexture(textures ? "textures/" + name : name);

    public IntPtr GetTextureId(string name) => this.LowLevel.GetTexture(name);

    public void DisposeTexture(string name) => this.LowLevel.DisposeTexture(name);

    public IDisposable UseCurrentFont() => this.ImGuiRender.UseCurrentFont();

    [Obsolete]
    public System.Numerics.Vector2 DrawText(
      string text,
      SharpDX.Vector2 position,
      Color color,
      string fontName = null,
      FontAlign align = FontAlign.Left)
    {
      return this.ImGuiRender.DrawText(text, position.ToVector2Num(), color, -1, fontName, align);
    }

    [Obsolete]
    public System.Numerics.Vector2 DrawText(string text, SharpDX.Vector2 position, Color color) => this.DrawText(text, position.ToVector2Num(), color, (int) this._settings.FontSize, (string) this._settings.Font);

    [Obsolete]
    public System.Numerics.Vector2 DrawText(
      string text,
      SharpDX.Vector2 position,
      Color color,
      int height)
    {
      return this.DrawText(text, position.ToVector2Num(), color, height, (string) this._settings.Font);
    }

    [Obsolete]
    public System.Numerics.Vector2 DrawText(
      string text,
      SharpDX.Vector2 position,
      Color color,
      FontAlign align = FontAlign.Left)
    {
      return this.DrawText(text, position.ToVector2Num(), color, (int) this._settings.FontSize, (string) this._settings.Font, align);
    }

    [Obsolete]
    public System.Numerics.Vector2 DrawText(
      string text,
      SharpDX.Vector2 position,
      Color color,
      int height,
      FontAlign align = FontAlign.Left)
    {
      return this.DrawText(text, position.ToVector2Num(), color, height, (string) this._settings.Font, align);
    }

    [Obsolete]
    public System.Numerics.Vector2 DrawText(string text, SharpDX.Vector2 position, FontAlign align = FontAlign.Left) => this.DrawText(text, position.ToVector2Num(), Color.White, (int) this._settings.FontSize, align);

    [Obsolete]
    public void DrawLine(SharpDX.Vector2 p1, SharpDX.Vector2 p2, float borderWidth, Color color) => this.ImGuiRender.LowLevelApi.AddLine(p1.ToVector2Num(), p2.ToVector2Num(), color.ToImgui(), borderWidth);

    [Obsolete]
    public void DrawBox(SharpDX.Vector2 p1, SharpDX.Vector2 p2, Color color, float rounding = 0.0f) => this.ImGuiRender.LowLevelApi.AddRectFilled(p1.ToVector2Num(), p2.ToVector2Num(), color.ToImgui(), rounding);

    [Obsolete]
    public void DrawTexture(IntPtr user_texture_id, SharpDX.Vector2 a, SharpDX.Vector2 b, SharpDX.Vector2 c, SharpDX.Vector2 d) => this.ImGuiRender.LowLevelApi.AddImageQuad(user_texture_id, a.ToVector2Num(), b.ToVector2Num(), c.ToVector2Num(), d.ToVector2Num());

    [Obsolete]
    public void DrawFrame(SharpDX.Vector2 p1, SharpDX.Vector2 p2, Color color, int thickness) => this.DrawFrame(p1.ToVector2Num(), p2.ToVector2Num(), color, 0.0f, thickness, 0);

    private record SetTextScaleDisposable(ImGuiRender Render, float OldScale) : IDisposable
    {
      public void Dispose() => this.Render.TextScale = this.OldScale;
    }
  }
}
