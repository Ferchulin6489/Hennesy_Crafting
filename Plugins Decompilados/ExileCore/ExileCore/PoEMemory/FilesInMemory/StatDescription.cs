// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.StatDescription
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class StatDescription : RemoteMemoryObject
  {
    private List<string> _strings;

    public List<string> Strings
    {
      get
      {
        List<string> strings = this._strings;
        if (strings != null)
          return strings;
        return this._strings = ((IEnumerable<StatDescriptionStringContainer>) this.M.ReadStdVector<StatDescriptionStringContainer>(this.M.Read<StdVector>(this.Address, 16))).Select<StatDescriptionStringContainer, string>((Func<StatDescriptionStringContainer, string>) (x => this.M.ReadStringU(x.StringPtr))).ToList<string>();
      }
    }
  }
}
