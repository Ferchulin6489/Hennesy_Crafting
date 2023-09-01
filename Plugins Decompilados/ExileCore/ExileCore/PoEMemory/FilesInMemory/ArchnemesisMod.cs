// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.ArchnemesisMod
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory.Archnemesis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class ArchnemesisMod : RemoteMemoryObject
  {
    private string _displayName;
    private string _internalName;
    private string _description;
    private string _iconPath;
    private string _formatString;
    private List<string> _rewards;
    private ArchnemesisRecipe _recipe;
    private bool _recipeChecked;

    public string DisplayName
    {
      get
      {
        string displayName = this._displayName;
        if (displayName != null)
          return displayName;
        return this._displayName = this.M.ReadStringU(this.M.Read<long>(this.Address + 106L, 16));
      }
    }

    public string Description => this._description ?? (this._description = this.M.ReadStringU(this.M.Read<long>(this.Address + 98L)));

    public string InternalName => this._internalName ?? (this._internalName = this.M.ReadStringU(this.M.Read<long>(this.Address + 66L, new int[1])));

    public string IconPath => this._iconPath ?? (this._iconPath = this.M.ReadStringU(this.M.Read<long>(this.Address + 56L)));

    public string FormatString => this._formatString ?? (this._formatString = this.M.ReadStringU(this.M.Read<long>(this.Address + 48L)));

    public List<string> Rewards
    {
      get
      {
        if (this._rewards == null)
        {
          this._rewards = new List<string>();
          long num = this.M.Read<long>(this.Address + 24L);
          for (int index = 0; index < this.M.Read<int>(this.Address + 16L); ++index)
            this.Rewards.Add(this.M.ReadStringU(this.M.Read<long>(this.M.Read<long>(num + (long) (index * 2 * 8)))));
        }
        return this._rewards;
      }
    }

    public ArchnemesisRecipe Recipe
    {
      get
      {
        if (!this._recipeChecked)
        {
          this._recipeChecked = true;
          this._recipe = this.TheGame.Files.ArchnemesisRecipes.EntriesList.FirstOrDefault<ArchnemesisRecipe>((Func<ArchnemesisRecipe, bool>) (x => x.Outcome.Address == this.Address));
        }
        return this._recipe;
      }
    }

    public override string ToString() => this.DisplayName + " (" + string.Join(", ", (IEnumerable<string>) this.Rewards) + ")";
  }
}
