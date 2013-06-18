using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MouseThingy;
using MSG = System.Windows.Interop.MSG;

namespace MouseThingyGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static WindowControlGroups _ctrGroup;

        public MainWindow()
        {
            InitializeComponent();


            //        EventManager.RegisterClassHandler(typeof(WindowListViewItem),
            //WindowListViewItem.MouseLeftButtonDownEvent,
            //new RoutedEventHandler(WindowListViewItem.));


            var listenThread = new Thread(() =>
                {
                    _ctrGroup = new WindowControlGroups();
                    new MouseHooker(_ctrGroup);
                    new KeyboardHooker(_ctrGroup);

                    while (true)
                    {
                        //   Console.WriteLine("t");
                        MouseThingy.MSG msg;
                        User32MouseDefinitions.POINT p;
                        User32Import.GetCursorPos(out p);
                        var tmp = User32Import.WindowFromPoint(p);
                        //var tmpres = User32Import.PeekMessage(out msg, IntPtr.Zero, 0, int.MaxValue, 1);
                        Console.WriteLine("first" + (uint)WindowsMessages.KEYFIRST);
                        Console.WriteLine("last " + (uint)WindowsMessages.KEYLAST);
                        var result = User32Import.GetMessage(out msg, IntPtr.Zero, (uint)WindowsMessages.KEYFIRST,
                                                             (uint)WindowsMessages.KEYLAST);
                        Console.WriteLine(result);
                        if (result > 0)
                        {
                            User32Import.TranslateMessage(ref msg);
                            User32Import.DispatchMessage(ref msg);
                        }
                        else if (result == 0)
                        {
                            Console.WriteLine("quit");
                            // quiting, remove hooks
                        }
                        else
                        {
                            // error
                        }
                    }

                });
            listenThread.Start();


            while (_ctrGroup == null)
            {

            }
            _ctrGroup.PropertyChanged += WindowsChanged;

        }

        private void WindowsChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(Refresh);
        }

        public class WindowListViewItem : ListViewItem
        {
            public WindowControlGroups.Window Window;
            public WindowListViewItem(WindowControlGroups.Window w)
            {
                Window = w;
                PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                PreviewMouseRightButtonDown += OnMouseRightButtonDown;
            }

            public void OnMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
            {
                var w = (sender as WindowListViewItem).Window;
                var tmp = (sender as WindowListViewItem);

                _ctrGroup.RemoveWindowFromGroup(w.Key, w);

            }

            public void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
            {
                var w = (sender as WindowListViewItem).Window;
                w.BringToFront();
            }





        }

        private void Refresh()
        {
            var listView = List;
            listView.Items.Clear();
            var groups = _ctrGroup.GetWindowGroups();

            foreach (var key in groups.Keys)
            {
                var listViewForKey = new ListView();


                if (_ctrGroup.GetCurrentControlGroup() == key)
                {
                    var item = new ListViewItem { Content = key };
                    item.Background = Brushes.Red;
                    listViewForKey.Items.Add(item);

                }
                else
                {

                    var item = new ListViewItem { Content = key };

                    listViewForKey.Items.Add(item);


                }

                List<WindowControlGroups.Window> windows;
                if (groups.TryGetValue(key, out windows))
                {
                    foreach (var window in windows)
                    {

                        var content = string.Format("key {0} psName {1}", window.Key,
                                                    window.GetWindowProcess().ProcessName);
                        if (!(window.GetWindowProcess().ProcessName == "Idle"))
                        {
                            var item = new WindowListViewItem(window) { Content = content };
                            listViewForKey.Items.Add(item);
                        }
                    }
                }
                listView.Items.Add(listViewForKey);
            }
        }


        private void OnClick(object sender, RoutedEventArgs e)
        {
            Refresh();
        }








    }
}
