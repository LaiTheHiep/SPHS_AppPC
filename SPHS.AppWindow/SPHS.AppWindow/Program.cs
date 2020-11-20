using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPHS.AppWindow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Utils.AsyncDataDevice();
                    Thread.Sleep(Parameter_Special.TIME_ASYNC_DEVICE);
                }
            })
            { IsBackground = true }.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            //Application.Run(new SimulinkDevice());
        }
    }
}
