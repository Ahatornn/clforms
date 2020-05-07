using System;

namespace MazeCommon
{
    /// <summary>
    /// <see cref="MapItem"/> to <see langword="char"/> converter
    /// </summary>
    public static class MapItemCharConverter
    {
        /// <summary>
        /// Convert <see cref="MapItem"/> to <see langword="char"/>
        /// </summary>
        public static char ConvertToChar(MapItem value) => value switch
        {
            MapItem.Empty => '█',
            MapItem.Wall => '▓',
            MapItem.StartPoint => '☺',
            MapItem.EndPoint => '⌂',
            MapItem.Door => '◙',
            MapItem.Key => 'Ⱡ',
            MapItem.TenCoins => '⑩',
            MapItem.TwentyCoins => '⓴',
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }
}
