﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JonLibrary.Common;
using JonLibrary.Automatic;
namespace MicroDAQ
{
    static class Program
    {
        public static int waitMillionSecond = 180000;
        public static bool BeQuit;
        public static CycleTask RemoteCycle;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            #region 处理来自参数的快速启动请求，跳过对OPCSERVER的三分钟等待
            foreach (string arg in args)
            {
                if (arg.Contains("fast"))
                {
                    waitMillionSecond = 1000;
                    break;
                }

            }
            #endregion

            #region 处理来自参数的调整模式请求，不使用错误捕获和重新启动
            foreach (string arg in args)
            {
                if (arg.Contains("debug"))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Form MainForm = null;
                    while (!BeQuit)
                    {
                        MainForm = new MainForm();
                        Application.Run(MainForm);
                        if (MainForm != null) MainForm.Dispose();
                    }
                    Environment.Exit(Environment.ExitCode);
                    break;
                }

            }
            #endregion
            bool createNew;
            //try
            //{
            //Console.WriteLine(Application.ProductName);
            using (System.Threading.Mutex m = new System.Threading.Mutex(true, "Global\\" + Application.ProductName, out createNew))
            {
                if (createNew)
                {
                    IniFile ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "MicroDAQ.ini");

                    DatabaseManager = new DatabaseManager(ini.GetValue("Database", "Address"),
                                                        ini.GetValue("Database", "Port"),
                                                        ini.GetValue("Database", "Database"),
                                                        ini.GetValue("Database", "Username"),
                                                        ini.GetValue("Database", "Password"));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Form MainForm = null;
                    while (!BeQuit)
                        try
                        {
                            MainForm = new MainForm();
                            //frmMain = new TestAlarm();
                            Application.Run(MainForm);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("OH. NO!" + ex.ToString());
                        }
                        finally
                        {
                            if (MainForm != null) MainForm.Dispose();
                        }
                    Environment.Exit(Environment.ExitCode);
                }
                else
                {
                    MessageBox.Show("程序已经在运行，无法再次启动。", "已启动", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            //}
            //catch
            //{
            //    MessageBox.Show("Only one instance of this application is allowed!");
            //}
        }

        public static MachineManager MeterManager = new MachineManager();
        public static DatabaseManager DatabaseManager;// = new DatabaseManager();
        public static DataItemManager M;
        public static FlowAlertManager M_flowAlert;

    }
}
