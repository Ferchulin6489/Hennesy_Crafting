// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.UrlDecoders.PathOfExileUrlDecoder
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


#nullable enable
namespace PassiveSkillTreePlanter.UrlDecoders
{
  public class PathOfExileUrlDecoder
  {
    private static readonly 
    #nullable disable
    Regex UrlRegex = new Regex("^(http(|s):\\/\\/|)(\\w*\\.|)pathofexile\\.com\\/(fullscreen-|)(?<type>atlas|passive)-skill-tree\\/(\\d+(\\.\\d+)+\\/)?(?<build>[\\w-=]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static bool TryMatch(
      string buildUrl,
      out ESkillTreeType type,
      out HashSet<ushort> passiveIds)
    {
      Match match = PathOfExileUrlDecoder.UrlRegex.Match(buildUrl);
      if (match != null && match.Success)
      {
        string unmatchedValue = match.Groups[nameof (type)].Value;
        ESkillTreeType eskillTreeType;
        switch (unmatchedValue)
        {
          case "atlas":
            eskillTreeType = ESkillTreeType.Atlas;
            break;
          case "passive":
            eskillTreeType = ESkillTreeType.Character;
            break;
          default:
            // ISSUE: reference to a compiler-generated method
            \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) unmatchedValue);
            break;
        }
        type = eskillTreeType;
        passiveIds = PathOfExileUrlDecoder.Decode(match.Groups["build"].Value);
        return true;
      }
      type = ESkillTreeType.Unknown;
      passiveIds = (HashSet<ushort>) null;
      return false;
    }

    private static HashSet<ushort> Decode(string buildCode)
    {
      HashSet<ushort> ushortSet = new HashSet<ushort>();
      byte[] numArray = Convert.FromBase64String(buildCode.Replace('-', '+').Replace('_', '/'));
      for (int index = ((int) numArray[0] << 24 | (int) numArray[1] << 16 | (int) numArray[2] << 8 | (int) numArray[3]) > 3 ? 7 : 6; index < numArray.Length; index += 2)
      {
        ushort num = (ushort) ((uint) numArray[index] << 8 | (uint) numArray[index + 1]);
        ushortSet.Add(num);
      }
      return ushortSet;
    }

    public static string Encode(HashSet<ushort> nodes, ESkillTreeType type)
    {
      string str1;
      switch (type)
      {
        case ESkillTreeType.Character:
          str1 = "https://www.pathofexile.com/fullscreen-passive-skill-tree/";
          break;
        case ESkillTreeType.Atlas:
          str1 = "https://www.pathofexile.com/fullscreen-atlas-skill-tree/";
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
          break;
      }
      string str2 = str1;
      byte[] first = new byte[4]
      {
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 6
      };
      byte[] second1 = new byte[1];
      byte[] second2 = new byte[1];
      byte[] second3 = new byte[1]{ (byte) nodes.Count };
      IEnumerable<byte> second4 = nodes.OrderBy<ushort, ushort>((Func<ushort, ushort>) (x => x)).SelectMany<ushort, byte>((Func<ushort, IEnumerable<byte>>) (x => (IEnumerable<byte>) new byte[2]
      {
        (byte) ((uint) x >> 8),
        (byte) x
      }));
      byte[] second5 = new byte[2];
      string str3 = Convert.ToBase64String(((IEnumerable<byte>) first).Concat<byte>((IEnumerable<byte>) second1).Concat<byte>((IEnumerable<byte>) second2).Concat<byte>((IEnumerable<byte>) second3).Concat<byte>(second4).Concat<byte>((IEnumerable<byte>) second5).ToArray<byte>()).Replace('+', '-').Replace('/', '_');
      return str2 + str3;
    }
  }
}
