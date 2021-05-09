using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Ambro.Drawing;

namespace FillShape
{
    // this program colours in empty spaces of a picture

    //sample text files are in the folder of the solution
    public partial class frmMain : Form
    {
        //form variables
        private Grid grid;
        private char[,] shape;
        private Color fillColor;

        public frmMain()
        {
            InitializeComponent();

            //initialize grid
            grid = new Grid(15, 40, 0, 30, 20);

            //default fill color to blue
            fillColor = Color.Maroon;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //draw grid          
            grid.DrawGrid(e.Graphics);
        }

        private void frmMain_MouseClick(object sender, MouseEventArgs e)
        {
            //determine which box the user clicked
            int col = e.X / 20;
            int row = (e.Y - 30) / 20;
            
            //fill shape if within shape or fill outside
            FillShape(row, col);

            //set up the grid cell components to display colour accordingly
            ConfigureGrid();

            //refesh to call OnPaint 
            this.Refresh();
        }

        //call FillShape method
        public void FillShape(int r, int c)
        {
            //check if index is in bounds of array
            if (r >= 0 && r < shape.GetLength(0) && c >= 0 && c < shape.GetLength(1))
            {
                //check if the box colour is white
                if (shape[r, c] == '.')
                {
                    //colour in white box with fillcolour
                    shape[r, c] = 'F';

                    //pass box that is next to current box in all four direction
                    FillShape(r + 1, c);
                    FillShape(r, c + 1);
                    FillShape(r - 1, c);
                    FillShape(r, c - 1);
                }
            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            //open selected file
            OpenFileDialog fileDialog = new OpenFileDialog();

            //check if file is a readable text file
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(fileDialog.OpenFile());

                //transfer text file values into array
                string line = sr.ReadLine();
                int cols = int.Parse(line);
                line = sr.ReadLine();
                int rows = int.Parse(line);
                shape = new char[rows, cols];

                for (int r = 0; r < rows; r++)
                {
                    line = sr.ReadLine();
                    for (int c = 0; c < cols; c++)
                    {
                        shape[r, c] = line[c];
                    }
                }
                //draw grid
                ConfigureGrid();
            }
            else
            {
            }

            //refresh form
            this.Refresh();
        }

        private void ConfigureGrid()
        {
            //store number of rows and columns of grid
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);

            //loop through each of the elements in grid
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    //change colour of each box depending on character
                    if (shape[r, c] == '.') grid.getCell(r, c).setBackColor(Color.White);
                    else if (shape[r, c] == 'T') grid.getCell(r, c).setBackColor(Color.Black);
                    else if (shape[r, c] == 'F') grid.getCell(r, c).setBackColor(fillColor);
                }
            }
        }

        private void mnuFileColor_Click(object sender, EventArgs e)
        {
            //create variable to store selected colour
            ColorDialog cd = new ColorDialog();

            //check if colour is selected
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //change fillcolour to selected colour
                fillColor = cd.Color;
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            //close form
            this.Close();
        }
    }
}