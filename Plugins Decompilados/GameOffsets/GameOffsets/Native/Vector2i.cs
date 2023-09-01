// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.Vector2i
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System;
using System.Globalization;

namespace GameOffsets.Native
{
  public struct Vector2i : IEquatable<Vector2i>
  {
    public int X;
    public int Y;

    public Vector2i(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public int Length() => (int) Math.Sqrt((double) this.LengthSqr());

    public int LengthSqr() => this.X * this.X + this.Y * this.Y;

    public void Normalize() => Vector2i.Divide(ref this, (float) this.Length(), out this);

    public int Distance(Vector2i v) => Vector2i.Distance(ref this, ref v);

    public float DistanceF(Vector2i v) => Vector2i.DistanceF(ref this, ref v);

    public int Distance(ref Vector2i v) => Vector2i.Distance(ref this, ref v);

    public int DistanceSqr(Vector2i v) => Vector2i.DistanceSqr(ref this, ref v);

    public int DistanceSqr(ref Vector2i v) => Vector2i.DistanceSqr(ref this, ref v);

    public SharpDX.Vector3 ToVector3() => new SharpDX.Vector3((float) this.X, (float) this.Y, 0.0f);

    public SharpDX.Vector2 ToVector2() => new SharpDX.Vector2((float) this.X, (float) this.Y);

    public System.Numerics.Vector2 ToVector2Num() => new System.Numerics.Vector2((float) this.X, (float) this.Y);

    public bool Equals(Vector2i other) => Vector2i.Equals(ref this, ref other);

    public bool Equals(ref Vector2i other) => Vector2i.Equals(ref this, ref other);

    public static bool Equals(ref Vector2i v1, ref Vector2i v2) => v1.X == v2.X && v1.Y == v2.Y;

    public static bool operator ==(Vector2i ls, Vector2i rs) => Vector2i.Equals(ref ls, ref rs);

    public static bool operator !=(Vector2i ls, Vector2i rs) => !Vector2i.Equals(ref ls, ref rs);

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      try
      {
        return this.Equals((Vector2i) obj);
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
    }

    public override int GetHashCode() => this.X.GetHashCode() * 397 ^ this.Y.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{{{0}, {1}}}", (object) this.X, (object) this.Y);

    public static Vector2i operator +(Vector2i ls, Vector2i rs)
    {
      Vector2i result;
      Vector2i.Add(ref ls, ref rs, out result);
      return result;
    }

    public static Vector2i operator -(Vector2i ls, Vector2i rs)
    {
      Vector2i result;
      Vector2i.Subtract(ref ls, ref rs, out result);
      return result;
    }

    public static Vector2i operator -(Vector2i v)
    {
      v.X = -v.X;
      v.Y = -v.Y;
      return v;
    }

    public static Vector2i operator *(Vector2i ls, Vector2i rs)
    {
      Vector2i result;
      Vector2i.Multiply(ref ls, ref rs, out result);
      return result;
    }

    public static Vector2i operator *(Vector2i ls, int rs)
    {
      Vector2i result;
      Vector2i.Multiply(ref ls, (float) rs, out result);
      return result;
    }

    public static Vector2i operator *(Vector2i ls, float rs) => new Vector2i((int) ((double) ls.X * (double) rs), (int) ((double) ls.Y * (double) rs));

    public static Vector2i operator /(Vector2i ls, Vector2i rs)
    {
      Vector2i result;
      Vector2i.Multiply(ref ls, ref rs, out result);
      return result;
    }

    public static Vector2i operator /(Vector2i ls, int rs)
    {
      Vector2i result;
      Vector2i.Divide(ref ls, (float) rs, out result);
      return result;
    }

    public static implicit operator System.Numerics.Vector2(Vector2i vector) => vector.ToVector2Num();

    public static void Add(ref Vector2i v1, ref Vector2i v2, out Vector2i result) => result = new Vector2i()
    {
      X = v1.X + v2.X,
      Y = v1.Y + v2.Y
    };

    public static void Subtract(ref Vector2i v1, ref Vector2i v2, out Vector2i result) => result = new Vector2i()
    {
      X = v1.X - v2.X,
      Y = v1.Y - v2.Y
    };

    public static void Multiply(ref Vector2i v1, ref Vector2i v2, out Vector2i result) => result = new Vector2i()
    {
      X = v1.X * v2.X,
      Y = v1.Y * v2.Y
    };

    public static void Multiply(ref Vector2i v1, float scalar, out Vector2i result) => result = new Vector2i()
    {
      X = (int) ((double) v1.X * (double) scalar),
      Y = (int) ((double) v1.Y * (double) scalar)
    };

    public static void Divide(ref Vector2i v1, ref Vector2i v2, out Vector2i result) => result = new Vector2i()
    {
      X = v1.X / v2.X,
      Y = v1.Y / v2.Y
    };

    public static void Divide(ref Vector2i v1, float divisor, out Vector2i result) => Vector2i.Multiply(ref v1, 1f / divisor, out result);

    public static int Distance(ref Vector2i v1, ref Vector2i v2) => (int) Math.Sqrt((double) Vector2i.DistanceSqr(ref v1, ref v2));

    public static float DistanceF(ref Vector2i v1, ref Vector2i v2) => (float) Math.Sqrt((double) Vector2i.DistanceSqr(ref v1, ref v2));

    public static int DistanceSqr(ref Vector2i v1, ref Vector2i v2)
    {
      int num1 = v1.X - v2.X;
      int num2 = v1.Y - v2.Y;
      return num1 * num1 + num2 * num2;
    }

    public static void GetDirection(ref Vector2i from, ref Vector2i to, out Vector2i dir)
    {
      Vector2i.Subtract(ref to, ref from, out dir);
      dir.Normalize();
    }

    public static Vector2i Min(Vector2i v1, Vector2i v2)
    {
      Vector2i result;
      Vector2i.Min(ref v1, ref v2, out result);
      return result;
    }

    public static void Min(ref Vector2i v1, ref Vector2i v2, out Vector2i result)
    {
      result = new Vector2i();
      result.X = Math.Min(v1.X, v2.X);
      result.Y = Math.Min(v1.Y, v2.Y);
    }

    public static Vector2i Max(Vector2i v1, Vector2i v2)
    {
      Vector2i result;
      Vector2i.Max(ref v1, ref v2, out result);
      return result;
    }

    public static void Max(ref Vector2i v1, ref Vector2i v2, out Vector2i result) => result = new Vector2i()
    {
      X = Math.Max(v1.X, v2.X),
      Y = Math.Max(v1.Y, v2.Y)
    };

    public static Vector2i Zero { get; } = new Vector2i(0, 0);

    public static Vector2i One { get; } = new Vector2i(1, 1);

    public static int Dot(Vector2i v1, Vector2i v2) => v1.X * v2.X + v1.Y * v2.Y;
  }
}
