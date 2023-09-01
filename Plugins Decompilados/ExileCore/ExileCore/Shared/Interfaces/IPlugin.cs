// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Interfaces.IPlugin
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System;

namespace ExileCore.Shared.Interfaces
{
  public interface IPlugin : IDisposable
  {
    bool Initialized { get; set; }

    ISettings _Settings { get; }

    bool CanUseMultiThreading { get; }

    bool Force { get; }

    string DirectoryName { get; set; }

    string DirectoryFullName { get; set; }

    string InternalName { get; }

    string Name { get; }

    string Description { get; }

    int Order { get; }

    void DrawSettings();

    void OnLoad();

    void OnUnload();

    bool Initialise();

    Job Tick();

    void Render();

    void OnClose();

    void SetApi(GameController gameController, Graphics graphics, PluginManager pluginManager);

    void OnPluginSelectedInMenu();

    void EntityAdded(Entity entity);

    void EntityRemoved(Entity entity);

    void EntityAddedAny(Entity entity);

    void EntityIgnored(Entity entity);

    void AreaChange(AreaInstance area);

    void ReceiveEvent(string eventId, object args);

    void _LoadSettings();

    void _SaveSettings();

    void OnPluginDestroyForHotReload();
  }
}
