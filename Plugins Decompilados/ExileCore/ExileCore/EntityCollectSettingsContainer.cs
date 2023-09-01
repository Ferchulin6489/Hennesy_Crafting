// Decompiled with JetBrains decompiler
// Type: ExileCore.EntityCollectSettingsContainer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Nodes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ExileCore
{
  public class EntityCollectSettingsContainer
  {
    public Stack<Entity> Simple { get; set; }

    public Queue<uint> KeyForDelete { get; set; }

    public ConcurrentDictionary<uint, Entity> EntityCache { get; set; }

    public MultiThreadManager MultiThreadManager { get; set; }

    public Func<long> EntitiesCount { get; set; }

    public uint EntitiesVersion { get; set; }

    public bool NeedUpdate { get; set; } = true;

    public ToggleNode CollectEntitiesInParallelWhenMoreThanX { get; set; }

    public DebugInformation DebugInformation { get; set; }

    public bool Break { get; set; }

    public Func<bool> ParseEntitiesInMultiThread { get; set; }
  }
}
