using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using wManager;

namespace MarsSettingsGUITestApp
{
    class Program
    {
        static void Main(string[] args)
        {        
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
            Console.ReadLine();
        }

        private static void ThreadStartingPoint()
        {
            TestSetting.Load();
            var settingWindow = new MarsSettingsGUI.SettingsWindow(TestSetting.CurrentSetting, "Green");
            settingWindow.ShowDialog();
            TestSetting.CurrentSetting.Save();
            //System.Windows.Threading.Dispatcher.Run();            
        }
    }
}
