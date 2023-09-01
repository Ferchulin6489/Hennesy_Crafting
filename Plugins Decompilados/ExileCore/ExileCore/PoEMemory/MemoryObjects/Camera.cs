// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Camera
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using Serilog;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class Camera : RemoteMemoryObject
  {
    private readonly CachedValue<CameraOffsets> _cachedValue;

    public Camera()
    {
      this._cachedValue = (CachedValue<CameraOffsets>) new FrameCache<CameraOffsets>((Func<CameraOffsets>) (() => this.M.Read<CameraOffsets>(this.Address)));
      this._cachedValue.OnUpdate += (CachedValue<CameraOffsets>.CacheUpdateEvent) (offsets =>
      {
        this.HalfHeight = (float) offsets.Height * 0.5f;
        this.HalfWidth = (float) offsets.Width * 0.5f;
      });
    }

    public CameraOffsets CameraOffsets => this._cachedValue.Value;

    public int Width => this.CameraOffsets.Width;

    public int Height => this.CameraOffsets.Height;

    private float HalfWidth { get; set; }

    private float HalfHeight { get; set; }

    [Obsolete]
    public SharpDX.Vector2 Size => new SharpDX.Vector2((float) this.Width, (float) this.Height);

    public System.Numerics.Vector2 SizeNum => new System.Numerics.Vector2((float) this.Width, (float) this.Height);

    public float ZFar => this.CameraOffsets.ZFar;

    [Obsolete]
    public SharpDX.Vector3 Position => this.CameraOffsets.Position.ToSharpDx();

    public System.Numerics.Vector3 PositionNum => this.CameraOffsets.Position;

    public string PositionString => this.PositionNum.ToString();

    private Matrix4x4 Matrix => this.CameraOffsets.MatrixBytes;

    public System.Numerics.Vector2 WorldToScreen(System.Numerics.Vector3 vec)
    {
      try
      {
        System.Numerics.Vector4 left = System.Numerics.Vector4.Transform(new System.Numerics.Vector4(vec, 1f), this.Matrix);
        System.Numerics.Vector4 vector4 = System.Numerics.Vector4.Divide(left, left.W);
        System.Numerics.Vector2 screen;
        screen.X = (vector4.X + 1f) * this.HalfWidth;
        screen.Y = (1f - vector4.Y) * this.HalfHeight;
        return screen;
      }
      catch (Exception ex)
      {
        ILogger logger = Core.Logger;
        if (logger != null)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
          interpolatedStringHandler.AppendLiteral("Camera WorldToScreen ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          logger.Error(interpolatedStringHandler.ToStringAndClear());
        }
      }
      return System.Numerics.Vector2.Zero;
    }

    [Obsolete]
    public SharpDX.Vector2 WorldToScreen(SharpDX.Vector3 vec) => this.WorldToScreen(vec.ToVector3Num()).ToSharpDx();
  }
}
