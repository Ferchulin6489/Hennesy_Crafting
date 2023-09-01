// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BestiaryRecipe
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BestiaryRecipe : RemoteMemoryObject
  {
    private List<BestiaryRecipeComponent> components;
    private string description;
    private string hint;
    private string notes;
    private string recipeId;
    private BestiaryRecipeComponent specialMonster;

    public int Id { get; internal set; }

    public string RecipeId => this.recipeId == null ? (this.recipeId = this.M.ReadStringU(this.M.Read<long>(this.Address))) : this.recipeId;

    public string Description => this.description == null ? (this.description = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L))) : this.description;

    public string Notes => this.notes == null ? (this.notes = this.M.ReadStringU(this.M.Read<long>(this.Address + 32L))) : this.notes;

    public string HintText => this.hint == null ? (this.hint = this.M.ReadStringU(this.M.Read<long>(this.Address + 40L))) : this.hint;

    public bool RequireSpecialMonster => this.Components.Count == 4;

    public BestiaryRecipeComponent SpecialMonster
    {
      get
      {
        if (!this.RequireSpecialMonster)
          return (BestiaryRecipeComponent) null;
        if (this.specialMonster == null)
          this.specialMonster = this.Components.FirstOrDefault<BestiaryRecipeComponent>();
        return this.specialMonster;
      }
    }

    public IList<BestiaryRecipeComponent> Components
    {
      get
      {
        if (this.components == null)
        {
          int count = this.M.Read<int>(this.Address + 16L);
          this.components = this.M.ReadSecondPointerArray_Count(this.M.Read<long>(this.Address + 24L), count).Select<long, BestiaryRecipeComponent>((Func<long, BestiaryRecipeComponent>) (x => this.TheGame.Files.BestiaryRecipeComponents.GetByAddress(x))).ToList<BestiaryRecipeComponent>();
        }
        return (IList<BestiaryRecipeComponent>) this.components;
      }
    }

    public override string ToString() => this.HintText + ": " + this.Description;
  }
}
