using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseThingy
{
    public class WindowControlGroups
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
            else
            {

            }


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
                    User32Import.MoveWindow(window.Handle, window.PositionInfo.left, window.PositionInfo.top, window.PositionInfo.right - window.PositionInfo.left, window.PositionInfo.bottom - window.PositionInfo.top, true);
                    User32Import.SetForegroundWindow(window.Handle);
                }
            }

        }



        public class Window
        {
            public IntPtr Handle;
            public RECT PositionInfo;
            public string Name;

            public Window(string name, IntPtr handle, RECT position)
            {
                this.Name = String.IsNullOrEmpty(name) ? name : "";
                this.Handle = handle;
                this.PositionInfo = position;
            }

            public override bool Equals(object obj)
            {
                if (obj is Window)
                {
                    return (obj as Window).Handle == Handle;
                }


                return false;
            }


        }

    }
}
