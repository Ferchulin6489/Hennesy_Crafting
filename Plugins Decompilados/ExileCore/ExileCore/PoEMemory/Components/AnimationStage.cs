// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.AnimationStage
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Helpers;
using GameOffsets;

namespace ExileCore.PoEMemory.Components
{
  public class AnimationStage : StructuredRemoteMemoryObject<ActorAnimationStageOffsets>
  {
    private string _stageName;
    private int? _actorAnimationListIndex;
    private float? _stageStart;

    public float StageStart
    {
      get
      {
        float valueOrDefault = this._stageStart.GetValueOrDefault();
        if (this._stageStart.HasValue)
          return valueOrDefault;
        float stageStart = this.Structure.StageStart;
        this._stageStart = new float?(stageStart);
        return stageStart;
      }
    }

    public int ActorAnimationListIndex
    {
      get
      {
        int valueOrDefault = this._actorAnimationListIndex.GetValueOrDefault();
        if (this._actorAnimationListIndex.HasValue)
          return valueOrDefault;
        int animationListIndex = this.Structure.ActorAnimationListIndex;
        this._actorAnimationListIndex = new int?(animationListIndex);
        return animationListIndex;
      }
    }

    public string StageName => this._stageName ?? (this._stageName = this.Structure.StageName.ToString(this.M));
  }
}
