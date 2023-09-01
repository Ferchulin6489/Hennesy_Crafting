// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Abstract.BaseIcon
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;

namespace ExileCore.Shared.Abstract
{
  public abstract class BaseIcon
  {
    protected static readonly Dictionary<string, Size2> strongboxesUV = new Dictionary<string, Size2>()
    {
      {
        "Metadata/Chests/StrongBoxes/Large",
        new Size2(7, 7)
      },
      {
        "Metadata/Chests/StrongBoxes/Strongbox",
        new Size2(1, 2)
      },
      {
        "Metadata/Chests/StrongBoxes/Armory",
        new Size2(2, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/Arsenal",
        new Size2(4, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/Artisan",
        new Size2(3, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/Jeweller",
        new Size2(1, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/Cartographer",
        new Size2(5, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/CartographerLowMaps",
        new Size2(5, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/CartographerMidMaps",
        new Size2(5, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/CartographerHighMaps",
        new Size2(5, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/Ornate",
        new Size2(7, 7)
      },
      {
        "Metadata/Chests/StrongBoxes/Arcanist",
        new Size2(1, 8)
      },
      {
        "Metadata/Chests/StrongBoxes/Gemcutter",
        new Size2(6, 1)
      },
      {
        "Metadata/Chests/StrongBoxes/StrongboxDivination",
        new Size2(7, 1)
      },
      {
        "Metadata/Chests/AbyssChest",
        new Size2(7, 7)
      }
    };
    protected static readonly Dictionary<string, Color> FossilRarity = new Dictionary<string, Color>()
    {
      {
        "Fractured",
        Color.Aquamarine
      },
      {
        "Faceted",
        Color.Aquamarine
      },
      {
        "Glyphic",
        Color.Aquamarine
      },
      {
        "Hollow",
        Color.Aquamarine
      },
      {
        "Shuddering",
        Color.Aquamarine
      },
      {
        "Bloodstained",
        Color.Aquamarine
      },
      {
        "Tangled",
        Color.OrangeRed
      },
      {
        "Dense",
        Color.OrangeRed
      },
      {
        "Gilded",
        Color.OrangeRed
      },
      {
        "Sanctified",
        Color.Aquamarine
      },
      {
        "Encrusted",
        Color.Yellow
      },
      {
        "Aetheric",
        Color.Orange
      },
      {
        "Enchanted",
        Color.Orange
      },
      {
        "Pristine",
        Color.Orange
      },
      {
        "Prismatic",
        Color.Orange
      },
      {
        "Corroded",
        Color.Yellow
      },
      {
        "Perfect",
        Color.Orange
      },
      {
        "Jagged",
        Color.Yellow
      },
      {
        "Serrated",
        Color.Yellow
      },
      {
        "Bound",
        Color.Yellow
      },
      {
        "Lucent",
        Color.Yellow
      },
      {
        "Metallic",
        Color.Yellow
      },
      {
        "Scorched",
        Color.Yellow
      },
      {
        "Aberrant",
        Color.Yellow
      },
      {
        "Frigid",
        Color.Yellow
      }
    };
    private readonly ISettings _settings;
    protected bool _HasIngameIcon;

    public BaseIcon(Entity entity, ISettings settings)
    {
      BaseIcon baseIcon = this;
      this._settings = settings;
      this.Entity = entity;
      if (this._settings == null || this.Entity == null)
        return;
      this.Rarity = this.Entity.Rarity;
      switch (this.Rarity)
      {
        case MonsterRarity.White:
          this.Priority = IconPriority.Low;
          break;
        case MonsterRarity.Magic:
          this.Priority = IconPriority.Medium;
          break;
        case MonsterRarity.Rare:
          this.Priority = IconPriority.High;
          break;
        case MonsterRarity.Unique:
          this.Priority = IconPriority.Critical;
          break;
        default:
          this.Priority = IconPriority.Critical;
          break;
      }
      this.Show = (Func<bool>) (() => baseIcon.Entity.IsValid);
      this.Hidden = (Func<bool>) (() => entity.IsHidden);
      this.GridPositionNum = (Func<System.Numerics.Vector2>) (() => baseIcon.Entity.GridPosNum);
      if (!this.Entity.HasComponent<MinimapIcon>())
        return;
      string name = this.Entity.GetComponent<MinimapIcon>().Name;
      if (string.IsNullOrEmpty(name))
        return;
      MapIconsIndex index = Extensions.IconIndexByName(name);
      if (index != MapIconsIndex.MyPlayer)
      {
        this.MainTexture = new HudTexture("Icons.png")
        {
          UV = SpriteHelper.GetUV(index),
          Size = 16f
        };
        this._HasIngameIcon = true;
      }
      if (this.Entity.HasComponent<Portal>() && this.Entity.HasComponent<Transitionable>())
      {
        Transitionable transitionable = this.Entity.GetComponent<Transitionable>();
        this.Text = this.RenderName;
        this.Show = (Func<bool>) (() => closure_0.Entity.IsValid && transitionable.Flag1 == (byte) 2);
      }
      else if (this.Entity.Path.StartsWith("Metadata/Terrain/Labyrinth/Objects/Puzzle_Parts/Switch", StringComparison.Ordinal))
        this.Show = (Func<bool>) (() =>
        {
          Transitionable component1 = baseIcon.Entity.GetComponent<Transitionable>();
          MinimapIcon component2 = baseIcon.Entity.GetComponent<MinimapIcon>();
          return baseIcon.Entity.IsValid && component2.IsVisible && !component2.IsHide && component1.Flag1 != (byte) 2;
        });
      else if (this.Entity.Path.StartsWith("Metadata/MiscellaneousObjects/Abyss/Abyss"))
        this.Show = (Func<bool>) (() =>
        {
          MinimapIcon component = baseIcon.Entity.GetComponent<MinimapIcon>();
          if (!baseIcon.Entity.IsValid || !component.IsVisible)
            return false;
          return !component.IsHide || baseIcon.Entity.GetComponent<Transitionable>().Flag1 == (byte) 1;
        });
      else if (entity.Path.Contains("Metadata/Terrain/Leagues/Delve/Objects/DelveMineral"))
      {
        this.Priority = IconPriority.High;
        this.MainTexture.UV = SpriteHelper.GetUV(MapIconsIndex.DelveMineralVein);
        this.Text = "Sulphite";
        this.Show = (Func<bool>) (() => entity.IsValid && entity.IsTargetable);
      }
      else
        this.Show = (Func<bool>) (() =>
        {
          MinimapIcon component = baseIcon.Entity.GetComponent<MinimapIcon>();
          return component != null && component.IsVisible && !component.IsHide;
        });
    }

    public bool HasIngameIcon => this._HasIngameIcon;

    public Entity Entity { get; }

    [Obsolete]
    public Func<SharpDX.Vector2> GridPosition
    {
      get => (Func<SharpDX.Vector2>) (() => this.GridPositionNum().ToSharpDx());
      set => this.GridPositionNum = (Func<System.Numerics.Vector2>) (() => value().ToVector2Num());
    }

    public Func<System.Numerics.Vector2> GridPositionNum { get; set; }

    public RectangleF DrawRect { get; set; }

    public Func<bool> Show { get; set; }

    public Func<bool> Hidden { get; protected set; } = (Func<bool>) (() => false);

    public HudTexture MainTexture { get; protected set; }

    public IconPriority Priority { get; protected set; }

    public MonsterRarity Rarity { get; protected set; }

    public string Text { get; protected set; }

    public string RenderName => this.Entity.RenderName;

    protected bool PathCheck(Entity path, params string[] check)
    {
      foreach (string str in check)
      {
        if (path.Path.Equals(str, StringComparison.Ordinal))
          return true;
      }
      return false;
    }
  }
}
