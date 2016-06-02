/**
* Copyright 2016 IBM Corp.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worklight;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ResourceRequestWin10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainPage _this;

        public MainPage()
        {
            this.InitializeComponent();
            _this = this;
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IWorklightClient _newClient = WorklightClient.CreateInstance();

                StringBuilder uriBuilder = new StringBuilder().Append("/adapters").Append("/JavaAdapter").Append("/users")
                                            .Append("/").Append(this.firstname.Text)
                                            .Append("/").Append(this.middlename.Text)
                                            .Append("/").Append(this.lastname.Text);
                ;

                Debug.WriteLine(new Uri(uriBuilder.ToString(), UriKind.Relative));

                WorklightResourceRequest rr = _newClient.ResourceRequest(new Uri(uriBuilder.ToString(), UriKind.Relative), "POST", "");

                rr.SetQueryParameter("age", this.age.Text);

                KeyValuePair<String, String> headers = new KeyValuePair<string, string>("birthdate", this.date.Text);

                rr.SetHeader(headers);

                Dictionary<string, string> formParams = new Dictionary<string, string>();
                formParams.Add("height", this.height.Text);

                WorklightResponse resp = await rr.Send(formParams);

                Debug.WriteLine(">>>>> After rr.send()");

                System.Diagnostics.Debug.WriteLine(resp.ResponseText);

                System.Diagnostics.Debug.WriteLine(resp.ResponseJSON);

                this.Console.Text = resp.ResponseText;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }

        }

        private void ClearConsole(object sender, DoubleTappedRoutedEventArgs e)
        {
            Console.Text = "";
        }

        private void ShowConsole(object sender, TappedRoutedEventArgs e)
        {
            this.ConsolePanel.Visibility = Visibility.Visible;
            this.ConsoleTab.Foreground = new SolidColorBrush(Colors.DodgerBlue);
        }
    }
}
