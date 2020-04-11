using System;
using ClForms.Abstractions;
using ClForms.Abstractions.Engine;
using ClForms.Common.EventArgs;

namespace ClForms.Elements
{
    /// <summary>
    /// Defines the char tiling element
    /// </summary>
    public class TilePanel: Canvas, IElementStyle<TilePanel>
    {
        private char tile;

        /// <summary>
        /// Initialize a new instance <see cref="TilePanel"/>
        /// </summary>
        public TilePanel()
        {
            tile = '\0';
        }

        /// <summary>
        /// Initialize a new instance <see cref="TilePanel"/>
        /// </summary>
        public TilePanel(char tile)
            : this()
        {
            this.tile = tile;
        }

        #region Properties

        #region Tile

        /// <summary>
        /// Gets or sets the tiling char
        /// </summary>
        public char Tile
        {
            get => tile;
            set
            {
                if (tile != value)
                {
                    OnTileChanged?.Invoke(this, new PropertyChangedEventArgs<char>(tile, value));
                    tile = value;
                    InvalidateVisual();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc cref="IElementStyle{T}.SetStyle"/>
        public void SetStyle(Action<TilePanel> styleAction) => styleAction?.Invoke(this);

        /// <inheritdoc />
        protected override void InheritRender(IDrawingContext context) => context.Release(Background, Foreground, tile);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value of the <see cref="Tile" /> property changes
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<char>> OnTileChanged;

        #endregion
    }
}
