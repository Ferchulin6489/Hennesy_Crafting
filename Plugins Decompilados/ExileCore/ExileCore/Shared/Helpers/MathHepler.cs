// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.MathHepler
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.Shared.Helpers
{
  public static class MathHepler
  {
    private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static System.Numerics.Vector2 Rotate(this System.Numerics.Vector2 v, float angleDegrees)
    {
      double radians = MathHepler.ConvertToRadians((double) angleDegrees);
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      return new System.Numerics.Vector2((float) ((double) v.X * num1 - (double) v.Y * num2), (float) ((double) v.X * num2 + (double) v.Y * num1));
    }

    public static double ConvertToRadians(double angle) => Math.PI / 180.0 * angle;

    public static double GetPolarCoordinates(this System.Numerics.Vector2 vector, out double phi)
    {
      double polarCoordinates = (double) vector.Length();
      phi = Math.Acos((double) vector.X / polarCoordinates);
      if ((double) vector.Y < 0.0)
        phi = 2.0 * Math.PI - phi;
      return polarCoordinates;
    }

    public static string GetRandomWord(int length)
    {
      char[] chArray = new char[length];
      for (int index = 0; index < length; ++index)
        chArray[index] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[Random.Shared.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)];
      return new string(chArray);
    }

    public static float Max(params float[] values)
    {
      float val1 = ((IEnumerable<float>) values).First<float>();
      for (int index = 1; index < values.Length; ++index)
        val1 = Math.Max(val1, values[index]);
      return val1;
    }

    public static System.Numerics.Vector2 Translate(this System.Numerics.Vector2 vector, float dx = 0.0f, float dy = 0.0f) => new System.Numerics.Vector2(vector.X + dx, vector.Y + dy);

    public static System.Numerics.Vector2 Translate(this System.Numerics.Vector2 vector, System.Numerics.Vector2 vector2) => new System.Numerics.Vector2(vector.X + vector2.X, vector.Y + vector2.Y);

    public static System.Numerics.Vector2 Mult(this System.Numerics.Vector2 vector, float dx = 1f, float dy = 1f) => new System.Numerics.Vector2(vector.X * dx, vector.Y * dy);

    public static System.Numerics.Vector3 Translate(
      this System.Numerics.Vector3 vector,
      float dx,
      float dy,
      float dz)
    {
      return new System.Numerics.Vector3(vector.X + dx, vector.Y + dy, vector.Z + dz);
    }

    public static float Distance(this System.Numerics.Vector2 a, System.Numerics.Vector2 b) => System.Numerics.Vector2.Distance(a, b);

    public static float DistanceSquared(this System.Numerics.Vector2 a, System.Numerics.Vector2 b) => System.Numerics.Vector2.DistanceSquared(a, b);

    public static bool IsInRectangle(this System.Numerics.Vector2 point, System.Drawing.RectangleF rect) => (double) point.X >= (double) rect.X && (double) point.Y >= (double) rect.Y && (double) point.X <= (double) rect.Width && (double) point.Y <= (double) rect.Height;

    public static SharpDX.RectangleF GetDirectionsUV(double phi, double distance)
    {
      phi += Math.PI / 4.0;
      if (phi > 2.0 * Math.PI)
        phi -= 2.0 * Math.PI;
      float num1 = (float) Math.Round(phi / Math.PI * 4.0);
      if ((double) num1 >= 8.0)
        num1 = 0.0f;
      float num2 = distance > 60.0 ? (distance > 120.0 ? 2f : 1f) : 0.0f;
      float x = num1 / 8f;
      float y = num2 / 3f;
      return new SharpDX.RectangleF(x, y, (float) (((double) num1 + 1.0) / 8.0) - x, (float) (((double) num2 + 1.0) / 3.0) - y);
    }

    [Obsolete]
    public static SharpDX.Vector2 RotateVector2(SharpDX.Vector2 v, float angle)
    {
      double radians = MathHepler.ConvertToRadians((double) angle);
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      return new SharpDX.Vector2((float) ((double) v.X * num1 - (double) v.Y * num2), (float) ((double) v.X * num2 + (double) v.Y * num1));
    }

    [Obsolete]
    public static SharpDX.Vector2 NormalizeVector(SharpDX.Vector2 vec)
    {
      float num = MathHepler.VectorLength(vec);
      vec.X /= num;
      vec.Y /= num;
      return vec;
    }

    [Obsolete]
    public static float VectorLength(SharpDX.Vector2 vec) => (float) Math.Sqrt((double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y);

    [Obsolete]
    public static double GetPolarCoordinates(this SharpDX.Vector2 vector, out double phi)
    {
      double polarCoordinates = (double) vector.Length();
      phi = Math.Acos((double) vector.X / polarCoordinates);
      if ((double) vector.Y < 0.0)
        phi = 6.2831854820251465 - phi;
      return polarCoordinates;
    }

    [Obsolete]
    public static SharpDX.Vector2 Translate(this SharpDX.Vector2 vector, float dx = 0.0f, float dy = 0.0f) => new SharpDX.Vector2(vector.X + dx, vector.Y + dy);

    [Obsolete]
    public static SharpDX.Vector2 TranslateToNum(this System.Numerics.Vector2 vector, float dx = 0.0f, float dy = 0.0f) => new SharpDX.Vector2(vector.X + dx, vector.Y + dy);

    [Obsolete]
    public static SharpDX.Vector3 Translate(this SharpDX.Vector3 vector, float dx, float dy, float dz) => new SharpDX.Vector3(vector.X + dx, vector.Y + dy, vector.Z + dz);

    [Obsolete]
    public static float Distance(this SharpDX.Vector2 a, SharpDX.Vector2 b) => SharpDX.Vector2.Distance(a, b);

    [Obsolete]
    public static float DistanceSquared(this SharpDX.Vector2 a, SharpDX.Vector2 b) => SharpDX.Vector2.DistanceSquared(a, b);

    [Obsolete]
    public static bool PointInRectangle(this SharpDX.Vector2 point, SharpDX.RectangleF rect) => (double) point.X >= (double) rect.X && (double) point.Y >= (double) rect.Y && (double) point.X <= (double) rect.Width && (double) point.Y <= (double) rect.Height;

    [Obsolete]
    public static bool PointInRectangle(this System.Numerics.Vector2 point, SharpDX.RectangleF rect) => (double) point.X >= (double) rect.X && (double) point.Y >= (double) rect.Y && (double) point.X <= (double) rect.Width && (double) point.Y <= (double) rect.Height;

    [Obsolete("Use Random.Shared")]
    public static Random Randomizer => Random.Shared;
  }
}
