﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// MUX Reference ScrollViewerIRefreshInfoProviderAdapter.idl, commit c6174f1

using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.UI.Private.Controls;

internal interface IRefreshInfoProviderAdapter
{
	IRefreshInfoProvider AdaptFromTree(UIElement root, Size visualizerSize);

	void SetAnimations(UIElement refreshVisualizerAnimatableContainer);
}
