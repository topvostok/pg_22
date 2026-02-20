using ClassModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PhoneBook_Мыльников.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private List<Call> allCalls = new List<Call>();
        private DateTime? filterDateFrom = null;
        private DateTime? filterDateTo = null;
        private string filterPhoneNumber = "";
        private int filterCallType = 0;

        private void LoadAllCalls()
        {
            MainWindow.connect.LoadData(ClassConnection.Connection.tables.calls);
            allCalls = new List<Call>(MainWindow.connect.calls);
        }

        private void ShowCalls(List<Call> callsToShow)
        {
            parent.Children.Clear();
            foreach (var call in callsToShow)
            {
                parent.Children.Add(new Elements.Call_itm(call));
            }
            parent.Children.Add(new Elements.Add_itm(new Pages.PagesUser.Call_win(new Call())));
        }

        public enum page_main
        {
            users, calls, none
        };
        public static page_main page_select;

        public Main()
        {
            InitializeComponent();
            page_select = page_main.none;
            filterPanel.Visibility = Visibility.Collapsed;
        }
        private void Click_Phone(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible)
            {
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            }
            filterPanel.Visibility = Visibility.Collapsed;
            if (page_select != page_main.users)
            {
                page_select = page_main.users;


                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                opacityAnimation.Completed += delegate
                {
                    parent.Children.Clear();
                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.2);
                    opacityAnimation2.Completed += delegate
                    {
                        Dispatcher.InvokeAsync(async () =>
                        {
                            MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);

                            foreach (User user_item in MainWindow.connect.users)
                            {
                                if (page_select == page_main.users)
                                {
                                    parent.Children.Add(new Elements.User_itm(user_item));
                                    await Task.Delay(90);
                                }
                            }

                            if (page_select == page_main.users)
                            {
                                var ff = new Pages.PagesUser.User_win(new User());
                                parent.Children.Add(new Elements.Add_itm(ff));
                            }
                        });
                    };
                    parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation2);
                };
                parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation);
            }
        }

        private void Click_History(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible)
            {
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            }
            filterPanel.Visibility = Visibility.Visible;
            if (page_select != page_main.calls)
            {
                page_select = page_main.calls;
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                opacityAnimation.Completed += delegate
                {
                    parent.Children.Clear();
                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.2);
                    opacityAnimation2.Completed += delegate
                    {
                        Dispatcher.InvokeAsync(async () =>
                        {
                            MainWindow.connect.LoadData(ClassConnection.Connection.tables.calls);
                            foreach (Call call_itm in MainWindow.connect.calls)
                            {
                                if (page_select == page_main.calls)
                                {
                                    parent.Children.Add(new Elements.Call_itm(call_itm));
                                    await Task.Delay(90);
                                }
                            }
                            if (page_select == page_main.calls)
                            {
                                var ff = new Pages.PagesUser.Call_win(new ClassModule.Call());
                                parent.Children.Add(new Elements.Add_itm(ff));
                            }
                        });
                    };
                    parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation2);
                };
                parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation);

                LoadAllCalls();
                ApplyFilters();
            }
        }
        public void Anim_move(Control control1, Control control2, Frame frame_main = null, Page pages = null, page_main page_restart = page_main.none)
        {
            if (page_restart != page_main.none)
            {
                if (page_restart == page_main.users)
                {
                    page_select = page_main.none;
                    Click_Phone(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.calls)
                {
                    page_select = page_main.none;
                    Click_History(new object(), new RoutedEventArgs());
                }
            }
            else
            {
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.3);
                opacityAnimation.Completed += delegate
                {
                    if (pages != null)
                    {
                        frame_main.Navigate(pages);
                        //if (control1 == frame_main && control2 == frame_main)
                        // if (MainWindow.actualUser.role != "admin")
                        // {
                        //     parent.Children.Clear();
                        // }
                    }

                    control1.Visibility = Visibility.Hidden;
                    control2.Visibility = Visibility.Visible;

                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.4);

                    control2.BeginAnimation(ScrollViewer.OpacityProperty, opacityAnimation2);
                };

                control1.BeginAnimation(ScrollViewer.OpacityProperty, opacityAnimation);
            }
        }
        private void ApplyFilters()
        {
            List<Call> filteredCalls = allCalls;

            if (filterDateFrom.HasValue || filterDateTo.HasValue)
            {
                filteredCalls = filteredCalls.Where(call =>
                {
                    try
                    {
                        string datePart = call.Time_start.Split(' ')[0];
                        string[] dateParts = datePart.Split('.');

                        DateTime callDate = new DateTime(
                            int.Parse(dateParts[2]),
                            int.Parse(dateParts[1]),
                            int.Parse(dateParts[0]));

                        if (filterDateFrom.HasValue && callDate < filterDateFrom.Value.Date)
                            return false;

                        if (filterDateTo.HasValue && callDate > filterDateTo.Value.Date)
                            return false;

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }).ToList();
            }

            if (filterCallType != 0)
            {
                filteredCalls = filteredCalls.Where(call => call.Category_call == filterCallType).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filterPhoneNumber))
            {
                filteredCalls = filteredCalls.Where(call =>
                {
                    User user = MainWindow.connect.users.Find(x => x.Id == call.User_id);
                    if (user != null)
                    {
                        return user.Phone_num.Contains(filterPhoneNumber);
                    }
                    return false;
                }).ToList();
            }

            ShowCalls(filteredCalls);
        }

        public void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            filterDateFrom = dateFromPicker.SelectedDate;
            filterDateTo = dateToPicker.SelectedDate;

            if (cbCallType.SelectedItem is ComboBoxItem selectedType)
            {
                filterCallType = Convert.ToInt32(selectedType.Tag);
            }

            filterPhoneNumber = tbPhoneNumber.Text.Trim();

            if (filterDateFrom.HasValue && filterDateTo.HasValue)
            {
                if (filterDateFrom.Value > filterDateTo.Value)
                {
                    MessageBox.Show("Такого не может быть");
                    return;
                }
            }
            ApplyFilters();
        }

        public void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            dateFromPicker.SelectedDate = null;
            dateToPicker.SelectedDate = null;
            cbCallType.SelectedIndex = 0;
            tbPhoneNumber.Text = "";

            filterDateFrom = null;
            filterDateTo = null;
            filterCallType = 0;
            filterPhoneNumber = "";

            ApplyFilters();
        }
    }
}
