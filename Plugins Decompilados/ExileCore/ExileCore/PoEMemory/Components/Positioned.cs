// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Positioned
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class Positioned : Component
  {
    private readonly CachedValue<PositionedComponentOffsets> _cachedValue;

    public Positioned() => this._cachedValue = (CachedValue<PositionedComponentOffsets>) new FrameCache<PositionedComponentOffsets>((Func<PositionedComponentOffsets>) (() => this.M.Read<PositionedComponentOffsets>(this.Address)));

    public PositionedComponentOffsets PositionedStruct => this._cachedValue.Value;

    public new long OwnerAddress => this.PositionedStruct.OwnerAddress;

    public int GridX => this.GridPosition.X;

    public int GridY => this.GridPosition.Y;

    [Obsolete]
    public SharpDX.Vector2 GridPos => new SharpDX.Vector2((float) this.GridX, (float) this.GridY);

    public System.Numerics.Vector2 GridPosNum => new System.Numerics.Vector2((float) this.GridX, (float) this.GridY);

    public Vector2i GridPosI => new Vector2i(this.GridX, this.GridY);

    [Obsolete]
    public SharpDX.Vector2 WorldPos => this.PositionedStruct.WorldPosition.ToSharpDx();

    public System.Numerics.Vector2 WorldPosNum => this.PositionedStruct.WorldPosition;

    public Vector2i GridPosition => this.PositionedStruct.GridPosition;

    public float Rotation => this.PositionedStruct.Rotation;

    public float WorldX => this.WorldPosNum.X;

    public float WorldY => this.WorldPosNum.Y;

    public float RotationDeg => this.Rotation * 57.2957764f;

    public byte Reaction => this.PositionedStruct.Reaction;

    public int Size => this.PositionedStruct.Size;

    public float Scale => this.PositionedStruct.Scale;
  }
}
