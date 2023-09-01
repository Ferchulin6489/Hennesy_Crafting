// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalDialogue
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalDialogue : RemoteMemoryObject
  {
    public BetrayalTarget Target => this.TheGame.Files.BetrayalTargets.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public int Unknown1 => this.M.Read<int>(this.Address + 16L);

    public int Unknown2 => this.M.Read<int>(this.Address + 20L);

    public int Unknown3 => this.M.Read<int>(this.Address + 56L);

    public bool Unknown4 => this.M.Read<byte>(this.Address + 108L) > (byte) 0;

    public bool Unknown5 => this.M.Read<byte>(this.Address + 141L) > (byte) 0;

    public BetrayalJob Job => this.TheGame.Files.BetrayalJobs.GetByAddress(this.M.Read<long>(this.Address + 68L));

    public BetrayalUpgrade Upgrade => this.ReadObjectAt<BetrayalUpgrade>(100);

    public string DialogueText => this.M.ReadStringU(this.M.Read<long>(this.Address + 166L, 24));

    public List<int> Keys1 => this.ReadKeys(32L);

    public List<int> Keys2 => this.ReadKeys(84L);

    public List<int> Keys3 => this.ReadKeys(133L);

    private List<int> ReadKeys(long offset)
    {
      long num = this.M.Read<long>(this.Address + offset);
      List<int> intList = new List<int>();
      if (num != 0L)
      {
        for (long index = 0; index < 5L; ++index)
          intList.Add(this.M.Read<int>(num + index * 8L));
      }
      return intList;
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 4);
      interpolatedStringHandler.AppendFormatted(this.Target?.Name);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Job?.Name);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Upgrade?.UpgradeName);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.DialogueText);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
