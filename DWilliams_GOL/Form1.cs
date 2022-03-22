using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DWilliams_GOL
{
    public partial class Form1 : Form
    {
        // The universe array
        //Default values for dimensions
        static int universeX = 25;
        static int universeY = 25;
        bool[,] universe = new bool[universeX, universeY];
        bool[,] scratchPad;


        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;
        Color backgroundColor = Color.White;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        //settings (randomize universe, turn on/off neighbor count, turn on/off grid, torodial universe on/off)
        //default values, can be changed in the settings by the user.
        bool randUniverse = false;
        bool toggleGrid = true;
        bool toggleCount = true;
        bool torodialUniverse = true;
        bool toggleHUD = true;

        //timer interval in milliseconds, default is 100
        int timerInterval = 100;

        public Form1()
        {
            InitializeComponent();
            timer.Interval = timerInterval;
            timer.Tick += Timer_Tick;
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            //Sets dead/alive state for cells in next gen
            int aliveCount = 0;
            CellState();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = scratchPad[x, y];

                    if (scratchPad[x, y]) aliveCount++;
                }
            }

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelAliveCount.Text = "Number of alive cells: " + aliveCount.ToString();

            graphicsPanel1.Invalidate();
            this.Update();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            //drawing background color
            graphicsPanel1.BackColor = backgroundColor;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    Font font = new Font("Arial", 12f);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    Rectangle rect = new Rectangle(cellRect.X, cellRect.Y, cellWidth, cellHeight);

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    if (toggleGrid)
                    {
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }

                    //draws the amout of neighbors a cell has
                    if (toggleCount)
                    {
                        if (torodialUniverse) e.Graphics.DrawString(CountNeighborsTorodial(x, y).ToString(), font, Brushes.Black, rect, stringFormat);
                        else e.Graphics.DrawString(CountNeighborsFinite(x, y).ToString(), font, Brushes.Black, rect, stringFormat);
                    }
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        //This starts the timer back up to continue iterating through generations
        private void StartButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        //When clicked it will pause the passage of time in the universe and won't go to next generations
        private void PauseButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        //When clicked it makes the universe jump ahead 1 generation
        private void NextButton_Click(object sender, EventArgs e)
        {
            if (randUniverse && generations == 0)
            {
                RandomUniverse();
            }

            NextGeneration();
        }

        //when pressed this button will clear the universe and set the generations count to 1 and redraws the screen
        private void ClearButton_Click(object sender, EventArgs e)
        {
            generations = 0;
            timer.Dispose();

            //this nested for loop clears the universe and the scratchpad
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                    scratchPad[x, y] = false;
                }
            }

            //updates the status labels at the end of the generation
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelAliveCount.Text = "Number of alive cells: " + 0;

            graphicsPanel1.Invalidate();
            this.Update();
        }

        //Counts amunt of neighbors a cell has, there are no borders on the universe in this check, edges wrap around
        private int CountNeighborsTorodial(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for (int yOffSet = -1; yOffSet <= 1; yOffSet++)
            {
                for (int xOffSet = -1; xOffSet <= 1; xOffSet++)
                {
                    int xCheck = x + xOffSet;
                    int yCheck = y + yOffSet;

                    if (xOffSet == 0 && yOffSet == 0) continue;

                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }

                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }

                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }

                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }

            return count;
        }

        //this method counts the neighbors around a cell in a finite universe, does not wrap to the other side.
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for (int yOffSet = -1; yOffSet <= 1; yOffSet++)
            {
                for (int xOffSet = -1; xOffSet <= 1; xOffSet++)
                {
                    int xCheck = x + xOffSet;
                    int yCheck = y + yOffSet;

                    if (xOffSet == 0 && yOffSet == 0) continue;

                    if (xCheck < 0) continue;

                    if (yCheck < 0) continue;

                    if (xCheck >= xLen) continue;

                    if (yCheck >= yLen) continue;

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }

            return count;
        }

        //Determines whether or not a cell will be alive in the next generation
        private void CellState()
        {
            scratchPad = new bool[universe.GetLength(0), universe.GetLength(1)];

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    //checks to see if the universe is going to be finite or torodial
                    if (torodialUniverse)
                    {
                        if ((CountNeighborsTorodial(x, y) < 2) && universe[x, y])
                        {
                            scratchPad[x, y] = false;
                        }
                        else if ((CountNeighborsTorodial(x, y) > 3) && universe[x, y])
                        {
                            scratchPad[x, y] = false;
                        }
                        else if ((CountNeighborsTorodial(x, y) == 2 || CountNeighborsTorodial(x, y) == 3) && universe[x, y])
                        {
                            scratchPad[x, y] = true;
                        }
                        else if ((CountNeighborsTorodial(x, y) == 3) && universe[x, y] == false)
                        {
                            scratchPad[x, y] = true;
                        }
                    }
                    else
                    {
                        if ((CountNeighborsFinite(x, y) < 2) && universe[x, y])
                        {
                            scratchPad[x, y] = false;
                        }
                        else if ((CountNeighborsFinite(x, y) > 3) && universe[x, y])
                        {
                            scratchPad[x, y] = false;
                        }
                        else if ((CountNeighborsFinite(x, y) == 2 || CountNeighborsTorodial(x, y) == 3) && universe[x, y])
                        {
                            scratchPad[x, y] = true;
                        }
                        else if ((CountNeighborsFinite(x, y) == 3) && universe[x, y] == false)
                        {
                            scratchPad[x, y] = true;
                        }
                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        //opens a modal dialog box that will contain settings for Time between generations, the size of the universe
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dia = new ModalDialog(universeX, universeY, timerInterval);

            //if the modal dialog is closed with ok then it updates all of the setting variables
            if (DialogResult.OK == dia.ShowDialog())
            {
                universeX = dia.xSize;
                universeY = dia.ySize;
                universe = new bool[dia.xSize, dia.ySize];

                timer.Interval = dia.genInterval;
                timerInterval = dia.genInterval;

                toolStripStatusLabelGenerationTime.Text = "Time between generations(ms): " + dia.genInterval;
                toolStripStatusLabelUniverseSize.Text = "Size of current Universe(X: " + dia.xSize + " Y: " + dia.ySize + ")";
            }

            graphicsPanel1.Invalidate();
            this.Update();
        }

        //toggles what type of universe the game will be played in torodial/finite
        private void universeTypeTorodialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            torodialUniverse = !torodialUniverse;

            if (torodialUniverse)
            {
                universeTypeTorodialToolStripMenuItem.Text = "Universe Type (Torodial)";
                universeTypeTorodialToolStripMenuItem.ToolTipText = "Torodial universe has edges that wrap around to the other side.";

                graphicsPanel1.Invalidate();
            }
            else
            {
                universeTypeTorodialToolStripMenuItem.Text = "Universe Type (Finite)";
                universeTypeTorodialToolStripMenuItem.ToolTipText = "Finite universe has boundaries on the edges of the map.";

                graphicsPanel1.Invalidate();
            }
        }

        //toggles whether or not the universe will be put into a random state when clicked
        private void randomizeUniverseOFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            randUniverse = !randUniverse;

            if (randUniverse)
            {
                randomizeUniverseOFFToolStripMenuItem.Text = "Randomize Universe (ON)";

                RandomUniverse();
            }
            else
            {
                randomizeUniverseOFFToolStripMenuItem.Text = "Randomize Universe (OFF)";
            }
        }

        //toggles neighbor count on/off within the cells
        private void toggleNeighborCountONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleCount = !toggleCount;

            if (toggleCount)
            {
                toggleNeighborCountONToolStripMenuItem.Text = "Toggle Neighbor Count (ON)";
            }
            else
            {
                toggleNeighborCountONToolStripMenuItem.Text = "Toggle Neighbor Count (OFF)";
            }

            graphicsPanel1.Invalidate();
        }

        //toggles cell grid on/off
        private void toggleGridONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleGrid = !toggleGrid;

            if (toggleGrid)
            {
                toggleGridONToolStripMenuItem.Text = "Toggle Grid (ON)";
            }
            else
            {
                toggleGridONToolStripMenuItem.Text = "Toggle Grid (OFF)";
            }

            graphicsPanel1.Invalidate();
        }

        //toggles the displayed information on/off (generation count, number of alive cells, time between generation, size of the universe)
        private void toggleHUDONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleHUD = !toggleHUD;

            if (toggleHUD)
            {
                toggleGridONToolStripMenuItem.Text = "Toggle HUD (ON)";
                toolStripStatusLabelGenerations.Visible = true;
                toolStripStatusLabelAliveCount.Visible = true;
                toolStripStatusLabelGenerationTime.Visible = true;
                toolStripStatusLabelUniverseSize.Visible = true;

                this.Update();
            }
            else
            {
                toggleGridONToolStripMenuItem.Text = "Toggle HUD (OFF)";
                toolStripStatusLabelGenerations.Visible = false;
                toolStripStatusLabelAliveCount.Visible = false;
                toolStripStatusLabelGenerationTime.Visible = false;
                toolStripStatusLabelUniverseSize.Visible = false;

                this.Update();
            }
        }

        //changes the color of the living cells in the universe
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dia = new ColorDialog();

            dia.Color = cellColor;

            if (DialogResult.OK == dia.ShowDialog())
            {
                cellColor = dia.Color;
            }

            graphicsPanel1.Invalidate();
        }

        //changes the color of the grid in the universe
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dia = new ColorDialog();

            dia.Color = gridColor;

            if (DialogResult.OK == dia.ShowDialog())
            {
                gridColor = dia.Color;
            }

            graphicsPanel1.Invalidate();
        }

        //changes background color
        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dia = new ColorDialog();

            dia.Color = backgroundColor;

            if (DialogResult.OK == dia.ShowDialog())
            {
                backgroundColor = dia.Color;
            }

            graphicsPanel1.Invalidate();
        }

        //saves the current universe
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "All Files|*.*|Cells|*.cells";
            dia.FilterIndex = 2;
            dia.DefaultExt = "cells";

            if (DialogResult.OK == dia.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dia.FileName);

                //Write comments beginning the line with !

                for (int y = 0; y < universeY; y++)
                {
                    //this string represents the current row
                    string universeRow = string.Empty;

                    for (int x = 0; x < universeX; x++)
                    {
                        //checks if the current cell it is writing is dead or alive
                        if (universe[x, y])
                        {
                            universeRow += "O";
                        }
                        else if (!universe[x, y])
                        {
                            universeRow += ".";
                        }
                    }

                    writer.WriteLine(universeRow);
                }

                writer.Close();
            }
        }

        //reads universe from plaintext file and copies universe
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "All Files|*.*|Cells|*.cells";
            dia.FilterIndex = 2;

            if (DialogResult.OK == dia.ShowDialog())
            {
                StreamReader reader = new StreamReader(dia.FileName);

                //determines height and width of the universe
                int universeX = 0;
                int universeY = 0;

                //goes through file to determine size of the universe
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();

                    if (row[0] == '!') continue;

                    if (row[0] != '!') universeY++;

                    if (universeX != 0) continue;

                    universeX = row.Length;
                }

                universe = new bool[universeX, universeY];
                scratchPad = new bool[universeX, universeY];

                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                //goes back through the universe to determine the alive/dead state of the cell
                int yCounter = 0;
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();

                    if (row[0] == '!') continue;

                    if (row[0] != '!')
                    {
                        for (int x = 0; x < row.Length; x++)
                        {
                            if (row[x] == 'O') universe[x, yCounter] = true;
                            if (row[x] == '.') universe[x, yCounter] = false;
                        }
                    }

                    yCounter++;
                }

                reader.Close();
            }

            graphicsPanel1.Invalidate();
        }

        //makes random universe if enabled in the settings
        private void RandomUniverse()
        {
            Random rand = new Random();

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    //determines if a cell will be dead or alive
                    if (rand.NextDouble() <= 0.5)
                    {
                        universe[x, y] = true;
                    }
                }
            }

            graphicsPanel1.Invalidate();
        }

        //pops up dialog when form is closing and asks user if they want to save the settings
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //asks the user if they want to save the changes made to settings to use later when reopened.
            DialogResult result = MessageBox.Show("Would you like to save the setting changes?", "Save?", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                SaveSettings();
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        //Saves settings to be used when the application is next opened
        private void SaveSettings()
        {
            Properties.Settings.Default["gridColor"] = gridColor;
            Properties.Settings.Default["cellColor"] = cellColor;
            Properties.Settings.Default["backgroundColor"] = backgroundColor;
            Properties.Settings.Default["randUniverse"] = randUniverse;
            Properties.Settings.Default["toggleGrid"] = toggleGrid;
            Properties.Settings.Default["toggleCount"] = toggleCount;
            Properties.Settings.Default["torodialUniverse"] = torodialUniverse;
            Properties.Settings.Default["toggleHUD"] = toggleHUD;
            Properties.Settings.Default["universeX"] = universeX;
            Properties.Settings.Default["universeY"] = universeY;
            Properties.Settings.Default["timerInterval"] = timerInterval;

            Properties.Settings.Default.Save();
        }

        //method is called whenever the last saved info needs to be obtained
        private void LoadUserSettings()
        {
            gridColor = (Color)Properties.Settings.Default["gridColor"];
            cellColor = (Color)Properties.Settings.Default["cellColor"];
            backgroundColor = (Color)Properties.Settings.Default["backgroundColor"];
            randUniverse = (bool)Properties.Settings.Default["randUniverse"];
            toggleGrid = (bool)Properties.Settings.Default["toggleGrid"];
            toggleCount = (bool)Properties.Settings.Default["toggleCount"];
            torodialUniverse = (bool)Properties.Settings.Default["torodialUniverse"];
            toggleHUD = (bool)Properties.Settings.Default["toggleHUD"];
            universeX = (int)Properties.Settings.Default["universeX"];
            universeY = (int)Properties.Settings.Default["universeY"];
            timerInterval = (int)Properties.Settings.Default["timerInterval"];

            universe = new bool[universeX, universeY];

            toolStripStatusLabelGenerationTime.Text = "Time between generations(ms): " + timerInterval;
            toolStripStatusLabelUniverseSize.Text = "Size of current Universe(X: " + universeX + " Y: " + universeY + ")";

            //this series of if statements sets the text in the context menu to correctly resemble the loaded settings
            if (toggleHUD)
            {
                toggleGridONToolStripMenuItem.Text = "Toggle HUD (ON)";
                toolStripStatusLabelGenerations.Visible = true;
                toolStripStatusLabelAliveCount.Visible = true;
                toolStripStatusLabelGenerationTime.Visible = true;
                toolStripStatusLabelUniverseSize.Visible = true;
            }
            else
            {
                toggleGridONToolStripMenuItem.Text = "Toggle HUD (OFF)";
                toolStripStatusLabelGenerations.Visible = false;
                toolStripStatusLabelAliveCount.Visible = false;
                toolStripStatusLabelGenerationTime.Visible = false;
                toolStripStatusLabelUniverseSize.Visible = false;
            }

            if (torodialUniverse)
            {
                universeTypeTorodialToolStripMenuItem.Text = "Universe Type (Torodial)";
                universeTypeTorodialToolStripMenuItem.ToolTipText = "Torodial universe has edges that wrap around to the other side.";
            }
            else
            {
                universeTypeTorodialToolStripMenuItem.Text = "Universe Type (Finite)";
                universeTypeTorodialToolStripMenuItem.ToolTipText = "Finite universe has boundaries on the edges of the map.";
            }

            if (randUniverse)
            {
                randomizeUniverseOFFToolStripMenuItem.Text = "Randomize Universe (ON)";

                RandomUniverse();
            }
            else
            {
                randomizeUniverseOFFToolStripMenuItem.Text = "Randomize Universe (OFF)";
            }

            if (toggleCount)
            {
                toggleNeighborCountONToolStripMenuItem.Text = "Toggle Neighbor Count (ON)";
            }
            else
            {
                toggleNeighborCountONToolStripMenuItem.Text = "Toggle Neighbor Count (OFF)";
            }

            if (toggleGrid)
            {
                toggleGridONToolStripMenuItem.Text = "Toggle Grid (ON)";
            }
            else
            {
                toggleGridONToolStripMenuItem.Text = "Toggle Grid (OFF)";
            }

            graphicsPanel1.Invalidate();
            this.Update();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUserSettings();
        }

        //this button reverts the user settings to the last saved settings
        private void revertButton_Click(object sender, EventArgs e)
        {
            LoadUserSettings();

            graphicsPanel1.Invalidate();
            this.Update();
        }

        //this button resets the user defined settings to default values
        private void resetButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
            LoadUserSettings();
        }
    }
}