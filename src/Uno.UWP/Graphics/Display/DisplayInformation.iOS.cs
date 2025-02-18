﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Windows.Foundation;

namespace Windows.Graphics.Display
{
	public sealed partial class DisplayInformation
	{
		private static readonly Lazy<DisplayInformation> _lazyInstance = new Lazy<DisplayInformation>(() => new DisplayInformation());

		private static DisplayInformation InternalGetForCurrentView() => _lazyInstance.Value;

		private NSObject _didChangeStatusBarOrientationObserver;

		public static UIInterfaceOrientationMask[] PreferredOrientations =
		{
			UIInterfaceOrientationMask.Portrait,
			UIInterfaceOrientationMask.LandscapeRight,
			UIInterfaceOrientationMask.LandscapeLeft,
			UIInterfaceOrientationMask.PortraitUpsideDown
		};

		public DisplayOrientations CurrentOrientation => GetCurrentOrientation();

		/// <summary>
		//// Gets the native orientation of the display monitor, 
		///  which is typically the orientation where the buttons
		///  on the device match the orientation of the monitor.
		/// </summary>
		public DisplayOrientations NativeOrientation => GetNativeOrientation();

		public uint ScreenHeightInRawPixels
		{
			get;
			private set;
		}

		public uint ScreenWidthInRawPixels
		{
			get;
			private set;
		}

		public double RawPixelsPerViewPixel
		{
			get;
			private set;
		}

		/// <summary>
		/// Scale of 1 is considered @1x, which is the equivalent of 96.0 or 100% for UWP.
		/// https://developer.apple.com/documentation/uikit/uiscreen/1617836-scale
		/// </summary>
		public float LogicalDpi
		{
			get;
			private set;
		}

		public ResolutionScale ResolutionScale
		{
			get;
			private set;
		}

		/// <summary>
		/// Sets the NativeOrientation property 
		/// to appropriate value based on user interface idiom
		/// </summary>
		private DisplayOrientations GetNativeOrientation()
		{
			switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
			{
				case UIUserInterfaceIdiom.Phone:
					return DisplayOrientations.Portrait;
				case UIUserInterfaceIdiom.TV:
					return DisplayOrientations.Landscape;
				default:
					//in case of Pad, CarPlay and Unidentified there is no "native" orientation
					return DisplayOrientations.None;
			}
		}

		private DisplayOrientations GetCurrentOrientation()
		{
			var currentOrientationMask = UIApplication.SharedApplication
			   .StatusBarOrientation;

			switch (currentOrientationMask)
			{
				case UIInterfaceOrientation.LandscapeLeft:
					return DisplayOrientations.LandscapeFlipped;
				case UIInterfaceOrientation.LandscapeRight:
					return DisplayOrientations.Landscape;
				case UIInterfaceOrientation.Portrait:
					return DisplayOrientations.Portrait;
				case UIInterfaceOrientation.PortraitUpsideDown:
					return DisplayOrientations.PortraitFlipped;
				default:
					return DisplayOrientations.None;
			}
		}

		private void Update()
		{
			var screen = UIScreen.MainScreen;
			var bounds = screen.Bounds;

			RawPixelsPerViewPixel = screen.NativeScale;
			ScreenHeightInRawPixels = (uint)(bounds.Height * RawPixelsPerViewPixel);
			ScreenWidthInRawPixels = (uint)(bounds.Width * RawPixelsPerViewPixel);

			LogicalDpi = (float)(RawPixelsPerViewPixel * BaseDpi);

			ResolutionScale = (ResolutionScale)(int)(RawPixelsPerViewPixel * 100.0);
		}

		partial void Initialize()
		{
			NSNotificationCenter
				.DefaultCenter
				.AddObserver(
					UIScreen.ModeDidChangeNotification,
					n =>
					{
						Update();
					}
				);
			// we are notified on changes but we're already on a screen so let's initialize
			Update();
		}

		partial void StartOrientationChanged() => ObserveDisplayMetricsChanges();

		partial void StopOrientationChanged() => UnobserveDisplayMetricsChanges();

		partial void StartDpiChanged() => ObserveDisplayMetricsChanges();

		partial void StopDpiChanged() => UnobserveDisplayMetricsChanges();

		private void ObserveDisplayMetricsChanges()
		{
			_lastKnownOrientation = CurrentOrientation;
			_lastKnownDpi = LogicalDpi;
			if (_didChangeStatusBarOrientationObserver == null)
			{
				_didChangeStatusBarOrientationObserver = NSNotificationCenter
					.DefaultCenter
					.AddObserver(
						UIApplication.DidChangeStatusBarOrientationNotification,
						n =>
						{
							OnDisplayMetricsChanged();
						}
					);
			}
		}

		private void UnobserveDisplayMetricsChanges()
		{
			if (_dpiChanged == null && _orientationChanged == null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_didChangeStatusBarOrientationObserver);
				_didChangeStatusBarOrientationObserver = null;
			}
		}

		static partial void SetOrientationPartial(DisplayOrientations orientations)
		{
			var currentOrientationMask = UIApplication.SharedApplication
			   .StatusBarOrientation
			   .ToUIInterfaceOrientationMask();

			var toOrientationMask = orientations.ToUIInterfaceOrientationMask();

			//If we are not already in one of the requested orientations, we need to force the application to rotate.
			if (!toOrientationMask.HasFlag(currentOrientationMask))
			{
				//Rotate to the most preferred orientation that is requested
				//e.g. if our mask is Portrait | PortraitUpsideDown, we prefer to initially rotate to Portrait rather than PortraitUpsideDown
				var toOrientation = PreferredOrientations.FirstOrDefault(ori => toOrientationMask.HasFlag(ori)).ToUIInterfaceOrientation();

				UIDevice.CurrentDevice
					.SetValueForKey(
						new NSNumber((int)toOrientation),
						new NSString("orientation")
					);

				UIApplication.SharedApplication.SetStatusBarOrientation(toOrientation, false);
			}

			//Forces the rotation if the physical device is being held in an orientation that has now become supported
			UIViewController.AttemptRotationToDeviceOrientation();
		}
	}
}
