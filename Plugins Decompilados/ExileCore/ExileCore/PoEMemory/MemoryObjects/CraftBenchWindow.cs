// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.CraftBenchWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class CraftBenchWindow : Element
  {
    public Element PrefixesElement => this.GetChildFromIndices(3, 0, 0, 1, 0);

    public Element SuffixesElement => this.GetChildFromIndices(3, 0, 1, 1, 0);

    public Element FilterElement => this.GetChildFromIndices(2, 1, 0, 0);

    public Element CraftsListElement => this.GetChildFromIndices(3);

    public Element ItemSlotElement => this.GetChildFromIndices(5, 1);

    public Element CraftButton => this.GetChildFromIndices(5, 0, 0);
  }
}
