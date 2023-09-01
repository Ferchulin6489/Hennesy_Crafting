// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.FrameCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class FrameCache<T> : CachedValue<T>
  {
    private uint _frame;

    public FrameCache(Func<T> func)
      : base(func)
    {
      this._frame = uint.MaxValue;
    }

    protected override bool Update(bool force)
    {
      if (!((int) this._frame != (int) Core.FramesCount | force))
        return false;
      this._frame = Core.FramesCount;
      return true;
    }
  }
}
