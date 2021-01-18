using ClForms.Common;
using ClForms.Themes;

namespace MazeCommon
{
    /// <summary>
    /// Helper of represent color for <see cref="MapItem"/>
    /// </summary>
    public static class MapItemColorHelper
    {
        /// <summary>
        /// Gets <see cref="ContextColorPoint"/> based <see cref="MapItem"/> and isSelect
        /// </summary>
        public static ContextColorPoint GetColorPointForMapItem(MapItem item, bool isSelect)
            => item switch
            {
                MapItem.Wall => new ContextColorPoint(Color.Black, isSelect ? Color.Magenta : Color.DarkGray),
                MapItem.Door => new ContextColorPoint(Color.Black, isSelect ? Color.Magenta : Color.DarkGray),
                MapItem.StartPoint => new ContextColorPoint(isSelect ? Color.Magenta : Color.Black,
                    isSelect ? Color.White : Color.Cyan),
                MapItem.EndPoint => new ContextColorPoint(isSelect ? Color.Magenta : Color.Black,
                    isSelect ? Color.White : Color.DarkCyan),
                MapItem.Key => new ContextColorPoint(isSelect ? Color.Magenta : Color.Black,
                    isSelect ? Color.White : Color.DarkGreen),
                MapItem.TenCoins => new ContextColorPoint(isSelect ? Color.Magenta : Color.Black,
                    isSelect ? Color.White : Color.Yellow),
                MapItem.TwentyCoins => new ContextColorPoint(isSelect ? Color.Magenta : Color.Black,
                    isSelect ? Color.White : Color.Yellow),
                var pathShowed when
                    pathShowed == MapItem.PathShowedDown ||
                    pathShowed == MapItem.PathShowedLeft ||
                    pathShowed == MapItem.PathShowedRight ||
                    pathShowed == MapItem.PathShowedTop
                => new ContextColorPoint(isSelect ? Color.Magenta : Color.Black, Color.White),
                _ => new ContextColorPoint(Color.Black, isSelect ? Color.Magenta : Color.Black)
            };
    }
}
