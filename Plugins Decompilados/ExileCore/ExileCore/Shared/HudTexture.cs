// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.HudTexture
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;

namespace ExileCore.Shared
{
  public class HudTexture
  {
    public HudTexture()
    {
    }

    public HudTexture(string fileName) => this.FileName = fileName;

    public string FileName { get; set; }

    public RectangleF UV { get; set; } = new RectangleF(0.0f, 0.0f, 1f, 1f);

    public float Size { get; set; } = 13f;

    public Color Color { get; set; } = Color.White;
  }
}
