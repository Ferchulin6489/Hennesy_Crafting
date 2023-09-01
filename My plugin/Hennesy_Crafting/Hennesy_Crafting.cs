using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;
using System.Collections.Generic;
using System;
using Vector2 = System.Numerics.Vector2;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Components;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Nodes;
using Hennesy_Crafting.Settings;
using System.Linq;
using System.Runtime.CompilerServices;
using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.Shared.Enums;
using ExileCore.Shared;

namespace Hennesy_Crafting
{
    public class Hennesy_Crafting : BaseSettingsPlugin<Hennesy_CraftingSettings>
    {
        //Declas
        private Inventory _currentOpenedStashTab;
        private string _currentOpenedStashTabName;
        private StashData _sData;
        private string _drawInfoString = "";
        private FastModsModule _fastMods;
        private Dictionary<int, Color> TColors;

        //For Testing
        public string data1_ = "";
        public override void OnLoad()
        {
            this.Graphics.InitImage("menu-colors.png", true);
            this.Graphics.InitImage("preload-end.png", true);
        }
        public override bool Initialise()
        {
            //Perform one-time initialization here

            //Maybe load you custom config (only do so if builtin settings are inadequate for the job)
            //var configPath = Path.Join(ConfigDirectory, "custom_config.txt");
            //if (File.Exists(configPath))
            //{
            //    var data = File.ReadAllText(configPath);
            //}
            this._fastMods = new FastModsModule(this.Graphics, this.Settings.ItemMods);
            this.TColors = new Dictionary<int, Color>()
            {
                {
                    1, this.Settings.ItemMods.T1Color
                },
                {
                    2, this.Settings.ItemMods.T2Color
                },
                {
                    3, this.Settings.ItemMods.T3Color
                }
            };
            return true;
        }

        public override void AreaChange(AreaInstance area)
        {
            //Perform once-per-zone processing here
            //For example, Radar builds the zone map texture here
        }

        public override Job Tick()
        {
            //Perform non-render-related work here, e.g. position calculation.
            //This method is still called on every frame, so to really gain
            //an advantage over just throwing everything in the Render method
            //you have to return a custom job, but this is a bit of an advanced technique
            //here's how, just in case:
            //return new Job($"{nameof(Hennesy_Crafting)}MainJob", () =>
            //{
            //    var a = Math.Sqrt(7);
            //});

            //otherwise, just run your code here
            //var a = Math.Sqrt(7);
            return null;
        }

