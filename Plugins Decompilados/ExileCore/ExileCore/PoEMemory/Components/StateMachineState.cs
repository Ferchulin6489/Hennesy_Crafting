// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.StateMachineState
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class StateMachineState
  {
    public StateMachineState(string name, long value)
    {
      this.Name = name;
      this.Value = value;
    }

    public string Name { get; }

    public long Value { get; }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(": ");
      interpolatedStringHandler.AppendFormatted<long>(this.Value);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
