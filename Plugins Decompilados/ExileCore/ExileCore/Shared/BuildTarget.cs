// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.BuildTarget
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections;

namespace ExileCore.Shared
{
  public class BuildTarget
  {
    public string Name { get; set; }

    public string File { get; set; }

    public bool Succeeded { get; set; }

    public IEnumerable Outputs { get; set; }
  }
}
