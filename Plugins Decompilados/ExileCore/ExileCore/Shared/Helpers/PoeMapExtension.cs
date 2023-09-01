// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.PoeMapExtension
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets.Native;
using System;

namespace ExileCore.Shared.Helpers
{
  public static class PoeMapExtension
  {
    public const int TileToGridConversion = 23;
    public const int TileToWorldConversion = 250;
    public const float WorldToGridConversion = 0.092f;
    private const float Offset = 5.43478251f;

    public static System.Numerics.Vector2 GridToWorld(this System.Numerics.Vector2 v) => v / 0.092f + new System.Numerics.Vector2(5.43478251f);

    public static System.Numerics.Vector3 GridToWorld(this System.Numerics.Vector2 v, float z) => new System.Numerics.Vector3((float) ((double) v.X / 0.092000000178813934 + 5.4347825050354), (float) ((double) v.Y / 0.092000000178813934 + 5.4347825050354), z);

    public static System.Numerics.Vector2 GridToWorld(this Vector2i v) => v.ToVector2Num() / 0.092f + new System.Numerics.Vector2(5.43478251f);

    public static System.Numerics.Vector3 GridToWorld(this Vector2i v, float z) => new System.Numerics.Vector3((float) ((double) v.X / 0.092000000178813934 + 5.4347825050354), (float) ((double) v.Y / 0.092000000178813934 + 5.4347825050354), z);

    public static System.Numerics.Vector2 WorldToGrid(this System.Numerics.Vector3 v) => new System.Numerics.Vector2(MathF.Floor(v.X * 0.092f), MathF.Floor(v.Y * 0.092f));

    public static System.Numerics.Vector2 WorldToGrid(this System.Numerics.Vector2 v) => new System.Numerics.Vector2(MathF.Floor(v.X * 0.092f), MathF.Floor(v.Y * 0.092f));

    public static Vector2i WorldToGridI(this System.Numerics.Vector3 v) => new Vector2i((int) MathF.Floor(v.X * 0.092f), (int) MathF.Floor(v.Y * 0.092f));

    public static Vector2i WorldToGridI(this System.Numerics.Vector2 v) => new Vector2i((int) MathF.Floor(v.X * 0.092f), (int) MathF.Floor(v.Y * 0.092f));

    [Obsolete]
    public static SharpDX.Vector2 GridToWorld(this SharpDX.Vector2 v) => new SharpDX.Vector2((float) ((double) v.X / 0.092000000178813934 + 5.4347825050354), (float) ((double) v.Y / 0.092000000178813934 + 5.4347825050354));

    [Obsolete]
    public static SharpDX.Vector3 GridToWorld(this SharpDX.Vector2 v, float z) => new SharpDX.Vector3((float) ((double) v.X / 0.092000000178813934 + 5.4347825050354), (float) ((double) v.Y / 0.092000000178813934 + 5.4347825050354), z);

    [Obsolete]
    public static SharpDX.Vector2 WorldToGrid(this SharpDX.Vector3 v) => new SharpDX.Vector2((float) Math.Floor((double) v.X * 0.092000000178813934), (float) Math.Floor((double) v.Y * 0.092000000178813934));

    [Obsolete]
    public static SharpDX.Vector2 WorldToGrid(this SharpDX.Vector2 v) => new SharpDX.Vector2((float) Math.Floor((double) v.X * 0.092000000178813934), (float) Math.Floor((double) v.Y * 0.092000000178813934));
  }
}
