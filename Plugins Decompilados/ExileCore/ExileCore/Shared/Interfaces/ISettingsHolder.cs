// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Interfaces.ISettingsHolder
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;

namespace ExileCore.Shared.Interfaces
{
  public interface ISettingsHolder
  {
    string Name { get; set; }

    string Tooltip { get; set; }

    string Unique { get; }

    int ID { get; set; }

    Action DrawDelegate { get; set; }

    IList<ISettingsHolder> Children { get; }

    void Draw();
  }
}
