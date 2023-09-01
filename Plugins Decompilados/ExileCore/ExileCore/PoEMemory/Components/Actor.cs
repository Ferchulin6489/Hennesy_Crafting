// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Actor
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public class Actor : Component
  {
    private readonly CachedValue<ActorComponentOffsets> _cacheValue;
    private readonly CachedValue<AnimationController> _animationController;

    public Actor()
    {
      this._cacheValue = (CachedValue<ActorComponentOffsets>) new FrameCache<ActorComponentOffsets>((Func<ActorComponentOffsets>) (() => this.M.Read<ActorComponentOffsets>(this.Address)));
      this._animationController = (CachedValue<AnimationController>) KeyTrackingCache.Create<AnimationController, long>((Func<AnimationController>) (() => this.GetObject<AnimationController>(this.Struct.AnimationControllerPtr)), (Func<long>) (() => this.Struct.AnimationControllerPtr));
    }

    private ActorComponentOffsets Struct => this._cacheValue.Value;

    public short ActionId => this.Address == 0L ? (short) 0 : this.Struct.ActionId;

    public ActionFlags Action => this.Address == 0L ? ActionFlags.None : (ActionFlags) this.Struct.ActionId;

    public bool isAttacking => (this.Action & ActionFlags.UsingAbility) > ActionFlags.None;

    public int AnimationId => this.Address == 0L ? 0 : this.Struct.AnimationId;

    public AnimationE Animation => this.Address == 0L ? AnimationE.Idle : (AnimationE) this.Struct.AnimationId;

    public AnimationController AnimationController => this._animationController.Value;

    public Actor.ActionWrapper CurrentAction => this.Struct.ActionPtr <= 0L ? (Actor.ActionWrapper) null : this.GetObject<Actor.ActionWrapper>(this.Struct.ActionPtr).SetActor(this);

    public bool isMoving => (this.Action & ActionFlags.Moving) > ActionFlags.None || this.CurrentAction != null && this.CurrentAction.Skill.Name == "Cyclone";

    public long DeployedObjectsCount => this.Struct.DeployedObjectArray.Size / 8L;

    public List<DeployedObject> DeployedObjects
    {
      get
      {
        List<DeployedObject> deployedObjects = new List<DeployedObject>();
        if ((this.Struct.DeployedObjectArray.Last - this.Struct.DeployedObjectArray.First) / 8L > 300L)
          return deployedObjects;
        for (long first = this.Struct.DeployedObjectArray.First; first < this.Struct.DeployedObjectArray.Last; first += 8L)
          deployedObjects.Add(this.GetObject<DeployedObject>(first));
        return deployedObjects;
      }
    }

    public List<ActorSkill> ActorSkills
    {
      get
      {
        long first = this.Struct.ActorSkillsArray.First;
        long last = this.Struct.ActorSkillsArray.Last;
        long num = first + 8L;
        if ((last - num) / 16L > 50L)
          return new List<ActorSkill>();
        List<ActorSkill> actorSkills = new List<ActorSkill>();
        for (long addressPointer = num; addressPointer < last; addressPointer += 16L)
          actorSkills.Add(this.ReadObject<ActorSkill>(addressPointer).SetActor(this));
        return actorSkills;
      }
    }

    public List<ActorSkillCooldown> ActorSkillsCooldowns => this.M.ReadStructsArray<ActorSkillCooldown>(this.Struct.ActorSkillsCooldownArray.First, this.Struct.ActorSkillsCooldownArray.Last, 72, (RemoteMemoryObject) null);

    public List<ActorVaalSkill> ActorVaalSkills => this.M.ReadStructsArray<ActorVaalSkill>(this.Struct.ActorVaalSkills.First, this.Struct.ActorVaalSkills.Last, 32, (RemoteMemoryObject) null);

    [Obsolete("Use ActorSkillsCooldowns")]
    public IEnumerable<long> SkillUiStateOffsets => this.ActorSkillsCooldowns.Select<ActorSkillCooldown, long>((Func<ActorSkillCooldown, long>) (x => x.Address));

    public class ActionWrapper : RemoteMemoryObject
    {
      private readonly FrameCache<ActionWrapperOffsets> cacheValue;
      private Actor _actor;

      public ActionWrapper() => this.cacheValue = new FrameCache<ActionWrapperOffsets>((Func<ActionWrapperOffsets>) (() => this.M.Read<ActionWrapperOffsets>(this.Address)));

      public Actor.ActionWrapper SetActor(Actor actor)
      {
        this._actor = actor;
        return this;
      }

      private ActionWrapperOffsets Struct => this.cacheValue.Value;

      public int DestinationX => this.Struct.Destination.X;

      public int DestinationY => this.Struct.Destination.Y;

      public Vector2i Destination => this.Struct.Destination;

      public Entity Target => this.GetObject<Entity>(this.Struct.Target);

      [Obsolete("Use Destination instead")]
      public Vector2i CastDestination => this.Destination;

      public ActorSkill Skill => this.GetObject<ActorSkill>(this.Struct.Skill).SetActor(this._actor);
    }
  }
}
