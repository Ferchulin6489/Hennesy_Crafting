// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Constants
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;

namespace ExileCore.Shared
{
  public static class Constants
  {
    public const int DefaultMaxStringLength = 256;
    public static readonly Size2F MapIconsSize = new Size2F(14f, 44f);
    public static readonly Size2F MyMapIcons = new Size2F(7f, 8f);
    public static readonly uint[] PlayerXpLevels = new uint[101]
    {
      0U,
      0U,
      525U,
      1760U,
      3781U,
      7184U,
      12186U,
      19324U,
      29377U,
      43181U,
      61693U,
      85990U,
      117506U,
      157384U,
      207736U,
      269997U,
      346462U,
      439268U,
      551295U,
      685171U,
      843709U,
      1030734U,
      1249629U,
      1504995U,
      1800847U,
      2142652U,
      2535122U,
      2984677U,
      3496798U,
      4080655U,
      4742836U,
      5490247U,
      6334393U,
      7283446U,
      8348398U,
      9541110U,
      10874351U,
      12361842U,
      14018289U,
      15859432U,
      17905634U,
      20171471U,
      22679999U,
      25456123U,
      28517857U,
      31897771U,
      35621447U,
      39721017U,
      44225461U,
      49176560U,
      54607467U,
      60565335U,
      67094245U,
      74247659U,
      82075627U,
      90631041U,
      99984974U,
      110197515U,
      121340161U,
      133497202U,
      146749362U,
      161191120U,
      176922628U,
      194049893U,
      212684946U,
      232956711U,
      255001620U,
      278952403U,
      304972236U,
      333233648U,
      363906163U,
      397194041U,
      433312945U,
      472476370U,
      514937180U,
      560961898U,
      610815862U,
      664824416U,
      723298169U,
      786612664U,
      855129128U,
      929261318U,
      1009443795U,
      1096169525U,
      1189918242U,
      1291270350U,
      1400795257U,
      1519130326U,
      1646943474U,
      1784977296U,
      1934009687U,
      2094900291U,
      2268549086U,
      2455921256U,
      2658074992U,
      2876116901U,
      3111280300U,
      3364828162U,
      3638186694U,
      3932818530U,
      4250334444U
    };
    public static readonly uint[] ToraExperienceLevels = new uint[8]
    {
      0U,
      770U,
      2690U,
      9310U,
      32230U,
      100368U,
      308696U,
      934584U
    };
    public static readonly uint[] CatarinaExperienceLevels = new uint[8]
    {
      0U,
      770U,
      2690U,
      9310U,
      32230U,
      100368U,
      308696U,
      934584U
    };
    public static readonly uint[] HakuExperienceLevels = new uint[8]
    {
      0U,
      430U,
      1640U,
      6250U,
      23770U,
      81288U,
      274592U,
      913031U
    };
    public static readonly uint[] VaganExperienceLevels = new uint[8]
    {
      0U,
      1050U,
      3480U,
      11500U,
      37960U,
      112743U,
      330712U,
      954947U
    };
    public static readonly uint[] VoriciExperienceLevels = new uint[8]
    {
      0U,
      1050U,
      3480U,
      11500U,
      37960U,
      112743U,
      330712U,
      954947U
    };
    public static readonly uint[] ElreonExperienceLevels = new uint[8]
    {
      0U,
      430U,
      1640U,
      6250U,
      23770U,
      81288U,
      274592U,
      913031U
    };
    public static readonly uint[] ZanaExperienceLevels = new uint[8]
    {
      0U,
      4700U,
      13170U,
      36870U,
      103260U,
      289130U,
      809570U,
      2266810U
    };
    public static readonly uint[] LeoExperienceLevels = new uint[8]
    {
      0U,
      2700U,
      7030U,
      18270U,
      47520U,
      111204U,
      257016U,
      584710U
    };

    public static int GetLevel(uint[] expLevels, uint currExp)
    {
      for (int level = 1; level < expLevels.Length; ++level)
      {
        if (currExp < expLevels[level])
          return level;
      }
      return 8;
    }

    public static uint GetFullCurrentLevel(uint[] expLevels, uint currExp)
    {
      uint num = 0;
      for (int index = 1; index < expLevels.Length; ++index)
      {
        uint expLevel = expLevels[index];
        if (currExp < expLevel)
          return num + expLevel;
        num += expLevel;
      }
      return 8;
    }
  }
}
