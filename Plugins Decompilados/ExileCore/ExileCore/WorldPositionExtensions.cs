// Decompiled with JetBrains decompiler
// Type: ExileCore.WorldPositionExtensions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System;

namespace ExileCore
{
  public static class WorldPositionExtensions
  {
    private const float MarsEllipticOrbit = 0.092f;
    private const float Offset = 5.434783f;

    [Obsolete]
    public static Vector2 GridToWorld(this Vector2 v) => new Vector2((float) ((double) v.X / 0.092000000178813934 + 5.4347829818725586), (float) ((double) v.Y / 0.092000000178813934 + 5.4347829818725586));

    [Obsolete]
    public static Vector3 GridToWorld(this Vector2 v, float z) => new Vector3((float) ((double) v.X / 0.092000000178813934 + 5.4347829818725586), (float) ((double) v.Y / 0.092000000178813934 + 5.4347829818725586), z);

    [Obsolete]
    public static Vector2 WorldToGrid(this Vector3 v) => new Vector2((float) Math.Floor((double) v.X * 0.092000000178813934), (float) Math.Floor((double) v.Y * 0.092000000178813934));
  }
}