        public override void Render()
        {
            //Any Imgui or Graphics calls go here. This is called after Tick
            //Graphics.DrawText($"Plugin {GetType().Name} is working.", new Vector2(100, 70), Color.Red);

            //IngameUIElements ingameUi = this.GameController.Game.IngameState.IngameUi;
            //ingameUi.InventoryPanel.

            //Test1
            if (this.Settings.Ver_Items_Mods)
                Draw_ItemLevel();

            //if (this.Settings.Ver_HUD)
                //Draw_HUD();
                //Graphics.DrawText($"Debug: [{_fastMods.data2.ToString()}]", new Vector2(100, 150), Color.White);
        }
        private void Draw_ItemLevel()
        {
            HoverItemIcon hoverItemIcon = ((RemoteMemoryObject)this.GameController?.Game?.IngameState?.UIHover)?.AsObject<HoverItemIcon>();
            if (hoverItemIcon == null || hoverItemIcon.ToolTipType == null)
                return;
            TooltipItemFrameElement itemFrame = hoverItemIcon.ItemFrame;
            if (itemFrame == null)
                return;



            Entity poeEntity = hoverItemIcon.Item;
            Mods modsComponent = poeEntity?.GetComponent<Mods>();

            if (this.Settings.ItemMods.EnableFastMods && (modsComponent == null || (int)modsComponent.ItemRarity == 1 ||
                (int)modsComponent.ItemRarity == 2))
                this._fastMods.DrawUiHoverFastMods((Element)itemFrame);
            if (poeEntity == null || ((RemoteMemoryObject)poeEntity).Address == 0L)
                return;

            RectangleF clientRect = ((Element)itemFrame).GetClientRect();
            List<ItemMod> itemMods = modsComponent?.ItemMods;

            if (itemMods == null || ((IEnumerable<ItemMod>)itemMods).Any<ItemMod>((Func<ItemMod, bool>)(x => string.IsNullOrEmpty(x.RawName) && string.IsNullOrEmpty(x.Name))))
                return;
            List<ModValue> list = ((IEnumerable<ItemMod>)itemMods).Select<ItemMod, ModValue>((Func<ItemMod, ModValue>)(item => new ModValue(item, this.GameController.Files, modsComponent.ItemLevel, this.GameController.Files.BaseItemTypes.Translate(poeEntity.Path)))).ToList<ModValue>();

            System.Numerics.Vector2 num1 = ConvertHelper.TranslateToNum(clientRect.TopLeft, 20f, 56f);
            int num2 = list.Count<ModValue>((Func<ModValue, bool>)(item => item.CouldHaveTiers() && item.Tier == 1));
            int num3 = list.Count<ModValue>((Func<ModValue, bool>)(item => item.CouldHaveTiers() && item.Tier == 2));
            int num4 = list.Count<ModValue>((Func<ModValue, bool>)(item => item.CouldHaveTiers() && item.Tier == 3));
            float y1 = this.Graphics.MeasureText("T").Y * (float)(Math.Sign(num2) + Math.Sign(num3) + Math.Sign(num4));
            float x1 = this.Graphics.MeasureText("T1 x6").X;
            this.Graphics.DrawBox(num1, num1 + new System.Numerics.Vector2(x1, y1), Color.Black, 0.0f);
            DefaultInterpolatedStringHandler interpolatedStringHandler;
            if (num2 > 0)
            {
                ref float local = ref num1.Y;
                double num5 = (double)local;
                Graphics graphics = this.Graphics;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                interpolatedStringHandler.AppendLiteral("T1 x");
                interpolatedStringHandler.AppendFormatted<int>(num2);
                string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                System.Numerics.Vector2 vector2 = num1;
                Color color = this.Settings.ItemMods.T1Color;
                double y2 = (double)graphics.DrawText(stringAndClear, vector2, color).Y;
                local = (float)(num5 + y2);
            }
            if (num3 > 0)
            {
                ref float local = ref num1.Y;
                double num6 = (double)local;
                Graphics graphics = this.Graphics;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                interpolatedStringHandler.AppendLiteral("T2 x");
                interpolatedStringHandler.AppendFormatted<int>(num3);
                string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                System.Numerics.Vector2 vector2 = num1;
                Color color = this.Settings.ItemMods.T2Color;
                double y3 = (double)graphics.DrawText(stringAndClear, vector2, color).Y;
                local = (float)(num6 + y3);
            }
            if (num4 > 0)
            {
                ref float local = ref num1.Y;
                double num7 = (double)local;
                Graphics graphics = this.Graphics;
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                interpolatedStringHandler.AppendLiteral("T3 x");
                interpolatedStringHandler.AppendFormatted<int>(num4);
                string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                System.Numerics.Vector2 vector2 = num1;
                Color color = this.Settings.ItemMods.T3Color;
                double y4 = (double)graphics.DrawText(stringAndClear, vector2, color).Y;
                local = (float)(num7 + y4);
            }
            if (this.Settings.ItemLevel.Enable.Value)
            {
                Mods mods = modsComponent;
                string str = Convert.ToString(mods != null ? mods.ItemLevel : 0);
                int num8 = this.Settings.ItemLevel.TextSize + 10;
                this.Graphics.DrawImage("menu-colors.png", new RectangleF(clientRect.TopLeft.X - 2f, clientRect.TopLeft.Y - 2f, (float)num8, (float)num8), this.Settings.ItemLevel.BackgroundColor);
                this.Graphics.DrawText(str, ConvertHelper.TranslateToNum(clientRect.TopLeft, 3f, 3f), this.Settings.ItemLevel.TextColor);
            }
            if (this.Settings.ItemMods.Enable.Value)
            {
                float y5 = clientRect.Bottom + 5f;
                System.Numerics.Vector2 seed = new System.Numerics.Vector2(clientRect.X + 20f, y5 + 4f);
                float height = list.Where<ModValue>((Func<ModValue, bool>)(x => ((IEnumerable<StatsDat.StatRecord>)x.Record.StatNames).Count<StatsDat.StatRecord>((Func<StatsDat.StatRecord, bool>)(y => y != null)) > 0)).Aggregate<ModValue, System.Numerics.Vector2>(seed, (Func<System.Numerics.Vector2, ModValue, System.Numerics.Vector2>)((position, item) => this.DrawMod(item, position))).Y - y5;
                if ((double)height > 4.0)
                    this.Graphics.DrawBox(new RectangleF(clientRect.X + 1f, y5, clientRect.Width, height), this.Settings.ItemMods.BackgroundColor);
            }
            Weapon weaponComponent;
            //Graphics.DrawText($"Check: {poeEntity.TryGetComponent<Weapon>(out weaponComponent).ToString()}",new Vector2(100,100), Color.Green);
            if (!poeEntity.TryGetComponent<Weapon>(out weaponComponent))
                return;
            //Graphics.DrawText($"weaponComponent: [{weaponComponent.DamageMin} / {weaponComponent.DamageMax}]", new Vector2(100, 125), Color.White);
            this.DrawWeaponDps(clientRect, poeEntity, list, weaponComponent);
        }
        public override void EntityAdded(Entity entity)
        {
            //If you have a reason to process every entity only once,
            //this is a good place to do so.
            //You may want to use a queue and run the actual
            //processing (if any) inside the Tick method.
        }
        #region HUD_For_Crafting
        private void Draw_HUD()
        {
            bool isVisible_Stash = ((Element)this.GameController.IngameState.IngameUi.StashElement).IsVisible;
            //bool isVisible_Inventory = ((Element)this.GameController.IngameState.IngameUi.InventoryPanel).IsVisible;

            if (isVisible_Stash)
            {
                //Draw_HUD_Background();
            }

        }
        //private void Draw_HUD_Background()
        //{
        //    RectangleF clientRect = this._currentOpenedStashTab.InventoryUIElement.GetClientRect();
        //    RectangleF rectangleF = new RectangleF(clientRect.Right, clientRect.Bottom, 270f, 240f);

