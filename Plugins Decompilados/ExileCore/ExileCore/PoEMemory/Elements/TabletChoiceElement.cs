// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.TabletChoiceElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.Elements
{
  public class TabletChoiceElement : Element
  {
    private StampChoice _choice;

    public StampChoice Choice => this._choice ?? (this._choice = this.TheGame.Files.StampChoices.GetByAddress(this.M.Read<long>(this.Address + 792L)));
  }
}
