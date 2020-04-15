using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ICSharpCode.SharpZipLib.Zip;

namespace Zeus
{
	// Token: 0x02000004 RID: 4
	public partial class MainWindow : Window
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000216C File Offset: 0x0000036C
		public MainWindow()
		{
			this.InitializeComponent();
			if (!Directory.Exists(this.folder))
			{
				Directory.CreateDirectory(this.folder);
			}
			if (File.Exists(this.auth_file))
			{
				JsonValue jsonValue = JsonValue.Parse(File.ReadAllText(this.auth_file));
				this.user = (jsonValue as JsonObject);
				this.UsernameInput.Text = this.user["username"];
				this.PasswordInput.Password = this.user["password"];
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000227C File Offset: 0x0000047C
		private async void Authenticate()
		{
			this.user["username"] = this.UsernameInput.Text;
			this.user["password"] = this.PasswordInput.Password;
			Dictionary<string, string> nameValueCollection = new Dictionary<string, string>
			{
				{
					"username",
					this.UsernameInput.Text
				},
				{
					"password",
					this.PasswordInput.Password
				}
			};
			TaskAwaiter<string> taskAwaiter = (await MainWindow.client.PostAsync("https://dashboard.indra.menu/authenticate-injector", new FormUrlEncodedContent(nameValueCollection))).Content.ReadAsStringAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<string> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<string>);
			}
			if (taskAwaiter.GetResult() != "success")
			{
				this.UpdateStatus("Auth failed");
				this.TimeoutReset();
				this.cant_click = false;
			}
			else
			{
				this.UpdateStatus("Logged in");
				File.WriteAllText(this.auth_file, this.user.ToString());
				this.Download();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022B8 File Offset: 0x000004B8
		private void Completed(object sender, AsyncCompletedEventArgs e)
		{
			this.UpdateStatus("Downloaded");
			try
			{
				if (File.Exists(this.zip_file))
				{
					new FastZip().ExtractZip(this.zip_file, this.folder, null);
				}
				this.UpdateStatus("Extracted");
				if (Process.GetProcessesByName("GTA5").Length == 0)
				{
					this.UpdateStatus("GTA not open");
					this.TimeoutReset();
					return;
				}
				BasicInject.Main(Path.Combine(this.folder, "IndraGTA.dll"));
			}
			catch (Exception)
			{
			}
			this.UpdateStatus("Done");
			this.cant_click = false;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002360 File Offset: 0x00000560
		private void Download()
		{
			if (File.Exists(this.zip_file))
			{
				File.Delete(this.zip_file);
			}
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadFileCompleted += this.Completed;
				webClient.DownloadProgressChanged += this.ProgressChanged;
				try
				{
					webClient.DownloadFileAsync(new Uri("http://indra.menu/download/Indra.zip"), this.zip_file);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023F0 File Offset: 0x000005F0
		private void InjectButton_Click(object sender, RoutedEventArgs e)
		{
			if (!this.cant_click)
			{
				this.cant_click = true;
				try
				{
					this.Authenticate();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002428 File Offset: 0x00000628
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			base.Dispatcher.Invoke(delegate()
			{
				this.UpdateStatus("Inject");
				this.cant_click = false;
			});
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002444 File Offset: 0x00000644
		private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.UpdateStatus(e.ProgressPercentage.ToString() + "%");
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002470 File Offset: 0x00000670
		private void TimeoutReset()
		{
			this.timer = new Timer
			{
				Interval = 1000.0
			};
			this.timer.Elapsed += this.OnTimedEvent;
			this.timer.AutoReset = true;
			this.timer.Enabled = true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024C6 File Offset: 0x000006C6
		private void UpdateStatus(string status)
		{
			this.InjectButton.Content = status;
		}

		// Token: 0x0400000A RID: 10
		private static readonly HttpClient client = new HttpClient();

		// Token: 0x0400000B RID: 11
		private JsonObject user = new JsonObject(Array.Empty<KeyValuePair<string, JsonValue>>());

		// Token: 0x0400000C RID: 12
		private string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Indra");

		// Token: 0x0400000D RID: 13
		private string temp_folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");

		// Token: 0x0400000E RID: 14
		private string auth_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Indra", "Login.json");

		// Token: 0x0400000F RID: 15
		private string zip_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", "DownloadedFile.zip");

		// Token: 0x04000010 RID: 16
		public bool cant_click;

		// Token: 0x04000011 RID: 17
		public Timer timer;
	}
}