        //    this.Graphics.DrawBox(rectangleF, new Color(0, 0, 0, 200));
        //    this.Graphics.DrawFrame(rectangleF, Color.White, 2);

        //    float x1 = rectangleF.X + 10f;
        //    float y1 = rectangleF.Y + 10f;

        //    //this.Graphics.DrawText("Current " + (this._currentSetData.SetType == 1 ? "Chaos" : "Regal") + " set:", new Vector2(x1, y1), Color.White, 15);
        //    float y2 = y1 + 25f;

        //    for (int index = 0; index < 8; ++index)
        //    {
        //        foreach (StashItem preparedItem in this._itemSetTypes[index].GetPreparedItems())
        //        {
        //            int num = this._sData.PlayerInventory.StashTabItems.Contains(preparedItem) ? 1 : 0;
        //            bool flag = preparedItem.StashName == this._currentOpenedStashTabName;
        //            Color color = Color.Gray;
        //            if (num != 0)
        //                color = Color.Green;
        //            else if (flag)
        //                color = Color.Yellow;
        //            if (num == 0 & flag)
        //            {
        //                StashItem item = preparedItem;
        //                NormalInventoryItem normalInventoryItem = ((IEnumerable<NormalInventoryItem>)visibleInventoryItems).FirstOrDefault<NormalInventoryItem>((Func<NormalInventoryItem, bool>)(x => x.InventPosX == item.InventPosX && x.InventPosY == item.InventPosY));
        //                if (normalInventoryItem != null)
        //                    this.Graphics.DrawFrame(((Element)normalInventoryItem).GetClientRect(), Color.Yellow, 2);
        //            }
        //            this.Graphics.DrawText(preparedItem.StashName + " (" + preparedItem.ItemName + ") " + (preparedItem.LowLvl ? "L" : "H"), new Vector2(x1, y2), color, 15);
        //            y2 += 20f;
        //        }
        //    }

