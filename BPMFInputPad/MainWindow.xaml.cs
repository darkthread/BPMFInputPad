using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BPMFInputPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string[] symbols = new string[] {
            "ㄅㄉˇˋㄓˊ˙ㄚㄞㄢㄦ",
            "ㄆㄊㄍㄐㄔㄗㄧㄛㄟㄣ←",
            "ㄇㄋㄎㄑㄕㄘㄨㄜㄠㄤ清",
            "ㄈㄌㄏㄒㄖㄙㄩㄝㄡㄥ送"
        };

        Key[,] keys = new Key[4,11]
        {
            { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0, Key.OemMinus },
            { Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P, Key.Back },
            { Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L, Key.OemSemicolon, Key.Escape },
            { Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M, Key.OemComma, Key.OemPeriod, Key.OemQuestion, Key.Enter }
        };

        Dictionary<Key, Button> keyMapping = new Dictionary<Key, Button>();



        private void Window_Initialized(object sender, EventArgs e)
        {
            Enumerable.Range(1, 4).ToList()
                .ForEach(i => BpmfKeysGrid.RowDefinitions.Add(new RowDefinition()));
            Enumerable.Range(1, 11).ToList()
                .ForEach(i => BpmfKeysGrid.ColumnDefinitions.Add(new ColumnDefinition()));
            for (var r = 0; r < 4; r++)
            {
                var line = symbols[r];
                for (var c = 0; c < line.Length; c++)
                {
                    var b = new Button()
                    {
                        Content = line[c].ToString(),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Margin = new Thickness(3),
                        Background = new SolidColorBrush(buttonBgColor)
                    };
                    Grid.SetRow(b, r);
                    Grid.SetColumn(b, c);
                    b.Click += Button_Click;
                    b.Tag = line[c].ToString();
                    keyMapping.Add(keys[r, c], b);
                    BpmfKeysGrid.Children.Add(b);
                }
            }

            animation = new ColorAnimation();
            animation.From = buttonBgColor;
            animation.To = Colors.Orange;
            animation.AutoReverse = true;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
        }

        Color buttonBgColor = Colors.LightGray;
        ColorAnimation animation;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            b.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            var s = b.Tag as string;
            switch (s)
            {
                case "←":
                    if (txtInput.Text.Length > 0)
                    {
                        txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
                    }
                    break;
                case "清":
                    txtInput.Text = string.Empty;
                    break;
                case "送":
                    Clipboard.SetText(txtInput.Text);
                    break;
                default:
                    txtInput.Text += s;
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyMapping.ContainsKey(e.Key))
            {
                var b = keyMapping[e.Key];
                Button_Click(b, null);
            }
            else
            {
                if (e.Key == Key.Space)
                    txtInput.Text += " ";
            }
        }
    }
}
