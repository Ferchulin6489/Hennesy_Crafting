// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.Extensions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using GameOffsets.Native;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ExileCore.Shared.Helpers
{
  public static class Extensions
  {
    private static readonly Color[] Colors;
    private static readonly Dictionary<string, MapIconsIndex> Icons;

    static Extensions()
    {
      FieldInfo[] fields = typeof (Color).GetFields(BindingFlags.Static | BindingFlags.Public);
      Extensions.Colors = new Color[fields.Length];
      Extensions.ColorName = new Dictionary<string, Color>(fields.Length);
      Extensions.ColorHex = new Dictionary<Color, string>(fields.Length);
      int index1;
      for (int index2 = 0; index2 < fields.Length; ++index2)
      {
        FieldInfo fieldInfo = fields[index2];
        Color color = (Color) fieldInfo.GetValue((object) typeof (Color));
        Extensions.ColorName[fieldInfo.Name] = color;
        Extensions.ColorName[fieldInfo.Name.ToLower()] = color;
        Dictionary<Color, string> colorHex = Extensions.ColorHex;
        Color key = color;
        index1 = color.ToRgba();
        string str = index1.ToString("X");
        colorHex[key] = str;
        if (color != Color.Transparent)
          Extensions.Colors[index2] = color;
      }
      Extensions.Icons = new Dictionary<string, MapIconsIndex>(200);
      MapIconsIndex[] values = Enum.GetValues<MapIconsIndex>();
      for (index1 = 0; index1 < values.Length; ++index1)
      {
        MapIconsIndex mapIconsIndex = values[index1];
        Extensions.Icons[mapIconsIndex.ToString()] = mapIconsIndex;
      }
    }

    private static Dictionary<string, Color> ColorName { get; } = new Dictionary<string, Color>();

    private static Dictionary<Color, string> ColorHex { get; } = new Dictionary<Color, string>();

    public static Color GetRandomColor(this Color c) => Extensions.Colors[Random.Shared.Next(0, Extensions.Colors.Length - 1)];

    public static MapIconsIndex IconIndexByName(string name)
    {
      MapIconsIndex mapIconsIndex;
      Extensions.Icons.TryGetValue(name, out mapIconsIndex);
      return mapIconsIndex;
    }

    public static Color GetColorByName(string name)
    {
      Color color;
      return !Extensions.ColorName.TryGetValue(name, out color) ? Color.Zero : color;
    }

    public static string Hex(this Color clr)
    {
      string str;
      return !Extensions.ColorHex.TryGetValue(clr, out str) ? Extensions.ColorHex[Color.Transparent] : str;
    }

    public static uint ToImgui(this Color c) => (uint) c.ToRgba();

    public static System.Numerics.Vector4 ToImguiVec4(this Color c) => new System.Numerics.Vector4((float) c.R / (float) byte.MaxValue, (float) c.G / (float) byte.MaxValue, (float) c.B / (float) byte.MaxValue, (float) c.A / (float) byte.MaxValue);

    public static System.Numerics.Vector4 ToImguiVec4(this Color c, byte alpha) => new System.Numerics.Vector4((float) c.R, (float) c.G, (float) c.B, (float) alpha);

    public static System.Numerics.Vector4 ToVector4Num(this SharpDX.Vector4 v) => new System.Numerics.Vector4(v.X, v.Y, v.Z, v.W);

    public static System.Numerics.Vector3 ToVector3Num(this SharpDX.Vector3 v) => new System.Numerics.Vector3(v.X, v.Y, v.Z);

    public static System.Numerics.Vector2 ToVector2Num(this SharpDX.Vector2 v) => new System.Numerics.Vector2(v.X, v.Y);

    public static System.Numerics.Vector2 ToVector2(this Point point) => new System.Numerics.Vector2((float) point.X, (float) point.Y);

    public static SharpDX.Vector2 ToSharpDx(this System.Numerics.Vector2 v) => new SharpDX.Vector2(v.X, v.Y);

    public static SharpDX.Vector3 ToSharpDx(this System.Numerics.Vector3 v) => new SharpDX.Vector3(v.X, v.Y, v.Z);

    public static SharpDX.Vector4 ToSharpDx(this System.Numerics.Vector4 v) => new SharpDX.Vector4(v.X, v.Y, v.Z, v.W);

    public static System.Numerics.Vector2 Xy(this System.Numerics.Vector3 v) => new System.Numerics.Vector2(v.X, v.Y);

    public static System.Numerics.Vector2 Xy(this System.Numerics.Vector4 v) => new System.Numerics.Vector2(v.X, v.Y);

    public static System.Numerics.Vector3 Xyz(this System.Numerics.Vector4 v) => new System.Numerics.Vector3(v.X, v.Y, v.Z);

    public static Vector2i RoundToVector2I(this System.Numerics.Vector2 v) => new Vector2i((int) MathF.Round(v.X), (int) MathF.Round(v.Y));

    public static bool Contains(this RectangleF rectangle, System.Numerics.Vector2 v) => rectangle.Contains(v.ToSharpDx());

    public static Color ToSharpColor(this System.Numerics.Vector4 v) => new Color(v.X, v.Y, v.Z, v.W);

    public static uint ToImguiBase255(this System.Numerics.Vector4 v) => (uint) ((int) v.X | (int) v.Y << 8 | (int) v.Z << 16 | (int) v.W << 24);

    public static uint ToImguiBase1(this System.Numerics.Vector4 v) => (v * (float) byte.MaxValue).ToImguiBase255();

    public static int GetOffset<T>(Expression<Func<T, object>> selectExpression) where T : struct
    {
      try
      {
        return ((MemberExpression) ((UnaryExpression) selectExpression.Body).Operand).Member.GetCustomAttribute<FieldOffsetAttribute>().Value;
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
        throw;
      }
    }

    public static ExileCore.Shared.Cache.ValidCache<T> ValidCache<T>(
      this Entity entity,
      Func<T> func)
    {
      return new ExileCore.Shared.Cache.ValidCache<T>(entity, func);
    }

    public static uint HexToUInt(this ReadOnlySpan<char> span)
    {
      uint num1 = 0;
      for (int index = 0; index < span.Length; ++index)
      {
        char ch = span[index];
        if (num1 > 268435455U)
          return num1;
        num1 *= 16U;
        if (ch != char.MinValue)
        {
          uint num2 = num1;
          uint num3 = ch < '0' || ch > '9' ? (ch < 'A' || ch > 'F' ? num2 + (uint) ((int) ch - 97 + 10) : num2 + (uint) ((int) ch - 65 + 10)) : num2 + ((uint) ch - 48U);
          if (num3 < num1)
            return num1;
          num1 = num3;
        }
      }
      return num1;
    }
  }
}
