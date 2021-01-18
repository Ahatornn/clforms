namespace MazeCommon
{
    /// <summary>
    /// Maze entities
    /// </summary>
    public enum MapItem
    {
        /// <summary>
        /// Value for show user path as leftwards arrow
        /// </summary>
        PathShowedLeft = -5,

        /// <summary>
        /// Value for show user path as downwards arrow
        /// </summary>
        PathShowedDown = -4,

        /// <summary>
        /// Value for show user path as rightwards arrow
        /// </summary>
        PathShowedRight = -3,

        /// <summary>
        /// Value for show user path as upwards arrow
        /// </summary>
        PathShowedTop = -2,

        /// <summary>
        /// Value for generated maze algorithm
        /// </summary>
        Visited = -1,

        /// <summary>
        /// Empty
        /// </summary>
        Empty = 0,

        /// <summary>
        /// Wall
        /// </summary>
        Wall = 1,

        /// <summary>
        /// Start point (only one)
        /// </summary>
        StartPoint,

        /// <summary>
        /// Exit point (only one)
        /// </summary>
        EndPoint,

        /// <summary>
        /// Door
        /// </summary>
        Door,

        /// <summary>
        /// Key
        /// </summary>
        Key,

        /// <summary>
        /// 10 coins
        /// </summary>
        TenCoins,

        /// <summary>
        /// 20 coins
        /// </summary>
        TwentyCoins,
    }
}
