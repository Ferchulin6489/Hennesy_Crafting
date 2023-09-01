// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.AnimationStageList
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public class AnimationStageList : RemoteMemoryObject
  {
    private List<AnimationStage> _stages;

    private NativePtrArray StageList => this.M.Read<NativePtrArray>(this.Address);

    public List<AnimationStage> AllStages => this._stages ?? (this._stages = ((IEnumerable<long>) this.M.ReadStdVector<long>(this.StageList)).Select<long, AnimationStage>(new Func<long, AnimationStage>(((RemoteMemoryObject) this).GetObject<AnimationStage>)).ToList<AnimationStage>());
  }
}
