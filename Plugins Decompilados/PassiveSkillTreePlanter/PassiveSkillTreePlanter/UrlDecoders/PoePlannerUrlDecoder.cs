// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.UrlDecoders.PoePlannerUrlDecoder
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using ExileCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PassiveSkillTreePlanter.UrlDecoders
{
  public class PoePlannerUrlDecoder
  {
    private static readonly Regex UrlRegex = new Regex("^(http(|s):\\/\\/|)(\\w*\\.|)poeplanner\\.com\\/(?<build>[\\w-=]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static bool UrlMatch(string buildUrl) => PoePlannerUrlDecoder.UrlRegex.IsMatch(buildUrl);

    public static HashSet<ushort> Decode(string url)
    {
      string str = ((IEnumerable<string>) url.Split('/')).LastOrDefault<string>();
      if (str == null)
      {
        Logger.Log.Error<int>("Can't decode PoePlanner Url", 5);
        return new HashSet<ushort>();
      }
      byte[] sourceArray = Convert.FromBase64String(str.Replace("-", "+").Replace("_", "/"));
      int length = (int) sourceArray[3] << 8 | (int) sourceArray[4];
      byte[] destinationArray = new byte[length];
      Array.Copy((Array) sourceArray, 5, (Array) destinationArray, 0, length);
      HashSet<ushort> ushortSet = new HashSet<ushort>();
      try
      {
        for (int index = 4; index < length - 1; index += 2)
          ushortSet.Add((ushort) ((uint) destinationArray[index] << 8 | (uint) destinationArray[index + 1]));
      }
      catch
      {
        Logger.Log.Error<int>("Error while parsing some PoePlanner nodes from Url.", 5);
      }
      return ushortSet;
    }
  }
}
