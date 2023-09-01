// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.NpcDialog
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class NpcDialog : Element
  {
    public string NpcName => this.GetChildAtIndex(1)?.GetChildAtIndex(3)?.Text;

    public Element NpcLineWrapper => this.GetChildAtIndex(0)?.GetChildAtIndex(2);

    public List<NpcLine> NpcLines => this.GetNpcLines();

    public bool IsLoreTalkVisible => this.NpcLines.Count == 0 && this.IsVisible;

    private List<NpcLine> GetNpcLines()
    {
      List<NpcLine> npcLines = new List<NpcLine>();
      if (this.NpcLineWrapper?.Children == null)
      {
        DebugWindow.LogError("NpcLineWrapper?.Children is null, check offsets");
        return npcLines;
      }
      foreach (Element child in (IEnumerable<Element>) this.NpcLineWrapper?.Children)
      {
        try
        {
          NpcLine npcLine = new NpcLine(child);
          npcLines.Add(npcLine);
        }
        catch
        {
        }
      }
      return npcLines;
    }
  }
}
