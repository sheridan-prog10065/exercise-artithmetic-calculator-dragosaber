using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MathOperators;

public partial class MainPage : ContentPage
{
    /// <summary>
    /// Remembers the expressions that were calculated by the user
    /// </summary>
    private ObservableCollection<string> _expList;

    public MainPage()
    {
        InitializeComponent();

        _expList = new ObservableCollection<string>();
        _lstExpHistory.ItemsSource = _expList;
    }

    private async void OnCalculate(object sender, EventArgs e)
    {
        try
        {
            //Get the input to the arithmetic operation
            double leftOperand = double.Parse(_txtLeftOp.Text);
            double rightOperand = double.Parse(_txtRightOp.Text);

            //Obtain the character that represents the arithmetic operation
            //Cast to string is possible because SelectedItem is an Object
            //Extra paranthesis are needed to ensure the index operator is applied to the result of the cast
            char operation = ((string)_pckOperand.SelectedItem)[0];

            //Perform the arithmetic operation and obtain the result
            double result = PerformArithmeticOperation(operation, leftOperand, rightOperand);

            // Display the arithmetic calculation to the user. Show the work
            string expression = $"{leftOperand} {operation} {rightOperand} = {result}";

            //Remember the expression
            _expList.Add(expression);

            _txtMathExp.Text = expression;
        }
        catch (ArgumentNullException ex)
        {
            //The user did not provide any input
            await DisplayAlert("Error", "Please provide the required input", "OK");
            await DisplayAlert("MORE", "More details about this error", "OK");
        }
        catch(FormatException ex)
        {
            //The user has provided input but it is incorrect (e.g not a number)
            await DisplayAlert("Error", "Please provide the correct input", "OK");
        }
        catch (DivideByZeroException ex)
        {
            //The user has is executing a division and the denominator is zero
            await DisplayAlert("Error", "Please provide a non-zero denominator", "OK");
        }
    }

    private double PerformArithmeticOperation(char operation, double leftOperand, double rightOperand)
    {
        switch (operation)
        {
            case '+':
                return PerformAddition(leftOperand, rightOperand);

            case '-':
                return PerformSubtraction(leftOperand, rightOperand);

            case '*':
                return PerformMultiplication(leftOperand, rightOperand);

            case '/':
                return PerformDivision(leftOperand, rightOperand);

            case '%':
                return PerformDivisionRemainder(leftOperand, rightOperand);

            default:
                Debug.Assert(false, "Unknown operand. Cannot perform the arithmetic opertaion");
                return 0;
        }
    }

    private double PerformAddition(double leftOperand, double rightOperand)
    {
        return leftOperand + rightOperand;
    }

    private double PerformSubtraction(double leftOperand, double rightOperand)
    {
        return leftOperand - rightOperand;
    }
    private double PerformMultiplication(double leftOperand, double rightOperand)
    {
        return leftOperand * rightOperand;
    }

    private double PerformDivision(double leftOperand, double rightOperand)
    {
        string divOp = _pckOperand.SelectedItem as string; //another way of casting used for objects
        if (divOp.Contains("Int", StringComparison.OrdinalIgnoreCase))
        {
            //Integer division is preformed when both the operands are integers
            int intLeftOp = (int)leftOperand;
            int intRightOp = (int)rightOperand;
            int result = intLeftOp / intRightOp;
            return result;
        }
        else
        {
            //Real division
            return leftOperand / rightOperand;
        }
    }

    private double PerformDivisionRemainder(double leftOperand, double rightOperand)
    {
        return leftOperand % rightOperand;
    }
}
