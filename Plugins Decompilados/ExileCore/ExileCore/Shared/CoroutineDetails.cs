// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.CoroutineDetails
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared
{
  public struct CoroutineDetails
  {
    public CoroutineDetails(
      string name,
      string ownerName,
      long ticks,
      DateTime started,
      DateTime finished)
    {
      this.Name = name;
      this.OwnerName = ownerName;
      this.Ticks = ticks;
      this.Started = started;
      this.Finished = finished;
    }

    public string Name { get; set; }

    public string OwnerName { get; set; }

    public long Ticks { get; set; }

    public DateTime Started { get; set; }

    public DateTime Finished { get; set; }
  }
}
