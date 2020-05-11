using ClForms.Common;
using ClForms.Elements;

namespace MazeEditor.Forms
{
    public partial class MapViewerWindow
    {
        private void InitializeComponent()
        {
            panel1 = new Panel()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                AutoSize = false,
            };

            canvas1 = new Canvas()
            {
                Width = 45,
                Height = 15,
            };
            canvas1.OnPaint += CanvasPaint;
            panel1.AddContent(canvas1);

            AddContent(panel1);
        }

        public Panel panel1;
        private Canvas canvas1;
    }
}
