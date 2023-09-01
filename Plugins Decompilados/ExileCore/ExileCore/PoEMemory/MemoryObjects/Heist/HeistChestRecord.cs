// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Heist.HeistChestRecord
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects.Heist
{
  public class HeistChestRecord : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address, new int[1]));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
