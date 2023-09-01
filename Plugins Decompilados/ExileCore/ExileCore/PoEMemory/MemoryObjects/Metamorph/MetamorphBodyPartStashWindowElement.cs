// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Metamorph.MetamorphBodyPartStashWindowElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects.Metamorph
{
  public class MetamorphBodyPartStashWindowElement : Element
  {
    public string BodyPartName => this.GetChildFromIndices(1, 0).Text;

    public IEnumerable<MetamorphBodyPartElement> GetBodyPartStashWindowElements => (IEnumerable<MetamorphBodyPartElement>) this.GetChildAtIndex(0).GetChildrenAs<MetamorphBodyPartElement>();

    public override string ToString() => this.BodyPartName ?? "";
  }
}
