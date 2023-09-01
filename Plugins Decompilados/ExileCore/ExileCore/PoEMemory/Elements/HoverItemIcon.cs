// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.HoverItemIcon
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using Serilog;
using System;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Elements
{
  public class HoverItemIcon : Element
  {
    private const int HoveredItemTooltipOffset = 520;
    private const int ChatEntityArrayOffset = 1736;
    private ToolTipType? _tooltipType;

    [Obsolete("Use Element.Tooltip")]
    public Element InventoryItemTooltip => base.Tooltip;

    [Obsolete("Use Element.Tooltip")]
    public Element ItemInChatTooltip => base.Tooltip;

    public ItemOnGroundTooltip ToolTipOnGround => this.TheGame.IngameState.IngameUi.ItemOnGroundTooltip;

    [Obsolete]
    public int InventPosX => this.AsObject<NormalInventoryItem>().InventPosX;

    [Obsolete]
    public int InventPosY => this.AsObject<NormalInventoryItem>().InventPosY;

    public ToolTipType ToolTipType
    {
      get
      {
        try
        {
          ToolTipType valueOrDefault = this._tooltipType.GetValueOrDefault();
          int toolTipType1;
          if (!this._tooltipType.HasValue)
          {
            ToolTipType toolTipType2 = this.GetToolTipType();
            this._tooltipType = new ToolTipType?(toolTipType2);
            toolTipType1 = (int) toolTipType2;
          }
          else
            toolTipType1 = (int) valueOrDefault;
          return (ToolTipType) toolTipType1;
        }
        catch (Exception ex)
        {
          Core.Logger?.Error(ex.Message + " " + ex.StackTrace);
          return ToolTipType.None;
        }
      }
    }

    public new Element Tooltip
    {
      get
      {
        Element tooltip;
        switch (this.ToolTipType)
        {
          case ToolTipType.InventoryItem:
            tooltip = base.Tooltip;
            break;
          case ToolTipType.ItemOnGround:
            tooltip = this.ToolTipOnGround.Tooltip;
            break;
          case ToolTipType.ItemInChat:
            tooltip = base.Tooltip[0];
            break;
          default:
            tooltip = (Element) null;
            break;
        }
        return tooltip;
      }
    }

    public TooltipItemFrameElement ItemFrame
    {
      get
      {
        Element element;
        switch (this.ToolTipType)
        {
          case ToolTipType.InventoryItem:
            element = base.Tooltip[0];
            break;
          case ToolTipType.ItemOnGround:
            element = this.ToolTipOnGround.ItemFrame;
            break;
          case ToolTipType.ItemInChat:
            element = base.Tooltip.GetChildFromIndices(0, 1);
            break;
          default:
            element = (Element) null;
            break;
        }
        return element?.AsObject<TooltipItemFrameElement>();
      }
    }

    public Element Item2DIcon
    {
      get
      {
        Element item2Dicon;
        switch (this.ToolTipType)
        {
          case ToolTipType.InventoryItem:
            item2Dicon = (Element) null;
            break;
          case ToolTipType.ItemOnGround:
            item2Dicon = this.ToolTipOnGround.Item2DIcon;
            break;
          case ToolTipType.ItemInChat:
            item2Dicon = base.Tooltip.GetChildFromIndices(new int[2]);
            break;
          default:
            item2Dicon = (Element) null;
            break;
        }
        return item2Dicon;
      }
    }

    public Entity Item
    {
      get
      {
        switch (this.ToolTipType)
        {
          case ToolTipType.InventoryItem:
            return this.Entity;
          case ToolTipType.ItemOnGround:
            return this.TheGame.IngameState.IngameUi.ItemsOnGroundLabelElement?.ItemOnHover?.GetComponent<WorldItem>()?.ItemEntity;
          case ToolTipType.ItemInChat:
            return this.Parent.ReadObjectAt<Entity>(1736);
          default:
            return (Entity) null;
        }
      }
    }

    private ToolTipType GetToolTipType()
    {
      try
      {
        Element tooltip = base.Tooltip;
        if (tooltip != null && tooltip.IsVisible)
          return tooltip.ReadObjectAt<Element>(520).Address == tooltip[0].Address ? ToolTipType.InventoryItem : ToolTipType.ItemInChat;
        ItemOnGroundTooltip toolTipOnGround = this.ToolTipOnGround;
        if (toolTipOnGround != null)
        {
          if (toolTipOnGround.Tooltip != null)
          {
            Element tooltipUi = toolTipOnGround.TooltipUI;
            if (tooltipUi != null)
            {
              if (tooltipUi.IsVisible)
                return ToolTipType.ItemOnGround;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ILogger logger = Core.Logger;
        if (logger != null)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
          interpolatedStringHandler.AppendLiteral("HoverItemIcon.cs -> ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          logger.Error(interpolatedStringHandler.ToStringAndClear());
        }
      }
      return ToolTipType.None;
    }
  }
}
