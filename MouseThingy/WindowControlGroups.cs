using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MouseThingy.Annotations;

namespace MouseThingy
{
    public class WindowControlGroups : INotifyPropertyChanged
    {
        private readonly Dictionary<string, List<Window>> _windows;
        public WindowControlGroups()
        {
            _windows = new Dictionary<string, List<Window>>();
        }

        public void AddWindowToGroup(string key, Window window)
        {
            CreateControlGroup(key);
            List<Window> list;
            if (_windows.TryGetValue(key, out list))
            {
                if (!list.Contains(window))
                {
                    Console.WriteLine("Added window with name {2} handle {0} and rect {1} to group {3}", window.Handle,
                                      window.PositionInfo.ToString(), window.Name, key);
                    list.Add(window);
                }
                else
                {
                    foreach (var win in list)
                    {
                        if (win.Handle == window.Handle)
                        {
                            if (win.PositionInfo.bottom != window.PositionInfo.bottom
                                || win.PositionInfo.top != window.PositionInfo.top
                                || win.PositionInfo.left != window.PositionInfo.left
                                || win.PositionInfo.right != window.PositionInfo.right)
                            {
                                Console.WriteLine("window was in list, but it has new position when saving. Changing position");
                                win.PositionInfo = window.PositionInfo;
                            }
                        }
                    }


                    Console.WriteLine("Window already added");
                }
            }
            OnPropertyChanged(key);


        }

        public void RemoveWindowFromGroup(string key, Window window)
        {
            List<Window> list;
           
            if (_windows.TryGetValue(key, out list))
            {
                Console.WriteLine("Removed window with name {2} handle {0} and rect {1} to group {3}", window.Handle,
                                   window.PositionInfo.ToString(), window.Name, key);
                list.Remove(window);
            }
            OnPropertyChanged(key);

        }

        public void CreateControlGroup(string key)
        {
            List<Window> list;
            if (!_windows.TryGetValue(key, out list))
            {
                _windows.Add(key, new List<Window>());
            }
        }

        private string _currentControlGroup = VirtualKeyShort.KEY_1.ToString();
        public string GetCurrentControlGroup()
        {
            Console.WriteLine("Got current control group to " + _currentControlGroup);
            return _currentControlGroup;
        }

        public void SetCurrentControlGroup(string key)
        {
            Console.WriteLine("Set current control group to "+key);
            
            List<Window> list;
            if (_windows.TryGetValue(key, out list))
            {
                Console.WriteLine("containt {0} windows", list.Count);
            }
            _currentControlGroup = key;

            OnPropertyChanged(key);
        }

        public void ShowControlGroup(string key)
        {
            List<Window> list;
            if (_windows.TryGetValue(key, out list))
            {
                var reversed = new List<Window>();
                reversed.AddRange(list);
                reversed.Reverse();
                foreach (var window in reversed)
                {
                   window.BringToFront();
                }
            }
            OnPropertyChanged(key);
        }

        public Dictionary<string, List<Window>> GetWindowGroups()
        {
            return _windows;
        }



        public class Window
        {
            public IntPtr Handle;
            public RECT PositionInfo;
            public string Name;
            public string Key;

            public void BringToFront()
            {
                User32Import.ShowWindow(Handle, ShowWindowCommands.Normal);
                User32Import.MoveWindow(Handle, PositionInfo.left, PositionInfo.top, PositionInfo.right - PositionInfo.left, PositionInfo.bottom - PositionInfo.top, true);
                User32Import.SetForegroundWindow(Handle);
            }


            public Process GetWindowProcess()
            {
                int processId;
                var result = User32Import.GetWindowThreadProcessId(Handle, out processId);
                var process = Process.GetProcessById(processId);
                return process;
            }

            public Window(string key, IntPtr handle, RECT position)
            {
                Key = key;
                Handle = handle;
                PositionInfo = position;
            }

            public override bool Equals(object obj)
            {
                if (obj is Window)
                {
                    return (obj as Window).Handle == Handle;
                }


                return false;
            }

            public void KillWindow()
            {
                GetWindowProcess().Kill();
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
