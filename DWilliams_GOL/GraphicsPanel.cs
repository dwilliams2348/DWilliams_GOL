using System.Windows.Forms;

// Change the namespace to your project's namespace.
namespace DWilliams_GOL
{
    class GraphicsPanel : Panel
    {
        private ToolStrip toolStrip1;

        // Default constructor
        public GraphicsPanel()
        {
            // Turn on double buffering.
            this.DoubleBuffered = true;

            // Allow repainting when the window is resized.
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(100, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.ResumeLayout(false);

        }
    }
}
