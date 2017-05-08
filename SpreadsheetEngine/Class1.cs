//Git Commit Test of Changes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; //for INotifyPropertyChanged
using System.IO;
using System.Xml;

namespace CptS321
{
    public abstract class Cell : INotifyPropertyChanged
    {
        protected int rowI;
        protected int colI;
        protected uint bgColor;
        protected string text; //actual text for cell
        protected string value; //evaluates the value property (ie. if = is before the string, it's a formula)
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; //call the event upon property change

        /*Cell constructor*/
        public Cell()
        {

        }

        /*RowIndex property*/
        public int RowIndex { get { return this.rowI; } protected set { this.rowI = value; } }

        /*ColIndex property*/
        public int ColIndex { get { return this.colI; } protected set { this.colI = value; } }

        /*Background color propert*/
        public uint BGColor
        {
            get { return this.bgColor; }
            set
            {
                if (value == this.bgColor)
                    return;
                this.bgColor = value;
                OnPropertyChanged("BGColor");
            }
        }

        /*Text property*/
        public string Text
        {
            get { return this.text; }
            set
            {
                if (value == this.text)
                    return;
                this.text = value;
                OnPropertyChanged("Text"); //notify when "Text" property changes
            }
        }

        /*Value property*/
        public string Value
        {
            get { return this.value; }
            protected internal set
            {
                if (value == this.value)
                    return;
                this.value = value;
                OnPropertyChanged("Value");
            }
        }


