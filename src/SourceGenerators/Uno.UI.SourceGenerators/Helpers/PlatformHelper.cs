﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Uno.Roslyn;

#if NETFRAMEWORK
using Uno.SourceGeneration;
using Uno.UI;
using Uno.UI.SourceGenerators;
using Uno.UI.SourceGenerators.Helpers;
using Uno.UI.SourceGenerators.XamlGenerator;
#endif

namespace Uno.UI.SourceGenerators.Helpers
{
	public class PlatformHelper
	{
		public static bool IsValidPlatform(GeneratorExecutionContext context)
		{
			var evaluatedValue = context.GetMSBuildPropertyValue("TargetPlatformIdentifier");
			var windowsAppSDKWinUI = context.GetMSBuildPropertyValue("WindowsAppSDKWinUI");
			var projectTypeGuids = context.GetMSBuildPropertyValue("ProjectTypeGuidsProperty");

			var isUAP = evaluatedValue?.Equals("UAP", StringComparison.OrdinalIgnoreCase) ?? false;

			// Those two checks are now required since VS 16.9 which enables source generators by default
			// and the uno targets files are not present for uap targets.
			var isWindowsRuntimeApplicationOutput = context.Compilation.Options.OutputKind == OutputKind.WindowsRuntimeApplication;
			var isWindowsRuntimeMetadataOutput = context.Compilation.Options.OutputKind == OutputKind.WindowsRuntimeMetadata;

			var isWinAppSDK = windowsAppSDKWinUI?.Equals("True", StringComparison.OrdinalIgnoreCase) ?? false;
			var isNetCoreDesktop = projectTypeGuids?.Equals("{60dc8134-eba5-43b8-bcc9-bb4bc16c2548};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}", StringComparison.OrdinalIgnoreCase) ?? false;

			return !isUAP
				&& !isWinAppSDK
				&& !isNetCoreDesktop
				&& !isWindowsRuntimeMetadataOutput
				&& !isWindowsRuntimeApplicationOutput;
		}

		public static bool IsAndroid(GeneratorExecutionContext context)
			=> context.GetMSBuildPropertyValue("AndroidApplication")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

		public static bool IsiOS(GeneratorExecutionContext context)
			=> context.GetMSBuildPropertyValue("ProjectTypeGuidsProperty")?.Equals("{FEACFBD2-3405-455C-9665-78FE426C6842},{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}", StringComparison.OrdinalIgnoreCase) ?? false;

		public static bool IsMacOs(GeneratorExecutionContext context)
			=> context.GetMSBuildPropertyValue("ProjectTypeGuidsProperty")?.Equals("{A3F8F2AB-B479-4A4A-A458-A89E7DC349F1},{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}", StringComparison.OrdinalIgnoreCase) ?? false;

		public static bool IsExe(GeneratorExecutionContext context)
			=> context.GetMSBuildPropertyValue("OutputType")?.Equals("Exe", StringComparison.OrdinalIgnoreCase) ?? false;

		public static bool IsUnoHead(GeneratorExecutionContext context)
			=> context.GetMSBuildPropertyValue("IsUnoHead")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

		public static bool IsApplication(GeneratorExecutionContext context)
			=> IsAndroid(context)
				|| (IsiOS(context) && IsExe(context))
				|| (IsMacOs(context) && IsExe(context))
				|| IsUnoHead(context);
	}
}
