﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using static Private.Infrastructure.TestServices;
using System.Linq;
#if HAS_UNO
using Uno.UI.WinRT.Extensions.UI.Popups;
#endif

namespace Uno.UI.RuntimeTests.Tests.Windows_UI_Popups
{
	[TestClass]
#if __MACOS__
	[Ignore("Currently fails on macOS, part of #9282 epic")]
#endif
	public class Given_MessageDialog
	{
		[TestMethod]
		[RunsOnUIThread]
		public async Task Should_Close_Open_Flyouts()
		{
			var button = new Windows.UI.Xaml.Controls.Button();
			var flyout = new Flyout();
			FlyoutBase.SetAttachedFlyout(button, flyout);
			WindowHelper.WindowContent = button;
			Assert.AreEqual(0, GetNonMessageDialogPopupsCount());
			FlyoutBase.ShowAttachedFlyout(button);
			Assert.AreEqual(1, GetNonMessageDialogPopupsCount());
			var messageDialog = new MessageDialog("Should_Close_Open_Popups");
			var asyncOperation = messageDialog.ShowAsync();
			Assert.AreEqual(0, GetNonMessageDialogPopupsCount());
			asyncOperation.Cancel();
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task Should_Not_Close_Open_ContentDialogs()
		{
			Assert.AreEqual(0, GetNonMessageDialogPopupsCount());

			var contentDialog = new ContentDialog
			{
				Title = "Title",
				Content = "My Dialog Content"
			};

			contentDialog.ShowAsync();

			Assert.AreEqual(1, GetNonMessageDialogPopupsCount());

			var messageDialog = new MessageDialog("Hello");
			var asyncOperation = messageDialog.ShowAsync();
			Assert.AreEqual(1, GetNonMessageDialogPopupsCount());
			contentDialog.Hide();
			asyncOperation.Cancel();
		}

		[TestMethod]
		[RunsOnUIThread]
		public async Task When_Cancel_Then_CloseDialog()
		{
			var messageDialog = new MessageDialog("When_Cancel_Then_CloseDialog");
			var asyncOperation = messageDialog.ShowAsync();

			Assert.AreEqual(AsyncStatus.Started, asyncOperation.Status);

			await WindowHelper.WaitForIdle();

#if __IOS__ //in iOS we want to force calling in a different thread than UI
			await Task.Run(() => asyncOperation.Cancel());
#else
			asyncOperation.Cancel();
#endif

			await WindowHelper.WaitForIdle();

			Assert.AreEqual(AsyncStatus.Canceled, asyncOperation.Status);
		}

		private int GetNonMessageDialogPopupsCount()
		{
			var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
#if HAS_UNO
			return popups.Count(p => p.Child is not MessageDialogContentDialog);
#else
			return popups.Count();
#endif
		}
	}
}
