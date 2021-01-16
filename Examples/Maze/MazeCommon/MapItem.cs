namespace MazeCommon
{
    /// <summary>
    /// Maze entities
    /// </summary>
    public enum MapItem
    {
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
