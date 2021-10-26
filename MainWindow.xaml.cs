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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private Process pTemp;
        public MainWindow()
        {
            InitializeComponent();

            //创建一个进程
            pTemp = new Process();
            pTemp.StartInfo.FileName = "cmd.exe";
            pTemp.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
            pTemp.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            pTemp.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            pTemp.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            pTemp.StartInfo.CreateNoWindow = true;//不显示程序窗口
            pTemp.Start();//启动程序

            // 从 txt 初始 按钮 
            // InitTxtFuncs();

            // 从 json 初始按钮
            InitJsonFuncs();

            // 加一个打开本地目录的函数
            Action action = () =>
            {
                System.Diagnostics.Process.Start("explorer.exe", GetProjectRootPath());
            };
            AddNewBtn("打开目录", action);
        }

        private void InitTxtFuncs()
        {

            // 从 txt 获取 
            FileStream fs = new FileStream(@"file.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader m_streamReader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            //StreamReader m_streamReader = new StreamReader(fs);
            //使用StreamReader类来读取文件
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //从数据流中读取每一行，直到文件的最后一行

            string strLineName = m_streamReader.ReadLine();
            string strLinePath = "";
            while (strLineName != null)
            {
                strLinePath = m_streamReader.ReadLine();
                AddNewBtn(strLineName, strLinePath);
                strLineName = m_streamReader.ReadLine();

            }
            //关闭此StreamReader对象
            m_streamReader.Close();
        }
        private void InitJsonFuncs()
        {

            FileStream fs = new FileStream(@"file.json", FileMode.OpenOrCreate, FileAccess.Read);
            var sr = new StreamReader(fs);
            var text = sr.ReadToEnd();
            try
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(text);
                for (int i = 0; i < ja.Count; i++)
                {
                    AddNewBtn(ja[i]["Name"].ToString(), ja[i]["CMD"].ToString());
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void AddNewBtn(string strA, string strB)
        {
            Button btn = new Button();
            btn.Content = strA;
            btn.Click += (o, e) =>
            {
                //do someting...}
                func_cmd_call(strB);
            };
            BtnPar.Items.Add(btn);
            // BtnPar.Children.Add(btn);
        }
        private void AddNewBtn(string strA, Action action)
        {
            Button btn = new Button();
            btn.Content = strA;
            btn.Click += (o, e) =>
            {
                //do someting...}
                action();
            };
            BtnPar.Items.Add(btn);
            // BtnPar.Children.Add(btn);
        }

        private void func_cmd_call(string strCmd)
        {
            try
            {
                //pTemp.Start();
                string strCMD = strCmd;
                //向cmd窗口发送输入信息
                pTemp.StandardInput.WriteLine(strCMD);
                //pTemp.StandardInput.AutoFlush = true;
                //获取cmd窗口的输出信息
                //string output = pTemp.StandardOutput.ReadToEnd();
                //等待程序执行完退出进程
                // p.WaitForExit();
                //pTemp.Close();
                // MessageBox.Show(output);
                // Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n跟踪;" + ex.StackTrace);
            }
        }


        private void BtnPar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public static string GetProjectRootPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            return rootpath;
        }
    }
}

