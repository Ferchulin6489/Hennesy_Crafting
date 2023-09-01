// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.SupportedAnimationList
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
  public class SupportedAnimationList : StructuredRemoteMemoryObject<ActorAnimationListOffsets>
  {
    private List<AnimationStageList> _animations;

    public List<AnimationStageList> Animations => this._animations ?? (this._animations = this.M.ReadStructsArray<AnimationStageList>(this.Structure.AnimationList.First, this.Structure.AnimationList.Last, 24, (RemoteMemoryObject) null));
  }
}
