﻿using Uno.Extensions;
using Uno.Foundation.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel;

namespace Windows.UI.Xaml.Media
{
	public partial class ImageSource
	{
		public ImageSource()
		{
			InitializeBinder();
		}

		internal void UnloadImageData() { }
	}
}
