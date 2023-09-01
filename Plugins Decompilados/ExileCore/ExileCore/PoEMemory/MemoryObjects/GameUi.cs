// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.GameUi
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class GameUi : Element
  {
    public Element UnusedPassivePointsButton => this.GetChildAtIndex(3);

    public int UnusedPassivePointsAmount => this.GetUnusedPassivePointsAmount();

    public SentinelPanel SentinelPanel => this.GetChildFromIndices(7, 12, 4)?.AsObject<SentinelPanel>();

    public Element LifeOrb => this[1];

    public Element ManaOrb => this[2];

    private int GetUnusedPassivePointsAmount()
    {
      Element childFromIndices = this.GetChildFromIndices(3, 1);
      int result;
      return childFromIndices == null || !childFromIndices.IsVisible || !int.TryParse(childFromIndices.Text, out result) ? 0 : result;
    }
  }
}
