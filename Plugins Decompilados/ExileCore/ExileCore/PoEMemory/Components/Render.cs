// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Render
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using System;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class Render : Component
  {
    private readonly CachedValue<RenderComponentOffsets> _cachedValue;

    public Render() => this._cachedValue = (CachedValue<RenderComponentOffsets>) new FrameCache<RenderComponentOffsets>((Func<RenderComponentOffsets>) (() => this.M.Read<RenderComponentOffsets>(this.Address)));

    public RenderComponentOffsets RenderStruct => this._cachedValue.Value;

    public float X => this.PosNum.X;

    public float Y => this.PosNum.Y;

    public float Z => this.PosNum.Z;

    [Obsolete]
    public SharpDX.Vector3 Pos => this.RenderStruct.Pos.ToSharpDx();

    public System.Numerics.Vector3 PosNum => this.RenderStruct.Pos;

    [Obsolete]
    public SharpDX.Vector3 InteractCenter => this.InteractCenterNum.ToSharpDx();

    public System.Numerics.Vector3 InteractCenterNum => this.PosNum + this.BoundsNum / 2f;

    public float Height => (double) this.RenderStruct.Height <= 0.0099999997764825821 ? 0.0f : this.RenderStruct.Height;

    public string Name
    {
      get
      {
        IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (Render));
        interpolatedStringHandler.AppendFormatted<long>(this.RenderStruct.Name.buf);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        Func<string> func = (Func<string>) (() => this.RenderStruct.Name.ToString(this.M));
        return stringCache.Read(stringAndClear, func);
      }
    }

    public string NameNoCache => this.RenderStruct.Name.ToString(this.M);

    [Obsolete]
    public SharpDX.Vector3 Rotation => this.RenderStruct.Rotation.ToSharpDx();

    public System.Numerics.Vector3 RotationNum => this.RenderStruct.Rotation;

    [Obsolete]
    public SharpDX.Vector3 Bounds => this.RenderStruct.Bounds.ToSharpDx();

    public System.Numerics.Vector3 BoundsNum => this.RenderStruct.Bounds;

    [Obsolete]
    public SharpDX.Vector3 MeshRoration => this.RenderStruct.Rotation.ToSharpDx();

    public System.Numerics.Vector3 MeshRotationNum => this.RenderStruct.Rotation;

    public float TerrainHeight => (double) this.RenderStruct.Height <= 0.0099999997764825821 ? 0.0f : this.RenderStruct.Height;
  }
}
