using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Kalkylator
{
    public sealed partial class MainPage : Page
    {
        private TextBox resultTextBox;
        private TextBlock inputTextBlock;
        private char currentOperation, previousOperation;
        private int result, leftNumber, rightNumber;
        private bool newNumberState, equalsPressed, ongoingOperation, initState, divisionByZero, intMaxValueExceeded;

        public MainPage()
        {
            this.InitializeComponent();
            resultTextBox = ResultTextBox;
            inputTextBlock = InputTextBlock;
            result = 0;
            newNumberState = true;
            equalsPressed = false;
            ongoingOperation = false;
            initState = true;
            intMaxValueExceeded = false;
            IsButtonsExceptClearClickable(true);
        }

        private void NumberButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = (Button)sender;
            string btnText = clickedBtn.Tag.ToString();
            IsButtonsExceptClearClickable(true);

            if(newNumberState && currentOperation == '=')
            {
                NumberEnteredAfterEqualsReset(btnText);
            }
            else if(newNumberState) {
                resultTextBox.Text = btnText;
                newNumberState = false;
            }
            else
            {
                resultTextBox.Text += btnText;
            }
            ongoingOperation = false;
        }

        private void OperationButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = (Button)sender;
            currentOperation = char.Parse(clickedBtn.Tag.ToString());
            initState = false;

            if(ongoingOperation)
            {
                inputTextBlock.Text = resultTextBox.Text + " " + currentOperation;
            }
            else
            {
                if (inputTextBlock.Text.Length == 0)
                {
                    if(int.TryParse(resultTextBox.Text, out int resultNumber))
                    {
                        leftNumber = resultNumber;
                        inputTextBlock.Text = resultTextBox.Text + " " + currentOperation;
                    }
                    else
                    {
                        Debug.WriteLine("Max value exceeded");
                        intMaxValueExceeded = true;
                        PrintOperationsResult();
                    }
                }
                else
                {
                    if(equalsPressed)
                    {
                        leftNumber = result;
                        inputTextBlock.Text = leftNumber + " " + currentOperation;
                        equalsPressed = false;
                    }
                    else
                    {
                        if(int.TryParse(resultTextBox.Text, out int resultNumber))
                        {
                            rightNumber = resultNumber;
                            result = Calculate(leftNumber, rightNumber, previousOperation);
                            leftNumber = Calculate(leftNumber, rightNumber, previousOperation);
                        }
                        else
                        {
                            Debug.WriteLine("Max value exceeded");
                            intMaxValueExceeded = true;
                        }
                        PrintOperationsResult();
                    }
                }
                newNumberState = true;
                ongoingOperation = true;
            }
            previousOperation = currentOperation;
        }

        private void PrintOperationsResult()
        {
            if(divisionByZero)
            {
                resultTextBox.Text = "Division by zero";
                inputTextBlock.Text = "";
                IsButtonsExceptClearClickable(false);
                divisionByZero = false;
            }
            else if(intMaxValueExceeded)
            {
                resultTextBox.Text = "Max int value";
                IsButtonsExceptClearClickable(false);
                intMaxValueExceeded = false;
            }
            else
            {
                inputTextBlock.Text = leftNumber + " " + currentOperation;
                resultTextBox.Text = result.ToString();
            }
        }

        private void EqualsButtonClick(object sender, RoutedEventArgs e)
        {
            if(!initState)
            {
                if(currentOperation == '=')
                {
                    leftNumber = int.Parse(resultTextBox.Text);
                    inputTextBlock.Text = leftNumber + " " + previousOperation + " " + rightNumber + " =";
                    result = Calculate(leftNumber, rightNumber, previousOperation);
                }
                else
                {
                    if(int.TryParse(resultTextBox.Text, out int resultNumber))
                    {
                        rightNumber = resultNumber;
                        inputTextBlock.Text = leftNumber + " " + previousOperation + " " + rightNumber + " =";
                        result = Calculate(leftNumber, rightNumber, previousOperation);
                        leftNumber = Calculate(leftNumber, rightNumber, previousOperation);
                    }
                    else
                    {
                        Debug.WriteLine("Max value exceeded");
                        intMaxValueExceeded = true;
                    }
                }

                PrintEqualsResult();
                newNumberState = true;
                currentOperation = '=';
                equalsPressed = true;
            }
        }

        private void PrintEqualsResult()
        {
            if(divisionByZero)
            {
                resultTextBox.Text = "Division by zero";
                IsButtonsExceptClearClickable(false);
                divisionByZero = false;
            }
            else if(intMaxValueExceeded)
            {
                resultTextBox.Text = "Max int value";
                IsButtonsExceptClearClickable(false);
                intMaxValueExceeded = false;
            }
            else
            {
                resultTextBox.Text = result.ToString();
            }
        }

        private void IsButtonsExceptClearClickable(bool clickable)
        {
            BtnPlus.IsEnabled = clickable;
            BtnMinus.IsEnabled = clickable;
            BtnMult.IsEnabled = clickable;
            BtnDivide.IsEnabled = clickable;
            BtnEquals.IsEnabled = clickable;
            Btn1.IsEnabled = clickable;
            Btn2.IsEnabled = clickable;
            Btn3.IsEnabled = clickable;
            Btn4.IsEnabled = clickable;
            Btn5.IsEnabled = clickable; 
            Btn6.IsEnabled = clickable;
            Btn7.IsEnabled = clickable;
            Btn8.IsEnabled = clickable;
            Btn9.IsEnabled = clickable;
            Btn0.IsEnabled = clickable;
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            resultTextBox.Text = "0";
            inputTextBlock.Text = "";
            result = 0;
            leftNumber = 0;
            rightNumber = 0;
            newNumberState = true;
            equalsPressed = false;
            ongoingOperation = false;
            initState = true;
            IsButtonsExceptClearClickable(true);
        }

        private void NumberEnteredAfterEqualsReset(string buttonTag)
        {
            inputTextBlock.Text = "";
            resultTextBox.Text = buttonTag; 
            result = 0;
            leftNumber = 0;
            rightNumber = 0;
            newNumberState = false;
            equalsPressed = false;
            ongoingOperation = false;
            initState = true;
        }

        private int Calculate(int leftNumber, int rightNumber, char storedOperation)
        {
            switch(storedOperation)
            {
                case '+':
                    if ((long)leftNumber + (long)rightNumber <= int.MaxValue)
                    {
                        return leftNumber + rightNumber;
                    }
                    else
                    {
                        Debug.WriteLine("Max value exceeded");
                        intMaxValueExceeded = true;
                        return 0;
                    }
                case '-':
                    return leftNumber - rightNumber;
                case 'X':
                    if ((long)leftNumber * (long)rightNumber <= int.MaxValue)
                    {
                        return leftNumber * rightNumber;
                    }
                    else
                    {
                        Debug.WriteLine("Max value exceeded");
                        intMaxValueExceeded = true;
                        return 0;
                    }
                case '/':
                    if (rightNumber != 0)
                    {
                        return leftNumber / rightNumber;
                    }
                    else
                    {
                        Debug.WriteLine("Division by zero");
                        divisionByZero = true;
                        return 0;
                    }
                default:
                    Debug.WriteLine("Invalid operation");
                    return 0;
            }
        }
    }
}
