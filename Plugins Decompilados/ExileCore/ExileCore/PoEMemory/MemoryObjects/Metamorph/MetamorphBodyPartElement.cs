// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Metamorph.MetamorphBodyPartElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory.Metamorph;

namespace ExileCore.PoEMemory.MemoryObjects.Metamorph
{
  public class MetamorphBodyPartElement : Element
  {
    public MetamorphMetaSkill MetaSkill => this.ReadObjectAt<MetamorphMetaSkill>(1488);
  }
}
