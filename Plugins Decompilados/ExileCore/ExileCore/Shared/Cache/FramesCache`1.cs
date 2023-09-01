// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.FramesCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class FramesCache<T> : FrameCache<T>
  {
    private readonly uint _waitFrames;
    private uint _frame;

    public FramesCache(Func<T> func, uint waitFrames = 1)
      : base(func)
    {
      this._waitFrames = waitFrames;
      this._frame = 0U;
    }

    protected override bool Update(bool force)
    {
      if (!(Core.FramesCount >= this._frame | force))
        return false;
      this._frame += this._waitFrames;
      return true;
    }
  }
}
