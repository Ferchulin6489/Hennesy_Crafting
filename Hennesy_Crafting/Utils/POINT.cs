using SharpDX;
namespace Hennesy_Crafting.Utils
{
    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator Point(POINT point) => new Point(point.X, point.Y);
    }
}
