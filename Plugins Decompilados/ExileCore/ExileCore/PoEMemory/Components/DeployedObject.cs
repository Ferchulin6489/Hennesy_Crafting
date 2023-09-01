// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.DeployedObject
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class DeployedObject : RemoteMemoryObject
  {
    private readonly FrameCache<ActorDeployedObject> cacheValue;
    private Entity _entity;

    public DeployedObject() => this.cacheValue = new FrameCache<ActorDeployedObject>((Func<ActorDeployedObject>) (() => this.M.Read<ActorDeployedObject>(this.Address)));

    private ActorDeployedObject Struct => this.cacheValue.Value;

    public ushort ObjectId => this.Struct.ObjectId;

    public ushort SkillKey => this.Struct.SkillId;

    public Entity Entity => this._entity ?? (this._entity = EntityListWrapper.GetEntityById((uint) this.ObjectId));
  }
}
