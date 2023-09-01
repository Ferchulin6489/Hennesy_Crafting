// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Archnemesis.ArchnemesisRecipe
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory.Archnemesis
{
  public class ArchnemesisRecipe : RemoteMemoryObject
  {
    private ArchnemesisMod _outcome;
    private List<ArchnemesisMod> _components;

    public ArchnemesisMod Outcome => this._outcome ?? (this._outcome = this.TheGame.Files.ArchnemesisMods.GetByAddress(this.M.Read<long>(this.Address)));

    public List<ArchnemesisMod> Components
    {
      get
      {
        if (this._components == null)
        {
          this._components = new List<ArchnemesisMod>();
          long num = this.M.Read<long>(this.Address + 24L);
          for (int index = 0; index < this.M.Read<int>(this.Address + 16L); ++index)
            this.Components.Add(this.TheGame.Files.ArchnemesisMods.GetByAddress(this.M.Read<long>(num + (long) (index * 2 * 8))));
        }
        return this._components;
      }
    }

    public override string ToString() => this.Outcome.DisplayName + " (" + string.Join(", ", this.Components.Select<ArchnemesisMod, string>((Func<ArchnemesisMod, string>) (x => x.DisplayName))) + ")";
  }
}
