// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.AnimationController
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class AnimationController : Component
  {
    private readonly CachedValue<Func<float, float>> _progressTransformFunc;
    private readonly CachedValue<float> _animationSpeed;
    private readonly CachedValue<SupportedAnimationList> _supportedAnimationList;
    private readonly CachedValue<AnimationControllerOffsets> _cachedValue;

    public AnimationController()
    {
      this._cachedValue = (CachedValue<AnimationControllerOffsets>) this.CreateStructFrameCache<AnimationControllerOffsets>();
      this._progressTransformFunc = (CachedValue<Func<float, float>>) KeyTrackingCache.Create<Func<float, float>, (int, int)>((Func<Func<float, float>>) (() => this.ActiveAnimationData?.TransformRawProgressFunc ?? (Func<float, float>) (f => f)), (Func<(int, int)>) (() => (this.CurrentAnimationId, this.CurrentAnimationStage)));
      this._animationSpeed = (CachedValue<float>) KeyTrackingCache.Create<float, (int, int)>((Func<float>) (() => (float?) this.ActiveAnimationData?.AnimationSpeed ?? this.RawAnimationSpeed), (Func<(int, int)>) (() => (this.CurrentAnimationId, this.CurrentAnimationStage)));
      this._supportedAnimationList = (CachedValue<SupportedAnimationList>) new TimeCache<SupportedAnimationList>((Func<SupportedAnimationList>) (() => this.GetObject<SupportedAnimationList>(this.Structure.ActorAnimationArrayPtr)), 1000L);
    }

    public AnimationControllerOffsets Structure => this._cachedValue.Value;

    private ActiveAnimationData ActiveAnimationData => ((IEnumerable<long>) this.M.ReadStdVector<long>(this.Structure.ActiveAnimationsArrayPtr)).Select<long, ActiveAnimationData>(new Func<long, ActiveAnimationData>(((RemoteMemoryObject) this).ReadObject<ActiveAnimationData>)).LastOrDefault<ActiveAnimationData>((Func<ActiveAnimationData, bool>) (x => x.AnimationId == this.CurrentAnimationId));

    public float TransformProgress(float progress) => this._progressTransformFunc.Value(progress);

    public float MaxRawAnimationProgress => this.Structure.MaxAnimationProgress - this.Structure.MaxAnimationProgressOffset;

    public float RawNextAnimationPoint => this.Structure.NextAnimationPoint;

    public float RawAnimationProgress => this.Structure.AnimationProgress;

    public float RawAnimationSpeed => this.Structure.AnimationSpeedMultiplier1 * this.Structure.AnimationSpeedMultiplier2;

    public float TransformedMaxRawAnimationProgress => this.TransformProgress(this.MaxRawAnimationProgress);

    public float TransformedRawNextAnimationPoint => this.TransformProgress(this.RawNextAnimationPoint);

    public float TransformedRawAnimationProgress => this.TransformProgress(this.RawAnimationProgress);

    public float AnimationSpeed => this._animationSpeed.Value;

    public SupportedAnimationList SupportedAnimationList => this._supportedAnimationList.Value;

    public int CurrentAnimationId => this.Structure.AnimationInActorId;

    public int CurrentAnimationStage => this.Structure.CurrentAnimationStage;

    public AnimationStageList CurrentAnimation
    {
      get
      {
        if (this.CurrentAnimationId < 0 || this.CurrentAnimationId >= this.SupportedAnimationList.Animations.Count)
        {
          __Boxed<int> currentAnimationId = (ValueType) this.CurrentAnimationId;
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
          interpolatedStringHandler.AppendLiteral("There's only ");
          interpolatedStringHandler.AppendFormatted<int>(this.SupportedAnimationList.Animations.Count);
          interpolatedStringHandler.AppendLiteral(" animations");
          string stringAndClear = interpolatedStringHandler.ToStringAndClear();
          throw new ArgumentOutOfRangeException("CurrentAnimationId", (object) currentAnimationId, stringAndClear);
        }
        return this.SupportedAnimationList.Animations[this.CurrentAnimationId];
      }
    }

    public float NextAnimationPoint => this.TransformedRawNextAnimationPoint / this.TransformedMaxRawAnimationProgress;

    public float AnimationProgress => this.TransformedRawAnimationProgress / this.TransformedMaxRawAnimationProgress;

    public TimeSpan AnimationCompletesIn
    {
      get
      {
        float f = (this.TransformedMaxRawAnimationProgress - this.TransformedRawAnimationProgress) / this.AnimationSpeed;
        return TimeSpan.FromSeconds(float.IsNaN(f) || float.IsInfinity(f) ? 1.0 : (double) f);
      }
    }

    public TimeSpan AnimationActiveFor
    {
      get
      {
        float f = this.TransformedRawAnimationProgress / this.AnimationSpeed;
        return TimeSpan.FromSeconds(float.IsNaN(f) || float.IsInfinity(f) ? 0.0 : (double) f);
      }
    }
  }
}
