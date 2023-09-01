// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Element
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory
{
  public class Element : RemoteMemoryObject
  {
    public const int OffsetBuffers = 0;
    private static readonly int ChildStartOffset = ExileCore.Shared.Helpers.Extensions.GetOffset<ElementOffsets>((Expression<Func<ElementOffsets, object>>) (x => (object) x.ChildStart));
    private static readonly int EntityOffset = ExileCore.Shared.Helpers.Extensions.GetOffset<NormalInventoryItemOffsets>((Expression<Func<NormalInventoryItemOffsets, object>>) (x => (object) x.Item));
    private readonly CachedValue<ElementOffsets> _cacheElement;
    private readonly List<Element> _childrens = new List<Element>();
    private CachedValue<SharpDX.RectangleF> _getClientRect;
    private Element _parent;
    private long childHashCache;

    public Element() => this._cacheElement = (CachedValue<ElementOffsets>) new FrameCache<ElementOffsets>((Func<ElementOffsets>) (() => this.Address != 0L ? this.M.Read<ElementOffsets>(this.Address) : new ElementOffsets()));

    public ElementOffsets Elem => this._cacheElement.Value;

    public bool IsValid => this.Elem.SelfPointer == this.Address;

    public long ChildCount => (this.Elem.ChildEnd - this.Elem.ChildStart) / 8L;

    public bool IsVisibleLocal => this.Elem.Flags.HasFlag((Enum) ElementFlags.IsVisibleLocal);

    public bool IsScrollable => this.Elem.Flags.HasFlag((Enum) ElementFlags.IsScrollable);

    public bool IsSelected => this.Elem.IsSelected == 0L;

    public Element Root => this.TheGame.IngameState.UIRoot;

    public Element Parent => this.Elem.Parent != 0L ? this._parent ?? (this._parent = this.GetObject<Element>(this.Elem.Parent)) : (Element) null;

    [Obsolete]
    public SharpDX.Vector2 Position => this.Elem.Position.ToSharpDx();

    public System.Numerics.Vector2 PositionNum => this.Elem.Position;

    public float X => this.PositionNum.X;

    public float Y => this.PositionNum.Y;

    public System.Numerics.Vector2 ScrollOffset => this.Elem.ScrollOffset;

    public Element Tooltip => this.Address != 0L ? this.ReadObject<Element>(this.Elem.Tooltip) : (Element) null;

    public float Scale => this.Elem.Scale;

    public float Width => this.Elem.Size.X;

    public float Height => this.Elem.Size.Y;

    public bool isHighlighted => this.Elem.isHighlighted;

    public Entity Entity => this.ReadObject<Entity>(this.Address + (long) Element.EntityOffset);

    public System.Drawing.Point Center
    {
      get
      {
        SharpDX.Vector2 center = this.GetClientRect().Center;
        return new System.Drawing.Point(Convert.ToInt32(center.X), Convert.ToInt32(center.Y));
      }
    }

    public virtual string Text => this.GetText(256);

    public string TextNoTags => this.GetTextWithNoTags(256);

    public string GetText(int maxLength) => Element.ReplaceIconReferences(this.Elem.Text.ToString(this.M, maxLength));

    public string GetTextWithNoTags(int maxLength) => Element.ReplaceIconReferences(this.Elem.TextNoTags.ToString(this.M, maxLength));

    private static string ReplaceIconReferences(string text) => !string.IsNullOrWhiteSpace(text) ? text.Replace("    ", "{{icon}}") : (string) null;

    public bool IsVisible => this.Address < 1770350607106052L && this.Address > 0L && this.IsVisibleLocal && this.GetParentChain().All<Element>((Func<Element, bool>) (current => current.IsVisibleLocal));

    public IList<Element> Children => (IList<Element>) this.GetChildren<Element>();

    public long ChildHash => (long) this.Elem.Childs.GetHashCode();

    public SharpDX.RectangleF GetClientRectCache
    {
      get
      {
        CachedValue<SharpDX.RectangleF> getClientRect = this._getClientRect;
        return getClientRect == null ? (this._getClientRect = (CachedValue<SharpDX.RectangleF>) new TimeCache<SharpDX.RectangleF>(new Func<SharpDX.RectangleF>(this.GetClientRect), 200L)).Value : getClientRect.Value;
      }
    }

    public Element this[int index] => this.GetChildAtIndex(index);

    public int? IndexInParent => this.Parent?.Children.IndexOf(this);

    public string PathFromRoot
    {
      get
      {
        List<Element> parentChain = this.GetParentChain();
        if (parentChain.Count != 0)
        {
          parentChain.RemoveAt(parentChain.Count - 1);
          parentChain.Reverse();
        }
        parentChain.Add(this);
        ILookup<long?, string> properties = ((IEnumerable<PropertyInfo>) this.TheGame.IngameState.IngameUi.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => typeof (Element).IsAssignableFrom(p.PropertyType))).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetIndexParameters().Length == 0)).Select<PropertyInfo, (PropertyInfo, long?)>((Func<PropertyInfo, (PropertyInfo, long?)>) (p => (p, p.GetValue((object) this.TheGame.IngameState.IngameUi) is Element element ? new long?(element.Address) : new long?()))).Where<(PropertyInfo, long?)>((Func<(PropertyInfo, long?), bool>) (t =>
        {
          long? address = t.Address;
          return !address.HasValue || address.GetValueOrDefault() != 0L;
        })).ToLookup<(PropertyInfo, long?), long?, string>((Func<(PropertyInfo, long?), long?>) (x => x.Address), (Func<(PropertyInfo, long?), string>) (x => x.property.Name));
        return string.Join("->", parentChain.Select<Element, string>((Func<Element, string>) (x =>
        {
          List<string> list = properties[new long?(x.Address)].ToList<string>();
          string stringAndClear;
          if (list != null)
          {
            switch (list.Count)
            {
              case 0:
                stringAndClear = x.IndexInParent.ToString();
                goto label_5;
              case 1:
                DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(2, 2);
                interpolatedStringHandler1.AppendLiteral("(");
                interpolatedStringHandler1.AppendFormatted(list.First<string>());
                interpolatedStringHandler1.AppendLiteral(")");
                interpolatedStringHandler1.AppendFormatted<int?>(x.IndexInParent);
                stringAndClear = interpolatedStringHandler1.ToStringAndClear();
                goto label_5;
            }
          }
          List<string> values = list;
          DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
          interpolatedStringHandler2.AppendLiteral("(");
          interpolatedStringHandler2.AppendFormatted(string.Join<string>('/', (IEnumerable<string>) values));
          interpolatedStringHandler2.AppendLiteral(")");
          interpolatedStringHandler2.AppendFormatted<int?>(x.IndexInParent);
          stringAndClear = interpolatedStringHandler2.ToStringAndClear();
label_5:
          return stringAndClear;
        })));
      }
    }

    protected List<Element> GetChildren<T>() where T : Element
    {
      ElementOffsets elem = this.Elem;
      long childCount = this.ChildCount;
      if (this.Address == 0L || elem.ChildStart == 0L || elem.ChildEnd == 0L || childCount <= 0L || this.ChildHash == this.childHashCache || childCount > (long) Limits.ElementChildCount)
        return this._childrens;
      IList<long> longList = this.M.ReadPointersArray(elem.ChildStart, elem.ChildEnd);
      if ((long) longList.Count != childCount)
        return this._childrens;
      this._childrens.Clear();
      this._childrens.EnsureCapacity(longList.Count);
      foreach (long address in (IEnumerable<long>) longList)
        this._childrens.Add(this.GetObject<Element>(address));
      this.childHashCache = this.ChildHash;
      return this._childrens;
    }

    public List<T> GetChildrenAs<T>() where T : Element, new()
    {
      ElementOffsets elem = this.Elem;
      if (this.Address == 0L || elem.ChildStart == 0L || elem.ChildEnd == 0L || this.ChildCount <= 0L)
        return new List<T>();
      IList<long> longList = this.M.ReadPointersArray(elem.ChildStart, elem.ChildEnd);
      if ((long) longList.Count != this.ChildCount)
        return new List<T>();
      List<T> childrenAs = new List<T>(longList.Count);
      foreach (long address in (IEnumerable<long>) longList)
        childrenAs.Add(this.GetObject<T>(address));
      return childrenAs;
    }

    public List<Element> GetParentChain()
    {
      List<Element> parentChain = new List<Element>();
      if (this.Address == 0L)
        return parentChain;
      HashSet<Element> elementSet = new HashSet<Element>();
      Element root = this.Root;
      Element parent = this.Parent;
      if (root == null || parent == null)
        return parentChain;
      while (!elementSet.Contains(parent) && root.Address != parent.Address && parent.Address != 0L && elementSet.Count < 100)
      {
        parentChain.Add(parent);
        elementSet.Add(parent);
        parent = parent.Parent;
        if (parent == null)
          break;
      }
      return parentChain;
    }

    [Obsolete]
    public SharpDX.Vector2 GetParentPos()
    {
      System.Numerics.Vector2 zero = System.Numerics.Vector2.Zero;
      float scale = this.TheGame.IngameState.UIRoot.Scale;
      foreach (Element element in this.GetParentChain())
        zero += element.PositionNum * element.Scale / scale;
      return zero.ToSharpDx();
    }

    private System.Numerics.Vector2 GetChainPos()
    {
      System.Numerics.Vector2 zero = System.Numerics.Vector2.Zero;
      float scale = this.TheGame.IngameState.UIRoot.Scale;
      float num1 = 0.0f;
      System.Numerics.Vector2 vector2 = System.Numerics.Vector2.Zero;
      List<Element> parentChain = this.GetParentChain();
      for (int index = parentChain.Count - 1; index >= 0; --index)
      {
        Element element = parentChain[index];
        float num2 = element.Scale / scale;
        zero += element.PositionNum * num2 + (element.IsScrollable ? num1 * vector2 : System.Numerics.Vector2.Zero);
        num1 = num2;
        vector2 = element.ScrollOffset;
      }
      return zero + (this.PositionNum * this.Scale / scale + (this.IsScrollable ? num1 * vector2 : System.Numerics.Vector2.Zero));
    }

    public virtual SharpDX.RectangleF GetClientRect()
    {
      if (this.Address == 0L)
        return SharpDX.RectangleF.Empty;
      double width = (double) this.TheGame.IngameState.Camera.Width;
      float height = (float) this.TheGame.IngameState.Camera.Height;
      float x = (float) (width / 2560.0) / (float) (width / (double) height / 1.6000000238418579);
      float y = height / 1600f;
      int blackBarSize = this.TheGame.BlackBarSize;
      float scale = this.TheGame.IngameState.UIRoot.Scale;
      System.Numerics.Vector2 vector2 = this.GetChainPos() * new System.Numerics.Vector2(x, y);
      return new SharpDX.RectangleF(vector2.X + (float) blackBarSize, vector2.Y, x * this.Width * this.Scale / scale, y * this.Height * this.Scale / scale);
    }

    [Obsolete]
    public virtual SharpDX.RectangleF GetClientRectWithTrans(SharpDX.Vector2 posTrans)
    {
      if (this.Address == 0L)
        return SharpDX.RectangleF.Empty;
      SharpDX.Vector2 vector2 = this.GetParentPos() + posTrans;
      double width = (double) this.TheGame.IngameState.Camera.Width;
      float height = (float) this.TheGame.IngameState.Camera.Height;
      float num1 = (float) (width / 2560.0) / (float) (width / (double) height / 1.6000000238418579);
      float num2 = height / 1600f;
      float scale = this.TheGame.IngameState.UIRoot.Scale;
      return new SharpDX.RectangleF((vector2.X + this.X * this.Scale / scale) * num1, (vector2.Y + this.Y * this.Scale / scale) * num2, num1 * this.Width * this.Scale / scale, num2 * this.Height * this.Scale / scale);
    }

    [Obsolete]
    public virtual SharpDX.RectangleF GetRect(SharpDX.Vector2 rootPos)
    {
      if (this.Address == 0L)
        return SharpDX.RectangleF.Empty;
      SharpDX.Vector2 vector2 = rootPos;
      double width = (double) this.TheGame.IngameState.Camera.Width;
      float height = (float) this.TheGame.IngameState.Camera.Height;
      float num1 = (float) (width / 2560.0) / (float) (width / (double) height / 1.6000000238418579);
      float num2 = height / 1600f;
      float scale = this.TheGame.IngameState.UIRoot.Scale;
      return new SharpDX.RectangleF((vector2.X + this.X * this.Scale / scale) * num1, (vector2.Y + this.Y * this.Scale / scale) * num2, num1 * this.Width * this.Scale / scale, num2 * this.Height * this.Scale / scale);
    }

    public Element FindChildRecursive(Func<Element, bool> condition)
    {
      if (condition(this))
        return this;
      foreach (Element child in (IEnumerable<Element>) this.Children)
      {
        Element childRecursive = child.FindChildRecursive(condition);
        if (childRecursive != null)
          return childRecursive;
      }
      return (Element) null;
    }

    public Element FindChildRecursive(string text, bool contains = false) => this.FindChildRecursive((Func<Element, bool>) (elem =>
    {
      if (elem.Text == text)
        return true;
      if (!contains)
        return false;
      string text1 = elem.Text;
      return text1 != null && text1.Contains(text);
    }));

    public Element GetChildFromIndices(params int[] indices)
    {
      Element childFromIndices = this;
      foreach (int index in indices)
      {
        childFromIndices = childFromIndices.GetChildAtIndex(index);
        if (childFromIndices == null)
        {
          string str = "";
          ((IEnumerable<int>) indices).ForEach<int>((Action<int>) (i =>
          {
            string str1 = str;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
            interpolatedStringHandler.AppendLiteral("[");
            interpolatedStringHandler.AppendFormatted<int>(i);
            interpolatedStringHandler.AppendLiteral("] ");
            string stringAndClear = interpolatedStringHandler.ToStringAndClear();
            str = str1 + stringAndClear;
          }));
          DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(34, 3);
          interpolatedStringHandler1.AppendFormatted(nameof (Element));
          interpolatedStringHandler1.AppendLiteral(" with index: ");
          interpolatedStringHandler1.AppendFormatted<int>(index);
          interpolatedStringHandler1.AppendLiteral(" not found. Indices: ");
          interpolatedStringHandler1.AppendFormatted(str);
          DebugWindow.LogMsg(interpolatedStringHandler1.ToStringAndClear());
          return (Element) null;
        }
        if (childFromIndices.Address == 0L)
        {
          string str = "";
          ((IEnumerable<int>) indices).ForEach<int>((Action<int>) (i =>
          {
            string str2 = str;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
            interpolatedStringHandler.AppendLiteral("[");
            interpolatedStringHandler.AppendFormatted<int>(i);
            interpolatedStringHandler.AppendLiteral("] ");
            string stringAndClear = interpolatedStringHandler.ToStringAndClear();
            str = str2 + stringAndClear;
          }));
          DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(34, 3);
          interpolatedStringHandler2.AppendFormatted(nameof (Element));
          interpolatedStringHandler2.AppendLiteral(" with index: ");
          interpolatedStringHandler2.AppendFormatted<int>(index);
          interpolatedStringHandler2.AppendLiteral(" 0 address. Indices: ");
          interpolatedStringHandler2.AppendFormatted(str);
          DebugWindow.LogMsg(interpolatedStringHandler2.ToStringAndClear());
          return this.GetObject<Element>(0L);
        }
      }
      return childFromIndices;
    }

    public Element GetChildAtIndex(int index)
    {
      if ((long) index >= this.ChildCount)
        return (Element) null;
      return this.GetObject<Element>(this.M.Read<long>(this.Address + (long) Element.ChildStartOffset, index * 8));
    }
  }
}
