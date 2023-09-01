// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Ancestor.AncestorFightSelectionOpponentLine
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Linq;

namespace ExileCore.PoEMemory.MemoryObjects.Ancestor
{
  public class AncestorFightSelectionOpponentLine : Element
  {
    public Entity Reward
    {
      get
      {
        Element rewardElement = this.RewardElement;
        if (rewardElement == null)
          return (Entity) null;
        return rewardElement.Children.LastOrDefault<Element>()?.ReadObjectAt<Entity>(896);
      }
    }

    public Element RewardElement => this.GetChildFromIndices(4, 0);
  }
}
