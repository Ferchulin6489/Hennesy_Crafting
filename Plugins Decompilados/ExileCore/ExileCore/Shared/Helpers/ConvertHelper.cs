// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.ConvertHelper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Nodes;
using SharpDX;
using System;
using System.Drawing;
using System.Globalization;

namespace ExileCore.Shared.Helpers
{
  public static class ConvertHelper
  {
    public static string ToShorten(double value, string format = "0")
    {
      double num = Math.Abs(value);
      if (num >= 1000000.0)
        return (value / 1000000.0).ToString("F2") + "M";
      return num >= 1000.0 ? (value / 1000.0).ToString("F1") + "K" : value.ToString(format);
    }

    public static SharpDX.Color ToBGRAColor(this string value)
    {
      uint result;
      return !uint.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result) ? SharpDX.Color.Black : SharpDX.Color.FromBgra(result);
    }

    public static SharpDX.Color? ConfigColorValueExtractor(this string[] line, int index) => !ConvertHelper.IsNotNull(line, index) ? new SharpDX.Color?() : new SharpDX.Color?(line[index].ToBGRAColor());

    public static string ConfigValueExtractor(this string[] line, int index) => !ConvertHelper.IsNotNull(line, index) ? (string) null : line[index];

    private static bool IsNotNull(string[] line, int index) => line.Length > index && !string.IsNullOrEmpty(line[index]);

    public static System.Numerics.Vector3 ColorNodeToVector3(this ColorNode color)
    {
      SharpDX.Vector3 vector3 = color.Value.ToVector3();
      return new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
    }

    public static System.Numerics.Vector2 TranslateToNum(this SharpDX.Vector2 vector, float dx = 0.0f, float dy = 0.0f) => new System.Numerics.Vector2(vector.X + dx, vector.Y + dy);

    public static System.Numerics.Vector4 TranslateToNum(
      this SharpDX.Vector4 vector,
      float dx = 0.0f,
      float dy = 0.0f,
      float dz = 0.0f,
      float dw = 0.0f)
    {
      return new System.Numerics.Vector4(vector.X + dx, vector.Y + dy, vector.Z + dz, vector.W + dw);
    }

    public static System.Numerics.Vector3 TranslateToNum(
      this System.Numerics.Vector3 vector,
      float dx = 0.0f,
      float dy = 0.0f,
      float dz = 0.0f)
    {
      return new System.Numerics.Vector3(vector.X + dx, vector.Y + dy, vector.Z + dz);
    }

    public static string ToHex(this SharpDX.Color value) => ColorTranslator.ToHtml(System.Drawing.Color.FromArgb((int) value.A, (int) value.R, (int) value.G, (int) value.B));

    public static SharpDX.Color ColorFromHsv(double hue, double saturation, double value)
    {
      int num1 = Convert.ToInt32(Math.Floor(hue / 60.0)) % 6;
      double num2 = hue / 60.0 - Math.Floor(hue / 60.0);
      value *= (double) byte.MaxValue;
      byte num3 = Convert.ToByte(value);
      byte num4 = Convert.ToByte(value * (1.0 - saturation));
      byte num5 = Convert.ToByte(value * (1.0 - num2 * saturation));
      byte num6 = Convert.ToByte(value * (1.0 - (1.0 - num2) * saturation));
      ColorBGRA colorBgra;
      switch (num1)
      {
        case 0:
          colorBgra = new ColorBGRA(num3, num6, num4, byte.MaxValue);
          break;
        case 1:
          colorBgra = new ColorBGRA(num5, num3, num4, byte.MaxValue);
          break;
        case 2:
          colorBgra = new ColorBGRA(num4, num3, num6, byte.MaxValue);
          break;
        case 3:
          colorBgra = new ColorBGRA(num4, num5, num3, byte.MaxValue);
          break;
        case 4:
          colorBgra = new ColorBGRA(num6, num4, num3, byte.MaxValue);
          break;
        default:
          colorBgra = new ColorBGRA(num3, num4, num5, byte.MaxValue);
          break;
      }
      return (SharpDX.Color) colorBgra;
    }
  }
}
