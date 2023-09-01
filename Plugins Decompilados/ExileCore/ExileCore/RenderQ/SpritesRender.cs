// Decompiled with JetBrains decompiler
// Type: ExileCore.RenderQ.SpritesRender
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System;

namespace ExileCore.RenderQ
{
  public class SpritesRender
  {
    private readonly DX11 _dx11;
    private readonly ImGuiRender _imGuiRender;

    public SpritesRender(DX11 dx11, ImGuiRender imGuiRender)
    {
      this._dx11 = dx11;
      this._imGuiRender = imGuiRender;
    }

    [Obsolete]
    public bool LoadImage(string fileName) => this._dx11.InitTexture(fileName);

    [Obsolete]
    public void DrawImage(string fileName, RectangleF rect, RectangleF uv, Color color)
    {
      try
      {
        this._imGuiRender.DrawImage(fileName, rect, uv, color);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }
  }
}
