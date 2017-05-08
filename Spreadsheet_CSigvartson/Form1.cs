/*Cody Sigvartson, 11418590*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CptS321; //namespace of spreadsheet
using System.IO;

namespace Spreadsheet_CSigvartson
{
    public partial class Form1 : Form
    {
        Spreadsheet mySpreadsheet;
       
        public Form1()
        {
            InitializeComponent();
            mySpreadsheet = new Spreadsheet(50, 26);
            
            mySpreadsheet.CellPropertyChanged += MySpreadsheet_CellPropertyChanged; //"subscribe" to event: spreadsheet cell property was changed
            
                
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersWidth = 50;
         
            string cols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //string to name cols
            foreach (char c in cols)
            {
                dataGridView1.Columns.Add(c.ToString(), c.ToString()); //name each col 'A', 'B', etc
            }
            int j = 1; //variable to number rows
            for (int i = 0; i < 50; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = j.ToString();
                j++; 
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    mySpreadsheet.generateTextInCells();
        //}

        private void MySpreadsheet_CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if("Value" == e.PropertyName)
            {
                dataGridView1.Rows[((Cell)sender).RowIndex].Cells[((Cell)sender).ColIndex].Value = ((Cell)sender).Value;
            }
            if("BGColor" == e.PropertyName)
            {
                dataGridView1.Rows[((Cell)sender).RowIndex].Cells[((Cell)sender).ColIndex].Style.BackColor = UIntToColor(((Cell)sender).BGColor);
            }
        }

        /*Sets the data grid view value to the text of the corresponding spreadsheet engine cell*/
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = mySpreadsheet.getCell(e.RowIndex, e.ColumnIndex).Text;
        }

        /*Sets the data grid view value to the value of the corresponding spreadsheet engine cell*/
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                mySpreadsheet.getCell(e.RowIndex, e.ColumnIndex).Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }

        private void undoCellTextChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICmd temp = mySpreadsheet.popUndo();
            mySpreadsheet.AddRedo(temp.Exec());
            redoCellTextChangeToolStripMenuItem.Enabled = true;
        }

        private void redoCellTextChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICmd temp = mySpreadsheet.popRedo();
            mySpreadsheet.AddUndo(temp.Exec());
            undoCellTextChangeToolStripMenuItem.Enabled = true;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySpreadsheet.isUndoStackEmpty())
                undoCellTextChangeToolStripMenuItem.Enabled = false;
            else
                undoCellTextChangeToolStripMenuItem.Enabled = true;
            if (mySpreadsheet.isRedoStackEmpty())
                redoCellTextChangeToolStripMenuItem.Enabled = false;
            else
                redoCellTextChangeToolStripMenuItem.Enabled = true;
        }

        /*Converts a .Color to a UInt value, saw this method on stackoverflow*/
        private uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0));
        }

        /*Convers a UInt to a .Color, saw this method on stack overflow*/
        private Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        private void changeCellBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog();
            MultiCmd allCommands = new MultiCmd();
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                foreach(DataGridViewCell c in dataGridView1.SelectedCells)
                {
                    uint argb = ColorToUInt(myDialog.Color);
                    Cell tempCell = mySpreadsheet.getCell(c.RowIndex, c.ColumnIndex);
                    ICmd temp = new RestoreBG(tempCell, tempCell.BGColor);
                    allCommands.addCommands(temp);
                    mySpreadsheet.getCell(c.RowIndex, c.ColumnIndex).BGColor = argb;
                }
                mySpreadsheet.AddUndo(allCommands);
            }
        }

        /*Make a call to save the current spreadsheet to an XML format file*/
        private void saveSpreadhseetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream s;
            SaveFileDialog save = new SaveFileDialog(); //brings up dialog for user to find file to save to
            save.Title = "Save File As";
            if (save.ShowDialog() == DialogResult.OK) //checks the file is ok
            {
                if ((s = save.OpenFile()) != null) //checks file was opened properly, s is assigned to type file stream
                {
                    mySpreadsheet.saveSpreadsheet(s);
                }
                s.Close();
            }
        }

        /*Make a call to load a saved XML formatted spreadsheet file*/
        private void loadSpreadhseetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream s;
            OpenFileDialog load = new OpenFileDialog(); //brings up dialog for user to find file to save to
            load.Title = "Choose an XML file to load";
            if (load.ShowDialog() == DialogResult.OK) //checks the file is ok
            {
                if ((s = load.OpenFile()) != null) //checks file was opened properly, s is assigned to type file stream
                {
                    mySpreadsheet.loadSpreadsheet(s);
                }
                s.Close();
            }
        }
    }
}