        protected void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null) //does the event have subscribers?
                PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }


    //////////////////////////////////////////

    /*Class to instantiate a Cell in spreadsheet*/
    public class UsedCell : Cell
    {
        public UsedCell(int rowi, int coli, string text)
        {
            this.RowIndex = rowi;
            this.ColIndex = coli;
            this.Text = "";
            this.BGColor = 4294967295;
            this.value = "";
        }
    }

    //////////////////////////////////////////
    public interface ICmd
    {
        ICmd Exec();
    }

    /*undo/redo for cell text*/
    public class RestoreText : ICmd
    {
        private Cell cell;
        private string text;
        public RestoreText(Cell c, string txt)
        {
            cell = c;
            text = txt;
        }
        public ICmd Exec()
        {
            var inverse = new RestoreText(cell, cell.Value);
            cell.Value = text;
            return inverse;
        }
    }

    /*undo/redo for background color*/
    public class RestoreBG : ICmd
    {
        private Cell cell;
        private uint bgColor;

        public RestoreBG(Cell c, uint bg)
        {
            cell = c;
            bgColor = bg;
        }

        public ICmd Exec()
        {
            var inverse = new RestoreBG(cell, cell.BGColor);
            cell.BGColor = bgColor;
            return inverse;
        }
    }

    public class MultiCmd : ICmd
    {
        List<ICmd> commands;

        public MultiCmd()
        {
            commands = new List<ICmd>();
        }

        public void addCommands(ICmd com)
        {
            commands.Add(com);
        }

        public ICmd Exec()
        {
            var inverse = new MultiCmd();
            for(int i = commands.Count-1; i >= 0;i--)
            {
                inverse.commands.Add(commands[i].Exec());
            }
            return inverse;
        }
    }

    /////////////////////////////////////////
    public class Spreadsheet
    {
        private int rowCount, colCount;
        private Cell[,] ss;
        private Stack<ICmd> undoStack = new Stack<ICmd>();
        private Stack<ICmd> redoStack = new Stack<ICmd>();
        private Dictionary<Cell,HashSet<Cell>> dependencies = new Dictionary<Cell,HashSet<Cell>>(); //dictionary to map all cell dependencies
        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };
        public Spreadsheet(int rows, int cols)
        {
            this.rowCount = rows;
            this.colCount = cols;
            ss = new Cell[rows, cols]; //create spreadsheet (2d array of cells)
            for (int r = 0; r < rows; r++) //initialize each cell to proper row/col index, text = ""
            {
                for (int c = 0; c < cols; c++)
                {
                    ss[r, c] = new UsedCell(r, c, ""); //instantiate every index of the 2d array to a cell
                    ss[r, c].PropertyChanged += Spreadsheet_PropertyChanged; //"subscribe" to each cell
                }
            }
        }

        //returns a cell at a given index in the spreadsheet
        public Cell getCell(int rowI, int colI)
        {
            if (ss[rowI, colI] == null) //check for valid index
                return null;
            else
                return ss[rowI, colI];
        }

        /*returns the number of columns in the spreadsheet*/
        public int ColumnCount
        {
            get { return this.colCount; }
        }

        /*returns the number of rows in the spreadsheet*/
        public int RowCount
        {
            get { return this.rowCount; }
        }

        /*Generates text in 50 random cells of the spreadsheet,
         also generates */
        //public void generateTextInCells()
        //{
        //    Random getRow = new Random(); //random number generation (based off CPU)
        //    Random getCol = new Random();

        //    for (int i = 0; i < 2; i++)
        //    {
        //        ss[getRow.Next(0, 50), getCol.Next(0, 26)].Text = "Hello Cody!"; //generate 50 random Cells to say "Hello Cody!"
        //    }

        //    for (int j = 0; j < 50; j++)
        //    {
        //        ss[j, 1].Text = String.Format("This is cell B{0}", j + 1); //make all Cells in B column say This is B#
        //        ss[j, 0].Text = String.Format("=B{0}", j + 1); //set all A Cells to value of corresponding B Cell
        //    }
        //}


        /*Gets a cell value by taking a string variable and matching it to the corresponding cell, returns the value of that cell
         -Used to determine value of individual variables so the exp. tree can evaluate formulas*/
        public double getCorrespondingCellValue(string s)
        {
            s = "=" + s;
            int col = s[1] - 'A'; //returns the ASCII value for the letter, - 65 to get corresponding row
            int row;
            if(Int32.TryParse((s.Substring(2)), out row)) //check for valid input
            {
                double result;
                if(double.TryParse(getCell(row-1, col).Value, out result))
                {
                    return result;
                }
                return 0.0;
            }
            else
                return 0.0; //cell value used was null or empty string or invalid entry
        }

        /*Function getCorrespondingCellName() gets the cell name from row and col values*/
        public string getCorrespondingCellName(int row, int col)
        {
            char cellLetter;
            int cellRowNumber;
            string cellName = "";

            cellLetter = (char)(col+65); //get corresponding cell letter
            cellRowNumber = row + 1; //get corresponding cell row num
            cellName = cellLetter.ToString() + cellRowNumber.ToString();

            return cellName;
        }

        /*Returns a cell corresponding to a string variable
         -Used to update depenedency table*/
        public Cell getCorrespondingCell(string s)
        {
            s = "=" + s;
            int col = s[1] - 'A'; //returns the ASCII value for the letter, - 65 to get corresponding row
            int row = Int32.Parse(s.Substring(2)) - 1; //returns the integer part of the formula (minus 1 for index)
            
            return getCell(row, col); //return the value of the corresponding cell variable
        }

        /*Adds an undo command to the undoStack*/
        public void AddUndo(ICmd inverse)
        {
            undoStack.Push(inverse);
        }

        public ICmd popUndo()
        {
            return undoStack.Pop();
        }

        /*Adds a redo command to the redoStack, only called when undo button is clicked*/
        public void AddRedo(ICmd inverse)
        {
            redoStack.Push(inverse);
        }

        public ICmd popRedo()
        {
            return redoStack.Pop();
        }

        /*check if undo stack is empty*/
        public bool isUndoStackEmpty()
        {
            if (undoStack.Count == 0)
                return true;
            return false;
        }

        /*check if redo stack is empty*/
        public bool isRedoStackEmpty()
        {
            if (redoStack.Count == 0)
                return true;
            return false;
        }

        /*Updates Dictionary<Cell,HashSet<Cell>> of all the cell dependencies*/
        public void updateDependencyTable(Cell referenceCell, string value)
        {
            Cell key = getCorrespondingCell(value); //get the key, value is coming is as a variable such as 'A1'
            if (!dependencies.ContainsKey(key)) 
            {
                dependencies.Add(key, new HashSet<Cell>());
                dependencies[key].Add(referenceCell); //add the cell to the set of dependencies that corresponds to the key cell
            }
            else
            {
                if (!dependencies[key].Contains(referenceCell))
                {
                    dependencies[key].Add(referenceCell);
                }
            }
        }

        /*Function clearSpreadsheetData() clears all data within the spreadsheet. Everything goes back to default spreadsheet*/
        public void clearSpreadsheetData()
        {
            dependencies.Clear();
            for (int r = 0; r < this.RowCount; r++) //initialize each cell to proper row/col index, text = ""
            {
                for (int c = 0; c < this.ColumnCount; c++)
                {
                    ss[r, c].BGColor = 4294967295;
                    ss[r, c].Text = "";
                }
            }
        }

        public void saveSpreadsheet(Stream s)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter myWrite = XmlWriter.Create(s, settings); //create an XML writer with the stream to XML file, proper settings
            myWrite.WriteStartDocument();
            myWrite.WriteStartElement("Spreadsheet");
            for (int r = 0; r < this.RowCount; r++) //go through cells to find which cells have changed properties to save
            {
                for (int c = 0; c < this.ColumnCount; c++)
                {
                    if(ss[r,c].BGColor != 4294967295 && ss[r,c].Text != "") //check for both background and text property changed
                    {
                        string cellName = getCorrespondingCellName(r, c); //get the cell name from the row and col
                        myWrite.WriteStartElement("cell");
                        myWrite.WriteAttributeString("name", cellName);
                        myWrite.WriteStartElement("bgcolor");
                        myWrite.WriteString(ss[r, c].BGColor.ToString());
                        myWrite.WriteEndElement();
                        myWrite.WriteStartElement("text");
                        myWrite.WriteString(ss[r, c].Text);
                        myWrite.WriteEndElement();
                        myWrite.WriteEndElement();
                    }
                    else if(ss[r,c].BGColor != 4294967295) //check if only background color needs to be saved
                    {
                        string cellName = getCorrespondingCellName(r, c);
                        myWrite.WriteStartElement("cell");
                        myWrite.WriteAttributeString("name", cellName);
                        myWrite.WriteStartElement("bgcolor");
                        myWrite.WriteString(ss[r, c].BGColor.ToString());
                        myWrite.WriteEndElement();
                        myWrite.WriteEndElement();
                    }
                    else if(ss[r,c].Text != "") //check if only cell text needs to be saved
                    {
                        string cellName = getCorrespondingCellName(r, c);
                        myWrite.WriteStartElement("cell");
                        myWrite.WriteAttributeString("name", cellName);
                        myWrite.WriteStartElement("text");
                        myWrite.WriteString(ss[r, c].Text);
                        myWrite.WriteEndElement();
                        myWrite.WriteEndElement();
                    }
                }
            }
            myWrite.WriteEndElement();
            myWrite.WriteEndDocument();
            myWrite.Close();
        }

        public void loadSpreadsheet(Stream s)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader myReader = XmlReader.Create(s, settings);
            string cellName = ""; //temp string to get cell name of XML cell and find corresponding cell in spreadsheet
            Cell temp = new UsedCell(0,0,""); //temp cell to get the row and col of XML cell

            this.clearSpreadsheetData(); //clear all current spreadsheet data to avoid merge

            while (myReader.Read())
            {
                if (myReader.IsStartElement("cell")) //is there a cell to load?
                {
                    if (myReader.HasAttributes) //does the cell have attributes (a name)?
                    {
                        cellName = myReader.GetAttribute("name"); //get the value of name attribute (cell name)
                        temp = this.getCorrespondingCell(cellName);
                    }
                }
                if(myReader.IsStartElement("bgcolor")) //is there a background color to load?
                {
                    uint bgcolor;
                    UInt32.TryParse(myReader.ReadElementContentAsString(), out bgcolor);
                    ss[temp.RowIndex, temp.ColIndex].BGColor = bgcolor;
                }
                if (myReader.IsStartElement("text")) //is there cell text to load?
                {
                    ss[temp.RowIndex, temp.ColIndex].Text = myReader.ReadElementContentAsString();
                }
            }
            myReader.Close();
            this.undoStack.Clear(); 
            this.redoStack.Clear();   
        }

        /*Checks that the entered formula is a valid formula
         Invalid formula examples: =AA1, =ZZ101, =Z12345, = Ba*/
        private bool checkValidFormulaVariables(string[] variables)
        {
            bool valid = true; //assume valid until proven otherwise
            foreach(string s in variables)
            {
                int col = s[0] - 'A'; //returns the ASCII value for the letter, - 65 to get corresponding row
                if (col > 25 || col < 0) //check for valid col range
                    valid = false;
                int row;
                if (Int32.TryParse((s.Substring(1)), out row)) //checking if rest of the variable is an int representing row number
                {
                    if ((row - 1) < 0 || (row - 1) > 50) //check row boundaries
                        valid = false;
                }
                else
                    valid = false;
                if (!valid) //if something was invalid in the formula, no need to check rest of variables in formula. Let user know asap
                    break;
            }
            return valid;
        }

        /*Checks for any variables in a cell's formula that self reference the cell by
         comparing the variable in the formulas' row/col with the row/col of the cell sender (self)*/
        private bool checkSelfReference(Cell self, string[] variables)
        {
            bool isSelfReference = true;
            Cell check;
            foreach (string s in variables)
            {
                int col = s[0] - 'A'; //returns the ASCII value for the letter, - 65 to get corresponding row
                int row;
                if (Int32.TryParse(s.Substring(1), out row)) //returns the integer part of the formula (minus 1 for index)
                {
                    isSelfReference = true;
                    check = getCell(row-1, col);
                    if (check.RowIndex != self.RowIndex || check.ColIndex != self.ColIndex) //check if row/col of sender cell = row/col of cell in formula
                        isSelfReference = false;
                    if (isSelfReference) //if self reference, exit loop no need to check rest of variables
                        break;
                }
            }
            return isSelfReference;
        }

        private void Spreadsheet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Text" == e.PropertyName) //checks if Cell.Text property was changed
            {
                string oldValue = ((Cell)sender).Value; //make a copy of the value of the old string for undo methods
                ICmd temp = new RestoreText((Cell)sender, oldValue);
                AddUndo(temp);
                if (((Cell)sender).Text.IndexOf('=') == 0) //must evaluate if it's a formula
                {
                    //remove cell from every hashset in dependency table to update the dependency table after formula evaluation
                    foreach (KeyValuePair<Cell, HashSet<Cell>> p in dependencies)
                    {
                        dependencies[p.Key].Remove((Cell)sender);
                    }

                    string formula = ((Cell)sender).Text.Replace("=", "");
                    var tree = new ExpTree(formula); //put the formula in tree for evaluation
                    string[] variables = tree.GetVars(); //get the variables from the tree
                    if (checkValidFormulaVariables(variables) && !checkSelfReference(((Cell)sender), variables)) //check if variables are in valid format
                    {
                        foreach (string s in variables)
                        {
                            double value = getCorrespondingCellValue(s); //get the individual variable value
                            tree.SetVar(s, value); //set the variable so the tree can evaluate the expression
                            updateDependencyTable((Cell)sender, s);
                        }
                        double finalValue = tree.Eval(); //returns the final result of formula evaluation
                        ((Cell)sender).Value = finalValue.ToString();
                    }
                    else if (!checkValidFormulaVariables(variables)) //variables not in valid format
                    {
                        tree.ClearVariables(); //delete bad variables
                        ((Cell)sender).Text = "!(invalid entry)";
                    }
                    else
                    {
                        tree.ClearVariables();
                        ((Cell)sender).Text = "!(self reference)";
                    }
                }
                else //just a constant value or some string was entered
                {
                    //check if any cells depend on the cell where value was changed and update them
                    if(dependencies.ContainsKey((Cell)sender) && dependencies[(Cell)sender].Count > 0)
                    {
                        foreach (Cell cell in dependencies[(Cell)sender])
                        {
                            //Re-evaluate the formulas of each cell depending on the changed cell
                            string formula = cell.Text.Replace("=","");
                            var tree = new ExpTree(formula);
                            string[] vars = tree.GetVars();
                            if (checkValidFormulaVariables(vars)) //check if variables are in valid format
                            {
                                foreach (string v in vars)
                                {
                                    Cell check = getCorrespondingCell(v);
                                    double varValue = 0.0;
                                    if (check == ((Cell)sender)) //check if the variable is the cell being changed
                                    {
                                        double.TryParse(((Cell)sender).Text, out varValue); //set value of that variable to the newly changed value
                                    }
                                    else
                                        varValue = getCorrespondingCellValue(v);
                                    tree.SetVar(v, varValue);
                                }
                                double finalValue = tree.Eval(); //returns the final result of formula evaluation
                                cell.Value = finalValue.ToString();
                            }
                            else
                                cell.Text = "!(invalid entry)";
                        }
                    }
                    ((Cell)sender).Value = ((Cell)sender).Text; //set the value of the cell
                }
            }

            if (CellPropertyChanged != null) //does the event have subscribers?
                CellPropertyChanged(sender, e);
        }
    }

    ///////////////////////////////////////

    public class ExpTree
    {
        private Node root;
        private static Dictionary<string, double> variables = new Dictionary<string, double>();

        public ExpTree(string expression)
        {
            this.root = Compile(expression); //sets the root to the last node recursively built bottom up, last node is the last operator to be evaluated
        }

        public void SetVar(string varName, double varValue) //adds the variable name and associated key to the dictionary of variables
        {
            if (!variables.ContainsKey(varName)) //check if the value is already in dict, if not add it
            {
                variables.Add(varName, varValue);
            }
            else //if value is already in dict, update it
                variables[varName] = varValue;
        }


        /*Get the names of all variables in the expression tree*/
        public string[] GetVars()
        {
            string[] vars = new string[variables.Count];
            int i = 0;
            foreach(KeyValuePair<string,double> p in variables)
            {
                vars[i] = p.Key;
                i++;
            }
            return vars;
        }

        public void ClearVariables()
        {
            variables.Clear();
        }

        public double Eval()
        {
            if (root != null) //tree isn't empty
                return root.Eval();
            else
                return double.NaN; //undefined result
        }

        public abstract class Node
        {
            public abstract double Eval(); //generic eval function
        }

        public class ConstantNode : Node //Node for constants
        {
            private double value; //value of the constant

            public ConstantNode(double num)
            {
                this.value = num;
            }

            public override double Eval()
            {
                return this.value; //returns the value of the constant
            }
        }

        public class VariableNode : Node //node for the variable name
        {
            private string name; //name of the variable

            public VariableNode(string term)
            {
                this.name = term;
                variables[this.name] = 0.0;
            }

            public override double Eval()
            {
                return variables[name]; //returns the value corresponding to the variable name in the dictionary
            }
        }

        public class OperationNode : Node //node for operators
        {
            private char op; //operator value
            private Node Left, Right; //operators have left and right children

            public OperationNode(char newOp, Node l, Node r)
            {
                this.op = newOp;
                this.Left = l;
                this.Right = r;
            }

            public override double Eval() //evaluates the op
            {
                double left = Left.Eval();
                double right = Right.Eval();
                switch (op)
                {
                    case '+':
                        return left + right; //add the left child to right
                    case '-':
                        return left - right; //subtract right child from left
                    case '/':
                        return left / right; //divide the left child by right
                    case '*':
                        return left * right; //multiply left and right child
                    default:
                        return 0;
                }
            }
        }

        private static Node Compile(string exp) //compiles the expression
        {
            exp = exp.Replace(" ", ""); //eliminate whitespace from expression

            for (int j = exp.Length - 1; j >= 0; j--) //go through the expression backwards to build the tree bottom up
            {
                if (exp[0] == '(') //check if expression is inside parentheses
                {
                    int count = 1; //saw an open parenthese, make count 1 to track levels of parentheses and --count when see closed
                    for (int i = 1; i < exp.Length; i++)
                    {
                        if (exp[i] == ')') //check for closed parentheses
                        {
                            count--; //closing off a level of parentheses
                            if (count == 0)
                            {
                                if (i == exp.Length - 1) //if at the end of the expression
                                {
                                    return Compile(exp.Substring(1, exp.Length - 2)); //compile inside the parentheses, length - 2 excludes the parentheses in the last index
                                }
                                else
                                    break;
                            }
                        }
                        if (exp[i] == '(') //if encountered another level of parentheses
                        {
                            count++;
                        }
                    }
                }
                switch (exp[j]) //evaluate each value in the expression, if operator then make a new node with the operator
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        int index = GetLowOperatorIndex(exp); //start building the tree at the lowest operator so it is the root and evaluated last
                        if (index != -1)
                        {
                            return new OperationNode(exp[index],
                                Compile(exp.Substring(0, index)),
                                Compile(exp.Substring(index + 1)));
                        }
                        break;
                }
            }
            return BuildSimple(exp);
        }

        private static Node BuildSimple(string term) //builds nodes out of each individual term
        {
            double num;
            if (double.TryParse(term, out num))
                return new ConstantNode(num); //term was an int
            return new VariableNode(term); //not an int
        }

        private static int GetLowOperatorIndex(string expression) //gets the lowest operator in the equation to build tree properly
        {
            int parenthesesCount = 0;
            int index = -1; //valid index check
            for(int i = expression.Length - 1; i >= 0; i--)
            {
                switch (expression[i])
                {
                    case ')':
                        parenthesesCount--;
                        break;
                    case '(':
                        parenthesesCount++;
                        break;
                    case '+':
                    case '-':
                        if (parenthesesCount == 0) //checks to see if outside all parenthese
                            return i;
                        break;
                    case '*':
                    case '/':
                        if (parenthesesCount == 0 && index == -1) //check we are outside all parantheses
                            index = i; //mark index to see if there is higher prescedence operators
                        break;
                }
            }
            return index;
        }
    }
}