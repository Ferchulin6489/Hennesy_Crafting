// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.StatDescriptionWrapper`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class StatDescriptionWrapper<T> : UniversalFileWrapper<T> where T : RemoteMemoryObject, new()
  {
    public StatDescriptionWrapper(IMemory mem, Func<long> address)
      : base(mem, address)
    {
    }

    protected override long RecordLength => 8;

    protected override int NumberOfRecords => (int) ((this.LastRecord - this.FirstRecord) / this.RecordLength);
  }
}