        //    float x2 = this.Settings.PositionX.Value;
        //    float y = this.Settings.PositionY.Value;
        //    RectangleF rectangleF1 = new RectangleF(x2, y, 230f, 200f);
        //    this.Graphics.DrawBox(rectangleF1, new Color(0, 0, 0, 200));
        //    this.Graphics.DrawFrame(rectangleF1, Color.White, 2);
        //    this.Graphics.DrawText(this._drawInfoString, new Vector2(x2 + 10f, y + 10f), Color.White, 15);
        //}
        #endregion HUD_For_Crafting
        #region Weapon_DPS
        private System.Numerics.Vector2 DrawMod(ModValue item, System.Numerics.Vector2 position)
        {
            System.Numerics.Vector2 vector2_1 = position;
            ItemModsSettings itemMods = this.Settings.ItemMods;
            (string, Color) valueTuple;
            switch ((int)item.AffixType - 1)
            {
                case 0:
                    valueTuple = ("[P]", itemMods.PrefixColor.Value);
                    break;
                case 1:
                    valueTuple = ("[S]", itemMods.SuffixColor.Value);
                    break;
                case 2:
                    valueTuple = ("[U]", new Color((int)byte.MaxValue, 140, 0));
                    break;
                case 3:
                    valueTuple = ("[NEM]", new Color((int)byte.MaxValue, 20, 147));
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
                    valueTuple = ("[E]", new Color((int)byte.MaxValue, 0, (int)byte.MaxValue));
                    break;
                case 10:
                    valueTuple = ("[ESS]", new Color(139, 0, 139));
                    break;
                case 12:
                    valueTuple = ("[BES]", new Color((int)byte.MaxValue, 99, 71));
                    break;
                case 13:
                    valueTuple = ("[DEL]", new Color(47, 79, 79));
                    break;
                case 14:
                    valueTuple = ("[SYN]", new Color((int)byte.MaxValue, 105, 180));
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
                    valueTuple = ("[FEE]", new Color((int)byte.MaxValue, 165, 0));
                    break;
                case 21:
                    valueTuple = ("[FEI]", new Color((int)byte.MaxValue, 165, 0));
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
                    valueTuple = ("[EXI]", new Color((int)byte.MaxValue, 69, 0));
                    break;
                case 28:
                    valueTuple = ("[EAT]", new Color((int)byte.MaxValue, 69, 0));
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
            if ((int)item.AffixType != 3)
            {
                int totalTiers = item.TotalTiers;
                ModType affixType1 = item.AffixType;
                bool flag = totalTiers > 0;
                Color color2 = (int)affixType1 == 1 ? (flag ? this.TColors.GetValueOrDefault<int, Color>(item.Tier, itemMods.PrefixColor) : itemMods.PrefixColor) : ((int)affixType1 == 2 ? (flag ? this.TColors.GetValueOrDefault<int, Color>(item.Tier, itemMods.SuffixColor) : itemMods.SuffixColor) : new Color());
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
                System.Numerics.Vector2 vector2_2 = (int)affixType2 == 1 ? this.Graphics.DrawText(str3, MathHepler.Translate(position, x1, 0.0f), itemMods.PrefixColor) : ((int)affixType2 == 2 ? this.Graphics.DrawText(str3, MathHepler.Translate(position, x1, 0.0f), itemMods.SuffixColor) : new System.Numerics.Vector2());
                System.Numerics.Vector2 vector2_3 = this.Settings.ItemMods.ShowModNames ? this.Graphics.DrawText(item.AffixText, MathHepler.Translate(position, x1 + vector2_2.X, 0.0f), color2) : System.Numerics.Vector2.Zero;
                if (this.Settings.ItemMods.StartStatsOnSameLine)
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
                        string str4 = string.Format(!intRange.HasSpread() ? "{0}" : "[{1}] {0}", (object)statName, (object)intRange);
                        string str5 = statName.ValueToString(num);
                        System.Numerics.Vector2 vector2_4;
                        if ((int)item.AffixType == 3 || this.Settings.ItemMods.StartStatsOnSameLine)
                        {
                            vector2_4 = this.Graphics.DrawText(str5, MathHepler.Translate(position, x1 + x2, 0.0f), Color.Gainsboro, (FontAlign)2);
                            this.Graphics.DrawText(str4, MathHepler.Translate(position, (float)((double)x1 + (double)x2 + 5.0), 0.0f), Color.Gainsboro);
                        }
                        else
                        {
                            vector2_4 = this.Graphics.DrawText(str5, MathHepler.Translate(position, x1, 0.0f), Color.Gainsboro, (FontAlign)2);
                            this.Graphics.DrawText(str4, MathHepler.Translate(position, 40f, 0.0f), Color.Gainsboro);
                        }
                        position.Y += vector2_4.Y;
                    }
                }
            }
            if ((double)Math.Abs(position.Y - vector2_1.Y) <= 1.0 / 1000.0)
                return vector2_1;
            return vector2_1 with { Y = position.Y + 4f };
        }

