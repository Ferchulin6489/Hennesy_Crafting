// Decompiled with JetBrains decompiler
// Type: ExileCore.PluginPanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore
{
  public class PluginPanel
  {
    private readonly List<Func<bool>> settings = new List<Func<bool>>();

    public PluginPanel(Vector2 startDrawPoint, Direction direction = Direction.Down)
      : this(direction)
    {
      this.StartDrawPoint = startDrawPoint;
    }

    public PluginPanel(Direction direction = Direction.Down) => this.Margin = new Vector2(0.0f, 0.0f);

    public bool Used => this.settings.Any<Func<bool>>((Func<Func<bool>, bool>) (x => x()));

    public Vector2 StartDrawPoint { get; set; }

    public Vector2 Margin { get; }

    public void WantUse(Func<bool> enabled) => this.settings.Add(enabled);
  }
}
