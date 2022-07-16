namespace DrawingWithButtons
{
    public partial class MainForm : Form
    {
        #region Global Variables

        // Stores all buttons drawn by user
        List<Button> BUTTONLIST = new List<Button>();

        // Stores total number of buttons created even after deletion (exception - failing in drawing button)
        int BUTTONCOUNTER = 0;

        // Stores an origin/pivot point of button (for height/width calculations)
        Point ORIGIN_POINT;

        // If user is drawing...
        bool isDrawing = false;

        #endregion

        #region MainForm

        public MainForm()
        {
            InitializeComponent();
        }

        // Updates count of currently placed buttons in Main Form Label
        private void UpdateMainFormLabel()
        {
            this.Text = "Drawing With Buttons! #: " + BUTTONLIST.Count;
        }

        #endregion

        #region Button

        private Button NewButton(MouseEventArgs e)
        {
            var button = new Button();
            button.Text = "Button " + BUTTONCOUNTER;
            button.Location = new Point(e.X, e.Y);
            button.Size = new Size(0, 0);
            button.BackColor = RandomColor();

            button.Click += RemoveButton;
            this.Controls.Add(button);

            return button;
        }

        private void RemoveButton(object? sender, EventArgs e)
        {
            var button = sender as Button;

            BUTTONLIST.Remove(button);
            this.Controls.Remove(button);

            UpdateMainFormLabel();
        }

        #endregion

        #region Drawing With Buttons

        private void DrawStart(object sender, MouseEventArgs e)
        {
            var button = NewButton(e);
            ORIGIN_POINT = button.Location;

            BUTTONLIST.Add(button);
            BUTTONCOUNTER++;

            isDrawing = true;
        }

        private void DrawContinue(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;

            var button = BUTTONLIST[BUTTONLIST.Count - 1];
            int width = e.X - ORIGIN_POINT.X;
            int height = e.Y - ORIGIN_POINT.Y;

            if (width > 0 && height > 0)        // Bottom Right
            {
                button.Size = new Size(width, height);
            }
            else if (width < 0 && height > 0)   // Bottom Left
            {
                button.Location = new Point(e.X, ORIGIN_POINT.Y);
                button.Size = new Size(Math.Abs(width), height);
            }
            else if (width > 0 && height < 0)   // Up Right
            {
                button.Location = new Point(ORIGIN_POINT.X, e.Y);
                button.Size = new Size(width, Math.Abs(height));
            }
            else if (width < 0 && height < 0)   // Up Left
            {
                button.Location = new Point(e.X, e.Y);
                button.Size = new Size(Math.Abs(width), Math.Abs(height));
            }
        }

        private void DrawEnd(object sender, MouseEventArgs e)
        {
            var button = BUTTONLIST[BUTTONLIST.Count - 1];

            if (button.Width < 30 || button.Height < 20)
            {
                RemoveButton(button, e);
                BUTTONCOUNTER--;
            }

            isDrawing = false;

            UpdateMainFormLabel();
        }

        #endregion

        #region Service Methods

        private Color RandomColor()
        {
            var rand = new Random();
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        #endregion
    }
}