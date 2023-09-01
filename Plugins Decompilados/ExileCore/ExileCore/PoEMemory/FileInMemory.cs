// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FileInMemory
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;


#nullable enable
namespace ExileCore.PoEMemory
{
  public abstract class FileInMemory
  {
    private readonly 
    #nullable disable
    Func<long> fAddress;

    protected FileInMemory(IMemory m, Func<long> address)
    {
      this.M = m;
      this.Address = address();
      this.fAddress = address;
    }

    public IMemory M { get; }

    public long Address { get; }

    protected long FirstRecord => this.M.Read<long>(this.fAddress() + 64L, new int[1]);

    protected long LastRecord => this.M.Read<long>(this.fAddress() + 64L, 8);

    protected virtual int NumberOfRecords => this.M.Read<int>(this.fAddress() + 64L, 64);

    protected virtual long RecordLength
    {
      get
      {
        int numberOfRecords = this.NumberOfRecords;
        return numberOfRecords != 0 ? (this.LastRecord - this.FirstRecord) / (long) numberOfRecords : 0L;
      }
    }

    protected IEnumerable<long> RecordAddresses()
    {
      if (this.fAddress() == 0L)
      {
        yield return 0;
      }
      else
      {
        int cnt = this.NumberOfRecords;
        if (cnt == 0)
        {
          yield return 0;
        }
        else
        {
          long firstRec = this.FirstRecord;
          long recLen = this.RecordLength;
          for (int i = 0; i < cnt; ++i)
            yield return firstRec + (long) i * recLen;
        }
      }
    }
  }
}
