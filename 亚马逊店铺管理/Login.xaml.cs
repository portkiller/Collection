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
using System.Windows.Shapes;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace 亚马逊店铺管理
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public RestClient _ProjectFlowMangement10WebApi = new RestClient(ConfigurationManager.AppSettings["ProjectFlowMangement10WebApi"]);
        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                return settingString;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            if (SnackbarFive.IsActive == true)
            {
                SnackbarFive.IsActive = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            WindowState = WindowState.Minimized;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.tbuser.Focus();
           

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (tbuser.Text == "")
            {
                SnackbarFive.IsActive=true;
                message.Content = "用户名不能为空";
            }
            else if (tbpasswd.Password == "")
            {
                SnackbarFive.IsActive = true;
                message.Content =" 密码不能为空！";
            }
            else{
                JObject _jobj = null;
                try
                {
                    string url = $"login1/{tbuser.Text}/{tbpasswd.Password}";
                    _jobj = JsonConvert.DeserializeObject<JObject>(_ProjectFlowMangement10WebApi.Get(url));
                    if (_jobj != null && _jobj["status"].ToString() == "1")
                    {
                        Console.WriteLine(string.Format("执行成功"));
                        if (Convert.ToBoolean(ck.IsChecked))
                        {
                            UpdateSettingString("userName", tbuser.Text);
                            UpdateSettingString("password", tbpasswd.Password);
                            UpdateSettingString("isRemember", "true");
                        
                        }
                        else
                        {
                            UpdateSettingString("userName", "");
                            UpdateSettingString("password", "");
                            UpdateSettingString("isRemember", "");
                         
                        }
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Hide();

                    }
                    else
                    {
                        SnackbarFive.IsActive = true;
                        message.Content = string.Format("信息错误，登陆失败！");

                    }

                }
                catch (Exception ex)
                {
                    SnackbarFive.IsActive = true;
                    message.Content = string.Format("执行失败,msg:{0}", ex.Message);
                  
                }
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbuser.Text = GetSettingString("userName");
            tbpasswd.Password = GetSettingString("password");
            if (GetSettingString("isRemember") == "true")
            {
                ck.IsChecked = true;
            }
            else
            {
                ck.IsChecked = false;
            }
        }
    }
}
