using Hennesy_Crafting.Settings;
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Nodes;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Hennesy_Crafting
{
    public class FastModsModule
    {
        private readonly Graphics _graphics;
        private readonly ItemModsSettings _modsSettings;
        private long _lastTooltipAddress;
        private Element _regularModsElement;
        private List<FastModsModule.ModTierInfo> _mods = new List<FastModsModule.ModTierInfo>();
        private readonly Regex _modTypeRegex = new Regex("\\<rgb\\(\\d+\\,\\d+\\,\\d+\\)\\>\\{([\\w ]+)\\}", RegexOptions.Compiled);
        private static readonly Regex FracturedRegex = new Regex("\\<fractured\\>\\{([^\\n]*\\n[^\\n]*)(?:\\n\\<italic\\>\\{[^\\n]*\\})?\\}(?=\\n|$)", RegexOptions.Compiled);
        public string data2 = "";
        public FastModsModule(Graphics graphics, ItemModsSettings modsSettings)
        {
            this._graphics = graphics;
            this._modsSettings = modsSettings;
        }

        public void DrawUiHoverFastMods(Element tooltip)
        {
            try
            {
                this.InitializeElements(tooltip);
                Element regularModsElement = this._regularModsElement;
                if (regularModsElement == null || !regularModsElement.IsVisibleLocal)
                    return;
                RectangleF getClientRectCache = this._regularModsElement.GetClientRectCache;
                System.Numerics.Vector2 vector2_1 = new System.Numerics.Vector2(tooltip.GetClientRectCache.X - 3f, getClientRectCache.TopLeft.Y);
                float num1 = getClientRectCache.Height / (float)this._mods.Count;
                FastModsModule.ModTierInfo mod;
                for (int index = 0; index < this._mods.Count; index = index + (mod.ModLines - 1) + 1)
                {
                    mod = this._mods[index];
                    float num2 = num1 * (float)mod.ModLines;
                    System.Numerics.Vector2 vector2_2 = MathHepler.Translate(vector2_1, 0.0f, num2 / 2f);
                    System.Numerics.Vector2 vector2_3 = this._graphics.DrawText(mod.DisplayName, vector2_2, mod.Color, (FontAlign)6);
                    vector2_3.X += 5f;
                    vector2_2.X -= vector2_3.X + 5f;
                    System.Numerics.Vector2 vector2_4 = vector2_3;
                    if (this._modsSettings.EnableFastModsTags)
                    {
                        foreach (FastModsModule.ModType modType in mod.ModTypes)
                        {
                            System.Numerics.Vector2 vector2_5 = this._graphics.DrawText(modType.Name, vector2_2, modType.Color, (FontAlign)6);
                            vector2_3.X += vector2_5.X + 5f;
                            vector2_2.X -= vector2_5.X + 5f;
                        }
                        if (mod.ModTypes.Count > 0)
                            vector2_3.X += 5f;
                    }
                    RectangleF rectangleF = new RectangleF((float)((double)vector2_1.X - (double)vector2_3.X - 3.0), vector2_1.Y, vector2_3.X + 6f, num1 * (float)mod.ModLines);
                    this._graphics.DrawBox(rectangleF, Color.Black);
                    this._graphics.DrawFrame(rectangleF, Color.Gray, 1);
                    this._graphics.DrawFrame(new RectangleF((float)((double)vector2_1.X - (double)vector2_4.X - 3.0), vector2_1.Y, vector2_4.X + 6f, num1 * (float)mod.ModLines), Color.Gray, 1);
                    vector2_1.Y += num2;
                }
            }
            catch
            {
            }
        }

        private void InitializeElements(Element tooltip)
        {
            Element childAtIndex = tooltip.GetChildAtIndex(1);
            if (childAtIndex == null)
                return;
            Element extendedModsElement = (Element)null;
            Element element = (Element)null;
            for (int index = ((ICollection<Element>)childAtIndex.Children).Count - 1; index >= 0; --index)
            {
                Element child = childAtIndex.Children[index];
                string text = child.Text;
                if (!string.IsNullOrEmpty(text) && (text.StartsWith("<smaller>", StringComparison.Ordinal) || text.StartsWith("<fractured>{<smaller>", StringComparison.Ordinal)) && !child.TextNoTags.StartsWith("Allocated Crucible", StringComparison.Ordinal))
                {
                    extendedModsElement = child;
                    element = childAtIndex.Children[index - 1];
                    break;
                }
            }
            if (element == null)
            {
                this._regularModsElement = (Element)null;
                this._lastTooltipAddress = 0L;
            }
            else
            {
                if (this._lastTooltipAddress == ((RemoteMemoryObject)tooltip).Address)
                {
                    long? address1 = ((RemoteMemoryObject)this._regularModsElement)?.Address;
                    long address2 = ((RemoteMemoryObject)element).Address;
                    if (address1.GetValueOrDefault() == address2 & address1.HasValue)
                        return;
                }
                this._lastTooltipAddress = ((RemoteMemoryObject)tooltip).Address;
                this._regularModsElement = element;
                this.ParseItemHover(tooltip, extendedModsElement);
            }
        }

        private static string RemoveFractured(string x) => FastModsModule.FracturedRegex.Replace(x, "$1");

        private void ParseItemHover(Element tooltip, Element extendedModsElement)
        {
            this._mods.Clear();
            //here1
            this.data2 = extendedModsElement.GetText(2500).Replace("\r\n", "\n");
            //this.data2 = this._regularModsElement.GetText(2500).Replace("\r\n", "\n");
            string[] strArray1 = FastModsModule.RemoveFractured(extendedModsElement.GetText(2500).Replace("\r\n", "\n")).Split('\n');
            string[] strArray2 = this._regularModsElement.GetTextWithNoTags(2500).Replace("\r\n", "\n").Split('\n');
            FastModsModule.ModTierInfo modTierInfo1 = (FastModsModule.ModTierInfo)null;
            Dictionary<string, FastModsModule.ModTierInfo> dictionary = new Dictionary<string, FastModsModule.ModTierInfo>();
            foreach (string input in strArray1)
            {
                if (!input.StartsWith("<italic>", StringComparison.Ordinal))
                {
                    if (input.StartsWith("<smaller>", StringComparison.Ordinal) || input.StartsWith("<crafted>", StringComparison.Ordinal))
                    {
                        bool flag1 = input.Contains("Prefix");
                        bool flag2 = input.Contains("Suffix");
                        if (!flag1 && !flag2)
                        {
                            DebugWindow.LogMsg("Cannot extract Affix type from mod text: " + input, 4f);
                            return;
                        }
                        string displayName = flag1 ? "P" : "S";
                        Color color1 = flag1 ? this._modsSettings.PrefixColor : this._modsSettings.SuffixColor;
                        bool flag3 = false;
                        int num = input.IndexOf("(Tier: ", StringComparison.Ordinal);
                        int startIndex;
                        if (num != -1)
                        {
                            startIndex = num + "(Tier: ".Length;
                        }
                        else
                        {
                            startIndex = input.IndexOf("(Rank: ", StringComparison.Ordinal);
                            if (startIndex != -1)
                            {
                                startIndex += "(Rank: ".Length;
                                flag3 = true;
                            }
                        }
                        int result;
                        if (startIndex != -1 && (int.TryParse(input.Substring(startIndex, 2), out result) || int.TryParse(input.Substring(startIndex, 1), out result)))
                        {
                            if (flag3)
                            {
                                string str = displayName;
                                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
                                interpolatedStringHandler.AppendLiteral(" Rank");
                                interpolatedStringHandler.AppendFormatted<int>(result);
                                string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                                displayName = str + stringAndClear;
                            }
                            else
                                displayName += result.ToString();
                            Color color2;
                            switch (result)
                            {
                                case 1:
                                    color2 = this._modsSettings.T1Color;
                                    break;
                                case 2:
                                    color2 = this._modsSettings.T2Color;
                                    break;
                                case 3:
                                    color2 = this._modsSettings.T3Color;
                                    break;
                                default:
                                    color2 = color1;
                                    break;
                            }
                            color1 = color2;
                        }
                        else if (input.Contains("Essence"))
                            displayName += "(Ess)";
                        modTierInfo1 = new FastModsModule.ModTierInfo(displayName, color1);
                        MatchCollection matchCollection = this._modTypeRegex.Matches(input);
                        if (matchCollection.Count > 0)
                        {
                            foreach (Match match in matchCollection)
                            {
                                string name = match.Groups[1].Value;
                                Color color3;
                                if (name != null)
                                {
                                    switch (name.Length)
                                    {
                                        case 4:
                                            switch (name[0])
                                            {
                                                case 'C':
                                                    if (name == "Cold")
                                                    {
                                                        color3 = new Color(41, 102, 241);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'F':
                                                    if (name == "Fire")
                                                    {
                                                        color3 = Color.Red;
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'L':
                                                    if (name == "Life")
                                                    {
                                                        color3 = Color.Magenta;
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'M':
                                                    if (name == "Mana")
                                                    {
                                                        color3 = new Color(20, 240, (int)byte.MaxValue);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                            }
                                            break;
                                        case 5:
                                            if (name == "Speed")
                                            {
                                                color3 = new Color(0, (int)byte.MaxValue, 192);
                                                goto label_55;
                                            }
                                            else
                                                break;
                                        case 6:
                                            switch (name[0])
                                            {
                                                case 'A':
                                                    if (name == "Attack")
                                                    {
                                                        color3 = new Color(240, 100, 30);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'C':
                                                    if (name == "Caster")
                                                    {
                                                        color3 = new Color(216, 0, (int)byte.MaxValue);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                            }
                                            break;
                                        case 8:
                                            switch (name[0])
                                            {
                                                case 'C':
                                                    if (name == "Critical")
                                                    {
                                                        color3 = new Color(168, 220, 26);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'P':
                                                    if (name == "Physical")
                                                    {
                                                        color3 = new Color(225, 170, 20);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                            }
                                            break;
                                        case 9:
                                            switch (name[0])
                                            {
                                                case 'E':
                                                    if (name == "Elemental")
                                                    {
                                                        color3 = Color.White;
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'G':
                                                    if (name == "Gem Level")
                                                    {
                                                        color3 = new Color(200, 230, 160);
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                                case 'L':
                                                    if (name == "Lightning")
                                                    {
                                                        color3 = Color.Yellow;
                                                        goto label_55;
                                                    }
                                                    else
                                                        break;
                                            }
                                            break;
                                    }
                                }
                                color3 = Color.Gray;
                            label_55:
                                Color color4 = color3;
                                modTierInfo1.ModTypes.Add(new FastModsModule.ModType(name, color4));
                            }
                        }
                    }
                    else if (input.StartsWith("<", StringComparison.Ordinal) && !char.IsLetterOrDigit(input[0]))
                        modTierInfo1 = (FastModsModule.ModTierInfo)null;
                    else if (modTierInfo1 != null)
                    {
                        string key = Regex.Replace(Regex.Replace(Regex.Replace(input, "\\([\\d-.]+\\)", string.Empty), "[\\d-.]+", "#"), "\\s\\([\\d]+% Increased\\)", string.Empty).Replace(" (#% Increased)", string.Empty);
                        if (key.StartsWith("+", StringComparison.Ordinal))
                            key = key.Substring(1);
                        if (!dictionary.ContainsKey(key))
                            dictionary[key] = modTierInfo1;
                    }
                }
            }
            List<FastModsModule.ModTierInfo> modTierInfoList = new List<FastModsModule.ModTierInfo>();
            //string input2 = "";
            foreach (string item in strArray2)
            {
                var input = item;
                if (input.StartsWith("+", StringComparison.Ordinal))
                    input = input.Substring(1);
                string str = Regex.Replace(input, "[\\d-.]+", "#");
                bool flag = false;
                foreach (KeyValuePair<string, FastModsModule.ModTierInfo> keyValuePair in dictionary)
                {
                    if (str.Contains(keyValuePair.Key))
                    {
                        flag = true;
                        modTierInfoList.Add(keyValuePair.Value);
                        break;
                    }
                }
                if (!flag)
                {
                    DebugWindow.LogMsg("Cannot extract mod from parsed mods: " + str + $"\n{input.ToString()}", 4f);
                    FastModsModule.ModTierInfo modTierInfo2 = new FastModsModule.ModTierInfo("?", Color.Gray);
                    modTierInfoList.Add(modTierInfo2);
                }
            }
            if (modTierInfoList.Count > 1)
            {
                for (int index = 1; index < modTierInfoList.Count; ++index)
                {
                    FastModsModule.ModTierInfo modTierInfo3 = modTierInfoList[index];
                    FastModsModule.ModTierInfo modTierInfo4 = modTierInfoList[index - 1];
                    if (modTierInfo3 == modTierInfo4)
                        ++modTierInfo3.ModLines;
                }
            }
            this._mods = modTierInfoList;
        }

        private class ModTierInfo
        {
            public ModTierInfo(string displayName, Color color)
            {
                this.DisplayName = displayName;
                this.Color = color;
            }

            public string DisplayName { get; }

            public Color Color { get; }

            public List<FastModsModule.ModType> ModTypes { get; set; } = new List<FastModsModule.ModType>();

            public int ModLines { get; set; } = 1;
        }

        public class ModType
        {
            public ModType(string name, Color color)
            {
                this.Name = name;
                this.Color = color;
            }

            public string Name { get; }

            public Color Color { get; }
        }
    }
}
