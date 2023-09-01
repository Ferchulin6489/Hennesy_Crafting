// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.ActiveAnimationData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class ActiveAnimationData : StructuredRemoteMemoryObject<ActiveAnimationOffsets>
  {
    public int AnimationId => this.Structure.AnimationId;

    public float SlowAnimationSpeed => this.Structure.SlowAnimationSpeed;

    public float NormalAnimationSpeed => this.Structure.NormalAnimationSpeed;

    public float? AnimationSpeed
    {
      get
      {
        if (this.Structure.SlowAnimationStartStagePtr != 0L)
        {
          float normalAnimationSpeed = this.NormalAnimationSpeed;
          if ((double) normalAnimationSpeed != 0.0)
            return new float?(normalAnimationSpeed);
        }
        return new float?();
      }
    }

    public AnimationStage SlowAnimationStartStage => this.GetObject<AnimationStage>(this.Structure.SlowAnimationStartStagePtr);

    public AnimationStage SlowAnimationEndStage => this.GetObject<AnimationStage>(this.Structure.SlowAnimationEndStagePtr);

    public Func<float, float> TransformRawProgressFunc
    {
      get
      {
        float num = this.SlowAnimationSpeed / this.NormalAnimationSpeed;
        if ((double) Math.Abs(num - 1f) < 0.001)
          return (Func<float, float>) (f => f);
        float slowdownStart = this.SlowAnimationStartStage.StageStart;
        float slowdownDuration = this.SlowAnimationEndStage.StageStart - slowdownStart;
        float totalDiff = slowdownDuration * (1f - num) / num;
        return (Func<float, float>) (progress => progress + totalDiff * Math.Clamp((progress - slowdownStart) / slowdownDuration, 0.0f, 1f));
      }
    }
  }
}
