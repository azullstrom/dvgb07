using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Lotto
{
    public sealed partial class MainPage : Page
    {
        private const int MIN_VAL = 1, MAX_VAL = 36, LOTTO_ROW_LENGTH = 7, MIN_ITER = 1, MAX_ITER = 1000000;
        private int seven, six, five;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Start_Lotto_Click(object sender, RoutedEventArgs e)
        {
            seven = 0;
            six = 0;
            five = 0;

            TextBox[] textBoxes = {N1_TextBox, N2_TextBox, N3_TextBox, N4_TextBox, N5_TextBox, N6_TextBox, N7_TextBox};

            if(NoDuplicates(textBoxes))
            {
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    string text = textBoxes[i].Text;

                    if (int.TryParse(text, out int value))
                    {
                        if (!IsBetweenNumbers(value, MIN_VAL, MAX_VAL))
                        {
                            Debug.WriteLine("Not valid input. Must be an integer between (inclusive) 1-35");
                            break;
                        }
                        if (i == textBoxes.Length - 1)
                        {
                            if (int.TryParse(Iterations_TextBox.Text, out int result))
                            {
                                if (!IsBetweenNumbers(result, MIN_ITER, MAX_ITER))
                                {
                                    Debug.WriteLine("Not valid input. Iterations must be a number between (inclusive) 1-999999");
                                    break;
                                }
                                Debug.WriteLine(result + " iterations");
                            }
                            else
                            {
                                Debug.WriteLine("Not valid input. Iterations must be a number");
                                break;
                            }
                            Start_Lotto();
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Not valid input. Only integers allowed.");
                        break;
                    }
                }
            } 
        }
        
        private void Start_Lotto()
        {
            List<int> list = new List<int>
            {
                int.Parse(N1_TextBox.Text),
                int.Parse(N2_TextBox.Text),
                int.Parse(N3_TextBox.Text),
                int.Parse(N4_TextBox.Text),
                int.Parse(N5_TextBox.Text),
                int.Parse(N6_TextBox.Text),
                int.Parse(N7_TextBox.Text)
            };
            list.Sort();

            int iterations = int.Parse(Iterations_TextBox.Text);
            Random random = new Random();
            for (int i = 0; i < iterations; i++)
            {
                List<int> random_list = new List<int>();
                
                for(int j = 0; j < LOTTO_ROW_LENGTH; j++)
                {
                    while(true)
                    {
                        int rand = random.Next(MIN_VAL, MAX_VAL);
                        if (!random_list.Contains(rand)) {
                            random_list.Add(rand);
                            break;
                        }
                    }
                }
                random_list.Sort();

                List<int> correct_list = list.Intersect(random_list).ToList();

                if(correct_list.Count == 7)
                {
                    seven++;
                } 
                else if(correct_list.Count == 6)
                {
                    six++;
                } 
                else if(correct_list.Count == 5)
                {
                    five++;
                } else
                {
                    //Something
                }
            }
            Seven_TextBox.Text = seven.ToString();
            Six_TextBox.Text = six.ToString();
            Five_TextBox.Text = five.ToString();
        }

        private bool IsBetweenNumbers(int input, int min, int max)
        {
            return input >= min && input < max;
        }

        private bool NoDuplicates(TextBox[] textBoxes)
        {
            for (int i = 0; i < textBoxes.Length - 1; i++)
            {
                for (int j = i + 1; j < textBoxes.Length; j++)
                {
                    if (textBoxes[i].Text == textBoxes[j].Text && textBoxes[i].Text != "")
                    {
                        Debug.WriteLine("Not valid input. Duplicates in lotto row.");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
