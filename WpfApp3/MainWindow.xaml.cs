using System;
using System.Text;
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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // <summary> Класс для подключения базы данных и других плюшек
        public static Connection connect;
        // <summary> Страница Main
        public static Pages.Main main;

        public MainWindow()
        {
            InitializeComponent();

            // инициализируем класс подключения к базе данных
            connect = new Connection();

            // загружаем данные о таблице пользователей
            connect.LoadData(Connection.tables.users);

            // загружаем данные о таблице телефонных номеров
            connect.LoadData(Connection.tables.calls);

            // инициализируем страницу main
            main = new Pages.Main();

            // вызываем функцию открытия страницы Main
            OpenPageMain();
        }

        public void OpenPageMain()
        {
            // анимация исчезновения
            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = 1;
            fadeOutAnimation.To = 0;
            fadeOutAnimation.Duration = TimeSpan.FromSeconds(0.6);

            fadeOutAnimation.Completed += delegate
            {
                // переходим на страницу
                frme.Navigate(main);

                // анимация появления
                DoubleAnimation fadeInAnimation = new DoubleAnimation();
                fadeInAnimation.From = 0;
                fadeInAnimation.To = 1;
                fadeInAnimation.Duration = TimeSpan.FromSeconds(1.2);

                // начинаем выполнение анимации появления
                frme.BeginAnimation(Frame.OpacityProperty, fadeInAnimation);
            };

            // начинаем выполнение анимации исчезновения
            frme.BeginAnimation(Frame.OpacityProperty, fadeOutAnimation);
        }
    }
}