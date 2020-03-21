using ClForms.Themes;

namespace ClForms.Elements
{
    /// <summary>
    /// Represents a Windows status bar control
    /// </summary>
    public class StatusBar: DockPanel
    {
        private Color mnemonicForeground;

        /// <summary>
        /// Возвращает или задаёт значение, указывающее цвет назначенного символа, связанного с доступом к кнопкам.
        /// </summary>
        public Color MnemonicForeground
        {
            get => mnemonicForeground;
            set
            {
                if (mnemonicForeground != value)
                {
                    mnemonicForeground = value;
                    InvalidateVisual();
                }
            }
        }
    }
}
