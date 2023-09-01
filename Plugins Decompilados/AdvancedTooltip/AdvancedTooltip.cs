// Decompiled with JetBrains decompiler
// Type: AdvancedTooltip.AdvancedTooltip
// Assembly: AdvancedTooltip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 655408A3-EFFF-42CC-9A14-FFB321F3C387
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\AdvancedTooltip\AdvancedTooltip.dll

using AdvancedTooltip.Settings;
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Nodes;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdvancedTooltip
{
  public class AdvancedTooltip : BaseSettingsPlugin<AdvancedTooltipSettings>
  {
    private Dictionary<int, Color> TColors;
    private FastModsModule _fastMods;

    public virtual void OnLoad()
    {
      this.Graphics.InitImage("menu-colors.png", true);
      this.Graphics.InitImage("preload-end.png", true);
    }

    public virtual bool Initialise()
    {
      this._fastMods = new FastModsModule(this.Graphics, this.Settings.ItemMods);
      this.TColors = new Dictionary<int, Color>()
      {
        {
          1,
          ColorNode.op_Implicit(this.Settings.ItemMods.T1Color)
        },
        {
          2,
          ColorNode.op_Implicit(this.Settings.ItemMods.T2Color)
        },
        {
          3,
          ColorNode.op_Implicit(this.Settings.ItemMods.T3Color)
        }
      };
      return true;
    }

    public virtual void Render()
    {
      HoverItemIcon hoverItemIcon = ((RemoteMemoryObject) this.GameController?.Game?.IngameState?.UIHover)?.AsObject<HoverItemIcon>();
      if (hoverItemIcon == null || hoverItemIcon.ToolTipType == null)
        return;
      TooltipItemFrameElement itemFrame = hoverItemIcon.ItemFrame;
      if (itemFrame == null)
        return;
      Entity poeEntity = hoverItemIcon.Item;
      Mods modsComponent = poeEntity?.GetComponent<Mods>();
      if (ToggleNode.op_Implicit(this.Settings.ItemMods.EnableFastMods) && (modsComponent == null || modsComponent.ItemRarity == 1 || modsComponent.ItemRarity == 2))
        this._fastMods.DrawUiHoverFastMods((Element) itemFrame);
      if (poeEntity == null || ((RemoteMemoryObject) poeEntity).Address == 0L)
        return;
      RectangleF clientRect = ((Element) itemFrame).GetClientRect();
      List<ItemMod> itemMods = modsComponent?.ItemMods;
      if (itemMods == null || ((IEnumerable<ItemMod>) itemMods).Any<ItemMod>((Func<ItemMod, bool>) (x => string.IsNullOrEmpty(x.RawName) && string.IsNullOrEmpty(x.Name))))
        return;
      List<ModValue> list = ((IEnumerable<ItemMod>) itemMods).Select<ItemMod, ModValue>((Func<ItemMod, ModValue>) (item => new ModValue(item, this.GameController.Files, modsComponent.ItemLevel, this.GameController.Files.BaseItemTypes.Translate(poeEntity.Path)))).ToList<ModValue>();
      System.Numerics.Vector2 num1 = ConvertHelper.TranslateToNum(clientRect.TopLeft, 20f, 56f);
      int num2 = list.Count<ModValue>((Func<ModValue, bool>) (item => item.CouldHaveTiers() && item.Tier == 1));
      int num3 = list.Count<ModValue>((Func<ModValue, bool>) (item => item.CouldHaveTiers() && item.Tier == 2));
      int num4 = list.Count<ModValue>((Func<ModValue, bool>) (item => item.CouldHaveTiers() && item.Tier == 3));
      float y1 = this.Graphics.MeasureText("T").Y * (float) (Math.Sign(num2) + Math.Sign(num3) + Math.Sign(num4));
      float x1 = this.Graphics.MeasureText("T1 x6").X;
      this.Graphics.DrawBox(num1, num1 + new System.Numerics.Vector2(x1, y1), Color.Black, 0.0f);
      DefaultInterpolatedStringHandler interpolatedStringHandler;
      if (num2 > 0)
      {
        ref float local = ref num1.Y;
        double num5 = (double) local;
        Graphics graphics = this.Graphics;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
        interpolatedStringHandler.AppendLiteral("T1 x");
        interpolatedStringHandler.AppendFormatted<int>(num2);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        System.Numerics.Vector2 vector2 = num1;
        Color color = ColorNode.op_Implicit(this.Settings.ItemMods.T1Color);
        double y2 = (double) graphics.DrawText(stringAndClear, vector2, color).Y;
        local = (float) (num5 + y2);
      }
      if (num3 > 0)
      {
        ref float local = ref num1.Y;
        double num6 = (double) local;
        Graphics graphics = this.Graphics;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
        interpolatedStringHandler.AppendLiteral("T2 x");
        interpolatedStringHandler.AppendFormatted<int>(num3);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        System.Numerics.Vector2 vector2 = num1;
        Color color = ColorNode.op_Implicit(this.Settings.ItemMods.T2Color);
        double y3 = (double) graphics.DrawText(stringAndClear, vector2, color).Y;
        local = (float) (num6 + y3);
      }
      if (num4 > 0)
      {
        ref float local = ref num1.Y;
        double num7 = (double) local;
        Graphics graphics = this.Graphics;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
        interpolatedStringHandler.AppendLiteral("T3 x");
        interpolatedStringHandler.AppendFormatted<int>(num4);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        System.Numerics.Vector2 vector2 = num1;
        Color color = ColorNode.op_Implicit(this.Settings.ItemMods.T3Color);
        double y4 = (double) graphics.DrawText(stringAndClear, vector2, color).Y;
        local = (float) (num7 + y4);
      }
      if (this.Settings.ItemLevel.Enable.Value)
      {
        Mods mods = modsComponent;
        string str = Convert.ToString(mods != null ? mods.ItemLevel : 0);
        int num8 = RangeNode<int>.op_Implicit(this.Settings.ItemLevel.TextSize) + 10;
        this.Graphics.DrawImage("menu-colors.png", new RectangleF(clientRect.TopLeft.X - 2f, clientRect.TopLeft.Y - 2f, (float) num8, (float) num8), ColorNode.op_Implicit(this.Settings.ItemLevel.BackgroundColor));
        this.Graphics.DrawText(str, ConvertHelper.TranslateToNum(clientRect.TopLeft, 2f, 2f), ColorNode.op_Implicit(this.Settings.ItemLevel.TextColor));
      }
      if (this.Settings.ItemMods.Enable.Value)
      {
        float y5 = clientRect.Bottom + 5f;
        System.Numerics.Vector2 seed = new System.Numerics.Vector2(clientRect.X + 20f, y5 + 4f);
        float height = list.Where<ModValue>((Func<ModValue, bool>) (x => ((IEnumerable<StatsDat.StatRecord>) x.Record.StatNames).Count<StatsDat.StatRecord>((Func<StatsDat.StatRecord, bool>) (y => y != null)) > 0)).Aggregate<ModValue, System.Numerics.Vector2>(seed, (Func<System.Numerics.Vector2, ModValue, System.Numerics.Vector2>) ((position, item) => this.DrawMod(item, position))).Y - y5;
        if ((double) height > 4.0)
          this.Graphics.DrawBox(new RectangleF(clientRect.X + 1f, y5, clientRect.Width, height), ColorNode.op_Implicit(this.Settings.ItemMods.BackgroundColor));
      }
      Weapon weaponComponent;
      if (!ToggleNode.op_Implicit(this.Settings.WeaponDps.Enable) || !poeEntity.TryGetComponent<Weapon>(ref weaponComponent))
        return;
      this.DrawWeaponDps(clientRect, poeEntity, list, weaponComponent);
    }

    private System.Numerics.Vector2 DrawMod(ModValue item, System.Numerics.Vector2 position)
    {
      System.Numerics.Vector2 vector2_1 = position;
      ItemModsSettings itemMods = this.Settings.ItemMods;
      (string, Color) valueTuple;
      switch (item.AffixType - 1)
      {
        case 0:
          valueTuple = ("[P]", itemMods.PrefixColor.Value);
          break;
        case 1:
          valueTuple = ("[S]", itemMods.SuffixColor.Value);
          break;
        case 2:
          valueTuple = ("[U]", new Color((int) byte.MaxValue, 140, 0));
          break;
        case 3:
          valueTuple = ("[NEM]", new Color((int) byte.MaxValue, 20, 147));
          break;
        case 4:
          valueTuple = ("[C]", new Color(220, 20, 60));
          break;
        case 5:
          valueTuple = ("[BLD]", new Color(0, 128, 0));
          break;
        case 6:
          valueTuple = ("[TOR]", new Color(178, 34, 34));
          break;
        case 7:
          valueTuple = ("[TEM]", new Color(65, 105, 225));
          break;
        case 8:
          valueTuple = ("[TAL]", new Color(218, 165, 32));
          break;
        case 9:
          valueTuple = ("[E]", new Color((int) byte.MaxValue, 0, (int) byte.MaxValue));
          break;
        case 10:
          valueTuple = ("[ESS]", new Color(139, 0, 139));
          break;
        case 12:
          valueTuple = ("[BES]", new Color((int) byte.MaxValue, 99, 71));
          break;
        case 13:
          valueTuple = ("[DEL]", new Color(47, 79, 79));
          break;
        case 14:
          valueTuple = ("[SYN]", new Color((int) byte.MaxValue, 105, 180));
          break;
        case 15:
          valueTuple = ("[SGS]", new Color(186, 85, 211));
          break;
        case 16:
          valueTuple = ("[SYB]", new Color(100, 149, 237));
          break;
        case 17:
          valueTuple = ("[BLI]", new Color(0, 100, 0));
          break;
        case 18:
          valueTuple = ("[BLT]", new Color(0, 100, 0));
          break;
        case 19:
          valueTuple = ("[MAF]", new Color(123, 104, 238));
          break;
        case 20:
          valueTuple = ("[FEE]", new Color((int) byte.MaxValue, 165, 0));
          break;
        case 21:
          valueTuple = ("[FEI]", new Color((int) byte.MaxValue, 165, 0));
          break;
        case 22:
          valueTuple = ("[LOG]", new Color(218, 165, 32));
          break;
        case 23:
          valueTuple = ("[SCU]", new Color(218, 165, 32));
          break;
        case 24:
          valueTuple = ("[SCD]", new Color(218, 165, 32));
          break;
        case 25:
          valueTuple = ("[SCM]", new Color(218, 165, 32));
          break;
        case 27:
          valueTuple = ("[EXI]", new Color((int) byte.MaxValue, 69, 0));
          break;
        case 28:
          valueTuple = ("[EAT]", new Color((int) byte.MaxValue, 69, 0));
          break;
        case 30:
          valueTuple = ("[CRU]", new Color(254, 114, 53));
          break;
        case 31:
          valueTuple = ("[CRC]", new Color(254, 114, 53));
          break;
        default:
          valueTuple = ("[?]", new Color(211, 211, 211));
          break;
      }
      (string str1, Color color1) = valueTuple;
      float x1 = this.Graphics.DrawText(str1, position, color1).X;
      if (item.AffixType != 3)
      {
        int totalTiers = item.TotalTiers;
        ModType affixType1 = item.AffixType;
        bool flag = totalTiers > 0;
        Color color2 = affixType1 == 1 ? (flag ? this.TColors.GetValueOrDefault<int, Color>(item.Tier, ColorNode.op_Implicit(itemMods.PrefixColor)) : ColorNode.op_Implicit(itemMods.PrefixColor)) : (affixType1 == 2 ? (flag ? this.TColors.GetValueOrDefault<int, Color>(item.Tier, ColorNode.op_Implicit(itemMods.SuffixColor)) : ColorNode.op_Implicit(itemMods.SuffixColor)) : new Color());
        string str2;
        if (totalTiers <= 0)
        {
          str2 = string.Empty;
        }
        else
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
          interpolatedStringHandler.AppendLiteral(" T");
          interpolatedStringHandler.AppendFormatted<int>(item.Tier);
          interpolatedStringHandler.AppendLiteral("(");
          interpolatedStringHandler.AppendFormatted<int>(totalTiers);
          interpolatedStringHandler.AppendLiteral(") ");
          str2 = interpolatedStringHandler.ToStringAndClear();
        }
        int length = " T11(11) ".Length;
        string str3 = str2.PadLeft(length);
        ModType affixType2 = item.AffixType;
        System.Numerics.Vector2 vector2_2 = affixType2 == 1 ? this.Graphics.DrawText(str3, MathHepler.Translate(position, x1, 0.0f), ColorNode.op_Implicit(itemMods.PrefixColor)) : (affixType2 == 2 ? this.Graphics.DrawText(str3, MathHepler.Translate(position, x1, 0.0f), ColorNode.op_Implicit(itemMods.SuffixColor)) : new System.Numerics.Vector2());
        System.Numerics.Vector2 vector2_3 = ToggleNode.op_Implicit(this.Settings.ItemMods.ShowModNames) ? this.Graphics.DrawText(item.AffixText, MathHepler.Translate(position, x1 + vector2_2.X, 0.0f), color2) : System.Numerics.Vector2.Zero;
        if (ToggleNode.op_Implicit(this.Settings.ItemMods.StartStatsOnSameLine))
          position.X += vector2_3.X + vector2_2.X;
        else
          position.Y += Math.Max(vector2_3.Y, vector2_2.Y);
      }
      float x2 = this.Graphics.MeasureText("+12345").X;
      for (int index = 0; index < 4; ++index)
      {
        IntRange intRange = item.Record.StatRange[index];
        if (intRange.Min != 0 || intRange.Max != 0)
        {
          StatsDat.StatRecord statName = item.Record.StatNames[index];
          int num = item.StatValue[index];
          if (num > -1000 && statName != null)
          {
            string str4 = string.Format(!intRange.HasSpread() ? "{0}" : "[{1}] {0}", (object) statName, (object) intRange);
            string str5 = statName.ValueToString(num);
            System.Numerics.Vector2 vector2_4;
            if (item.AffixType == 3 || ToggleNode.op_Implicit(this.Settings.ItemMods.StartStatsOnSameLine))
            {
              vector2_4 = this.Graphics.DrawText(str5, MathHepler.Translate(position, x1 + x2, 0.0f), Color.Gainsboro, (FontAlign) 2);
              this.Graphics.DrawText(str4, MathHepler.Translate(position, (float) ((double) x1 + (double) x2 + 5.0), 0.0f), Color.Gainsboro);
            }
            else
            {
              vector2_4 = this.Graphics.DrawText(str5, MathHepler.Translate(position, x1, 0.0f), Color.Gainsboro, (FontAlign) 2);
              this.Graphics.DrawText(str4, MathHepler.Translate(position, 40f, 0.0f), Color.Gainsboro);
            }
            position.Y += vector2_4.Y;
          }
        }
      }
      if ((double) Math.Abs(position.Y - vector2_1.Y) <= 1.0 / 1000.0)
        return vector2_1;
      return vector2_1 with { Y = position.Y + 4f };
    }

    private void DrawWeaponDps(
      RectangleF clientRect,
      Entity itemEntity,
      List<ModValue> modValues,
      Weapon weaponComponent)
    {
      if (weaponComponent == null || !itemEntity.IsValid)
        return;
      float num1 = (float) Math.Round(1000.0 / (double) weaponComponent.AttackTime, 2);
      int length = Enum.GetValues(typeof (DamageType)).Length;
      float[] numArray = new float[length];
      float num2 = 1f;
      int damageMax = weaponComponent.DamageMax;
      int damageMin = weaponComponent.DamageMin;
      foreach (ModValue modValue in modValues)
      {
        for (int index = 0; index < 4; ++index)
        {
          IntRange intRange = modValue.Record.StatRange[index];
          if (intRange.Min != 0 || intRange.Max != 0)
          {
            StatsDat.StatRecord statName = modValue.Record.StatNames[index];
            if (statName != null)
            {
              int num3 = modValue.StatValue[index];
              string key = statName.Key;
              if (key != null)
              {
                switch (key.Length)
                {
                  case 18:
                    if (key == "physical_damage_+%")
                      break;
                    continue;
                  case 21:
                    if (key == "local_attack_speed_+%")
                    {
                      num1 *= (float) ((100.0 + (double) num3) / 100.0);
                      continue;
                    }
                    continue;
                  case 24:
                    if (key == "local_physical_damage_+%")
                      break;
                    continue;
                  case 31:
                    switch (key[7])
                    {
                      case 'a':
                        switch (key)
                        {
                          case "local_maximum_added_fire_damage":
                            goto label_34;
                          case "local_maximum_added_cold_damage":
                            goto label_35;
                          default:
                            continue;
                        }
                      case 'i':
                        switch (key)
                        {
                          case "local_minimum_added_fire_damage":
                            goto label_34;
                          case "local_minimum_added_cold_damage":
                            goto label_35;
                          default:
                            continue;
                        }
                      default:
                        continue;
                    }
                  case 32:
                    switch (key[7])
                    {
                      case 'a':
                        if (key == "local_maximum_added_chaos_damage")
                          goto label_37;
                        else
                          continue;
                      case 'i':
                        if (key == "local_minimum_added_chaos_damage")
                          goto label_37;
                        else
                          continue;
                      default:
                        continue;
                    }
                  case 35:
                    switch (key[7])
                    {
                      case 'a':
                        if (key == "local_maximum_added_physical_damage")
                        {
                          damageMax += num3;
                          continue;
                        }
                        continue;
                      case 'i':
                        if (key == "local_minimum_added_physical_damage")
                        {
                          damageMin += num3;
                          continue;
                        }
                        continue;
                      default:
                        continue;
                    }
                  case 36:
                    switch (key[7])
                    {
                      case 'a':
                        if (key == "local_maximum_added_lightning_damage")
                          break;
                        continue;
                      case 'i':
                        if (key == "local_minimum_added_lightning_damage")
                          break;
                        continue;
                      default:
                        continue;
                    }
                    numArray[3] += (float) num3;
                    continue;
                  case 55:
                    switch (key[14])
                    {
                      case 'a':
                        if (key == "unique_local_maximum_added_cold_damage_when_in_off_hand")
                          goto label_35;
                        else
                          continue;
                      case 'i':
                        if (key == "unique_local_minimum_added_cold_damage_when_in_off_hand")
                          goto label_35;
                        else
                          continue;
                      default:
                        continue;
                    }
                  case 56:
                    switch (key[14])
                    {
                      case 'a':
                        switch (key)
                        {
                          case "unique_local_maximum_added_fire_damage_when_in_main_hand":
                            goto label_34;
                          case "unique_local_maximum_added_chaos_damage_when_in_off_hand":
                            goto label_37;
                          default:
                            continue;
                        }
                      case 'i':
                        switch (key)
                        {
                          case "unique_local_minimum_added_fire_damage_when_in_main_hand":
                            goto label_34;
                          case "unique_local_minimum_added_chaos_damage_when_in_off_hand":
                            goto label_37;
                          default:
                            continue;
                        }
                      default:
                        continue;
                    }
                  default:
                    continue;
                }
                num2 += (float) num3 / 100f;
                continue;
label_34:
                numArray[1] += (float) num3;
                continue;
label_35:
                numArray[2] += (float) num3;
                continue;
label_37:
                numArray[4] += (float) num3;
              }
            }
          }
        }
      }
      WeaponDpsSettings weaponDps = this.Settings.WeaponDps;
      Color[] colorArray = new Color[5]
      {
        Color.White,
        ColorNode.op_Implicit(weaponDps.DmgFireColor),
        ColorNode.op_Implicit(weaponDps.DmgColdColor),
        ColorNode.op_Implicit(weaponDps.DmgLightningColor),
        ColorNode.op_Implicit(weaponDps.DmgChaosColor)
      };
      Quality component = itemEntity.GetComponent<Quality>();
      if (component == null)
        return;
      float num4 = num2 + (float) component.ItemQuality / 100f;
      int num5 = (int) Math.Round((double) damageMin * (double) num4);
      int num6 = (int) Math.Round((double) damageMax * (double) num4);
      numArray[0] = (float) (num5 + num6);
      float num7 = (float) Math.Round((double) num1, 2);
      float num8 = numArray[0] / 2f * num7;
      float num9 = 0.0f;
      int num10 = 0;
      Color color = ColorNode.op_Implicit(weaponDps.PhysicalDamageColor);
      for (int index = 1; index < length; ++index)
      {
        num9 += numArray[index] / 2f * num7;
        if ((double) numArray[index] > 0.0)
        {
          if (num10 == 0)
          {
            num10 = index;
            color = colorArray[index];
          }
          else
            color = ColorNode.op_Implicit(weaponDps.ElementalDamageColor);
        }
      }
      System.Numerics.Vector2 vector2_1 = new System.Numerics.Vector2(clientRect.Right - 15f, clientRect.Y + 1f);
      System.Numerics.Vector2 vector2_2 = (double) num8 > 0.0 ? this.Graphics.DrawText(num8.ToString("#.#"), vector2_1, (FontAlign) 2) : System.Numerics.Vector2.Zero;
      System.Numerics.Vector2 vector2_3 = (double) num9 > 0.0 ? this.Graphics.DrawText(num9.ToString("#.#"), MathHepler.Translate(vector2_1, 0.0f, vector2_2.Y), color, (FontAlign) 2) : System.Numerics.Vector2.Zero;
      float num11 = num8 + num9;
      System.Numerics.Vector2 vector2_4 = (double) num11 > 0.0 ? this.Graphics.DrawText(num11.ToString("#.#"), MathHepler.Translate(vector2_1, 0.0f, vector2_2.Y + vector2_3.Y), Color.White, (FontAlign) 2) : System.Numerics.Vector2.Zero;
      this.Graphics.DrawText("dps", MathHepler.Translate(vector2_1, 0.0f, vector2_2.Y + vector2_3.Y + vector2_4.Y), ColorNode.op_Implicit(weaponDps.TextColor), (FontAlign) 2);
      this.Graphics.DrawImage("preload-end.png", new RectangleF(vector2_1.X - 100f, vector2_1.Y - 6f, 100f, 78f), ColorNode.op_Implicit(weaponDps.BackgroundColor));
    }
  }
}
