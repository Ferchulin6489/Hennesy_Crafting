// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.StateMachine
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
  public class StateMachine : Component
  {
    private readonly CachedValue<StateMachineComponentOffsets> _stateMachine;
    private readonly CachedValue<IList<StateMachineState>> _statesCache;

    public StateMachine()
    {
      this._stateMachine = (CachedValue<StateMachineComponentOffsets>) new FrameCache<StateMachineComponentOffsets>((Func<StateMachineComponentOffsets>) (() => this.Address != 0L ? this.M.Read<StateMachineComponentOffsets>(this.Address) : new StateMachineComponentOffsets()));
      this._statesCache = (CachedValue<IList<StateMachineState>>) new FrameCache<IList<StateMachineState>>(CacheUtils.RememberLastValue<IList<StateMachineState>>(new Func<IList<StateMachineState>, IList<StateMachineState>>(this.ReadStates)));
    }

    public IList<StateMachineState> States => this._statesCache.Value;

    public bool CanBeTarget => this.M.Read<byte>(this.Address + 160L) == (byte) 1;

    public bool InTarget => this.M.Read<byte>(this.Address + 162L) == (byte) 1;

    public IList<StateMachineState> ReadStates() => this.ReadStates((IList<StateMachineState>) null);

    private IList<StateMachineState> ReadStates(IList<StateMachineState> lastValue)
    {
      StateMachineComponentOffsets componentOffsets = this._stateMachine.Value;
      long num1 = componentOffsets.StatesValues.ElementCount<long>();
      List<StateMachineState> stateMachineStateList = new List<StateMachineState>();
      if (num1 <= 0L)
        return lastValue == null || lastValue.Count <= 0 || !componentOffsets.StatesValues.Equals(new NativePtrArray()) ? (IList<StateMachineState>) stateMachineStateList : lastValue;
      if (num1 > 100L)
      {
        Logger.Log.Error("Error reading states in StateMachine component");
        return (IList<StateMachineState>) stateMachineStateList;
      }
      long[] numArray = this.M.ReadStdVector<long>(componentOffsets.StatesValues);
      long num2 = this.M.Read<long>(componentOffsets.StatesPtr + 16L);
      for (int index = 0; (long) index < num1; ++index)
      {
        string name = this.M.Read<NativeUtf8Text>(num2 + (long) (index * 192)).ToString(this.M);
        long num3 = numArray[index];
        stateMachineStateList.Add(new StateMachineState(name, num3));
      }
      return (IList<StateMachineState>) stateMachineStateList;
    }
  }
}
