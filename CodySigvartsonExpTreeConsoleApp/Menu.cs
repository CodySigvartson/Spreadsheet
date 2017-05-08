/*Cody Sigvartson, 11418590*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace CodySigvartsonExpTreeConsoleApp
{
    class Menu
    {
        private int menuSelection;
        private string expression = " "; //default expression
        private string varName = " "; //varName for user to set variables
        private double varValue; //varValue for user to set value to variable name
        private ExpTree expTree; //global dict

        public Menu(string Expression = " ", string VarName = " ", double VarValue = double.NaN)
        {
            this.expression = Expression;
            this.varName = VarName;
            this.varValue = VarValue;
            expTree = new ExpTree(expression);
        }

        public void displayMenu()
        {
            while(true)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("-----------");
                Console.WriteLine("1: Enter a new expression");
                Console.WriteLine("2: Set a variable value");
                Console.WriteLine("3: Evaluate expression");
                Console.WriteLine("4: Quit");
                Console.WriteLine("-----------");
                this.menuSelection = Int32.Parse(Console.ReadLine());
                switch (this.menuSelection)
                {
                    case 1: //Enter a new expression
                        expTree.ClearVariables(); //clear all variables for new expression
                        this.expression = getExpression(); //gets an expression from the user to be evaluated
                        expTree = new ExpTree(this.expression); //builds an expression tree with the user entered expression
                        break;
                    case 2: //Set a variable value
                        setVariable();
                        expTree.SetVar(this.varName, this.varValue); //sets the variable in the dictionary of variables for the tree
                        break;
                    case 3: //Evaluate an expression
                        double result = 0;
                        result = expTree.Eval(); //returns the evaluated expression result
                        Console.WriteLine("Evaluated expression is: {0}", result);
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public string getExpression()
        {
            string exp = "";
            Console.WriteLine("Enter new expression: ");
            exp = Console.ReadLine();
            return exp;
        }

        public void setVariable()
        {
            Console.WriteLine("Enter variable name: ");
            this.varName = Console.ReadLine();
            Console.WriteLine("Enter the value for the variable name you just entered: ");
            this.varValue = Int32.Parse(Console.ReadLine());
        }
    }
}
