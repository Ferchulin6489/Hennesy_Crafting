// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalEventData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalEventData : Element
  {
    public BetrayalTarget Target1 => this.TheGame.Files.BetrayalTargets.GetByAddress(this.M.Read<long>(this.Address + 784L));

    public BetrayalTarget Target2 => this.TheGame.Files.BetrayalTargets.GetByAddress(this.M.Read<long>(this.Address + 816L));

    public BetrayalTarget Target3 => this.TheGame.Files.BetrayalTargets.GetByAddress(this.M.Read<long>(this.Address + 832L));

    public BetrayalChoiceAction Action => this.TheGame.Files.BetrayalChoiceActions.GetByAddress(this.M.Read<long>(this.Address + 800L));

    public string EventText => this.GetChildFromIndices(8, 1)?.Text;

    public Element ReleaseButton => this[6];

    public Element InterrogateButton => this.GetChildFromIndices(7, 0);

    public Element SpecialButton => this.GetChildFromIndices(8, 0);
  }
}
