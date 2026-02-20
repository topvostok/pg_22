using ClassConnection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PhoneBook_Мыльников
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Connection connect;
        public static Pages.Main main;
        public MainWindow()
        {
            InitializeComponent();
            connect = new Connection();
            connect.LoadData(Connection.tables.users);
            connect.LoadData(Connection.tables.calls);
            main = new Pages.Main();
            OpenPageMain();
        }

        public void OpenPageMain()
        {
            DoubleAnimation opgridAnimation = new DoubleAnimation();
            opgridAnimation.From = 1;
            opgridAnimation.To = 0;
            opgridAnimation.Duration = TimeSpan.FromSeconds(0.6);
            opgridAnimation.Completed += delegate
            {
                frame.Navigate(main);

                DoubleAnimation opgrisAnimation = new DoubleAnimation();
                opgrisAnimation.From = 0;
                opgrisAnimation.To = 1;
                opgrisAnimation.Duration = TimeSpan.FromSeconds(1.2);
                frame.BeginAnimation(Frame.OpacityProperty, opgrisAnimation);
            };

            frame.BeginAnimation(Frame.OpacityProperty, opgridAnimation);
        }

    }
}