        private void DrawWeaponDps(RectangleF clientRect, Entity itemEntity, List<ModValue> modValues, Weapon weaponComponent)
        {
            
            if (weaponComponent == null || !itemEntity.IsValid)
                return;
            float num1 = (float)Math.Round(1000.0 / (double)weaponComponent.AttackTime, 2);
            int length = Enum.GetValues(typeof(DamageType)).Length;
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
                                            num1 *= (float)((100.0 + (double)num3) / 100.0);
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
                                        numArray[3] += (float)num3;
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
                                num2 += (float)num3 / 100f;
                                continue;
                            label_34:
                                numArray[1] += (float)num3;
                                continue;
                            label_35:
                                numArray[2] += (float)num3;
                                continue;
                            label_37:
                                numArray[4] += (float)num3;
                            }
                        }
                    }
                }
            }
            WeaponDpsSettings weaponDps = this.Settings.WeaponDps;
            Color[] colorArray = new Color[5]
            {
                Color.White, weaponDps.DmgFireColor, weaponDps.DmgColdColor, weaponDps.DmgLightningColor,weaponDps.DmgChaosColor
            };
            Quality component = itemEntity.GetComponent<Quality>();
            if (component == null)
                return;
            float num4 = num2 + (float)component.ItemQuality / 100f;
            //int num5 = (int)Math.Round((double)damageMin * (double)num4);
            //int num6 = (int)Math.Round((double)damageMax * (double)num4);
            int num5 = (int)(damageMin * num4);
            int num6 = (int)(damageMax * num4);

            //numArray[0] = (float)(num5 + num6);
            numArray[0] = num5 + num6;

            //float num7 = (float)Math.Round((double)num1, 2);
            float num7 = (float)Math.Round(num1, 2);

            float num8 = numArray[0] / 2f * num7;
            float num9 = 0.0f;
            int num10 = 0;
            Color color = weaponDps.PhysicalDamageColor;
            //for (int index = 1; index < length; ++index)
            for (int index = 1; index < colorArray.Length; ++index)
            {
                num9 += numArray[index] / 2f * num7;
                if ((double)numArray[index] > 0.0)
                {
                    if (num10 == 0)
                    {
                        num10 = index;
                        color = colorArray[index];
                    }
                    else
                        color = weaponDps.ElementalDamageColor;
                }
            }
            Vector2 vector2_1 = new Vector2(clientRect.Right - 15f, clientRect.Y + 1f);
            this.Graphics.DrawImage("preload-end.png", new RectangleF(vector2_1.X - 100f, vector2_1.Y - 6f, 100f, 78f), weaponDps.BackgroundColor);
            Vector2 vector2_2 = (double)num8 > 0.0 ? this.Graphics.DrawText(num8.ToString("#.#"), vector2_1, (FontAlign)2) : Vector2.Zero;
            Vector2 vector2_3 = (double)num9 > 0.0 ? this.Graphics.DrawText(num9.ToString("#.#"), MathHepler.Translate(vector2_1, 0.0f, vector2_2.Y), color, (FontAlign)2) : Vector2.Zero;
            float num11 = num8 + num9;
            Vector2 vector2_4 = (double)num11 > 0.0 ? this.Graphics.DrawText(num11.ToString("#.#"), MathHepler.Translate(vector2_1, 0.0f, vector2_2.Y + vector2_3.Y), Color.White, (FontAlign)2) : Vector2.Zero;
            
            
            this.Graphics.DrawText("dps", MathHepler.Translate(vector2_1, 0.0f, vector2_2.Y + vector2_3.Y + vector2_4.Y), weaponDps.TextColor, FontAlign.Right);
        }

        #endregion Weapon_DPS
    }
}