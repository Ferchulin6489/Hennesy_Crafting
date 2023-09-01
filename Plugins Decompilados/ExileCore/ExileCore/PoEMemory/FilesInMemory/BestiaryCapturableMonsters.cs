// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BestiaryCapturableMonsters
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;
using System;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BestiaryCapturableMonsters : UniversalFileWrapper<BestiaryCapturableMonster>
  {
    private int IdCounter;

    public BestiaryCapturableMonsters(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    protected new void EntryAdded(long addr, BestiaryCapturableMonster entry) => entry.Id = this.IdCounter++;
  }
}
