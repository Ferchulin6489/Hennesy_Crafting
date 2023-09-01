// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.SpriteHelper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using SharpDX;

namespace ExileCore.Shared.Helpers
{
  public static class SpriteHelper
  {
    public static RectangleF GetUV(MyMapIconsIndex index) => SpriteHelper.GetUV((int) index, Constants.MyMapIcons);

    public static RectangleF GetUV(MapIconsIndex index) => SpriteHelper.GetUV((int) index, Constants.MapIconsSize);

    public static RectangleF GetUV(int index, Size2F size) => index % (int) size.Width == 0 ? new RectangleF((size.Width - 1f) / size.Width, (float) ((int) ((double) index / (double) size.Width) - 1) / size.Height, 1f / size.Width, 1f / size.Height) : new RectangleF((float) ((double) index % (double) size.Width - 1.0) / size.Width, (float) (index / (int) size.Width) / size.Height, 1f / size.Width, 1f / size.Height);

    public static RectangleF GetUV(Size2 index, Size2F size) => new RectangleF((float) (index.Width - 1) / size.Width, (float) (index.Height - 1) / size.Height, 1f / size.Width, 1f / size.Height);

    public static RectangleF GetUV(int x, int y, float width, float height) => new RectangleF((float) (x - 1) / width, (float) (y - 1) / height, 1f / width, 1f / height);
  }
}
