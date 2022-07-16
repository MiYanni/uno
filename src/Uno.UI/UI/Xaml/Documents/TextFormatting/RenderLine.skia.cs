﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windows.UI.Xaml.Documents.TextFormatting
{
	/// <summary>
	/// Represents the set of segment spans that make up a rendered text line.
	/// </summary>
	internal class RenderLine
	{
		private readonly List<RenderSegmentSpan> _segmentSpans;

		public IReadOnlyList<RenderSegmentSpan> SegmentSpans => _segmentSpans;

		public float Width { get; }

		public float WidthWithoutTrailingSpaces { get; }

		public float Height { get; }

		public float BaselineOffsetY { get; }

		public bool Wraps { get; }

		public RenderLine(IEnumerable<RenderSegmentSpan> spans, LineStackingStrategy lineStackingStrategy, float lineHeight, bool firstLine, bool wraps)
		{
			_segmentSpans = new(spans);

			Width = 0;

			int lastIndex = _segmentSpans.Count - 1;

			for (int i = 0; i < lastIndex; i++)
			{
				Width += _segmentSpans[i].Width;
			}

			WidthWithoutTrailingSpaces = Width;

			WidthWithoutTrailingSpaces += _segmentSpans[lastIndex].WidthWithoutTrailingSpaces;
			Width += _segmentSpans[lastIndex].Width;

			switch (lineStackingStrategy)
			{
				case LineStackingStrategy.MaxHeight:
					float maxStackHeight = GetMaxStackHeight();

					if (lineHeight == 0)
					{
						Height = maxStackHeight;
						BaselineOffsetY = -GetMaxBelowBaselineHeight();
					}
					else
					{
						if (lineHeight < maxStackHeight)
						{
							Height = maxStackHeight;
							BaselineOffsetY = -GetMaxBelowBaselineHeight();

						}
						else
						{
							Height = lineHeight;
							BaselineOffsetY = GetBaselineOffsetY(lineHeight, maxStackHeight);
						}
					}

					break;

				case LineStackingStrategy.BlockLineHeight:
					Height = lineHeight;
					BaselineOffsetY = GetBaselineOffsetY(lineHeight, GetMaxStackHeight());
					break;

				default: // LineStackingStrategy.BaselineToBaseline:
					if (firstLine)
					{
						Height = GetMaxAboveBaselineHeight();
						BaselineOffsetY = 0;
					}
					else
					{
						Height = lineHeight;
						BaselineOffsetY = GetBaselineOffsetY(lineHeight, GetMaxStackHeight());
					}

					break;
			}

			Wraps = wraps;
		}

		internal (float LineOffset, float JustifySpaceOffset) GetOffsets(float availableWidth, TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Left:
					return (GetLeftCharacterSpacingOffset(), 0);

				case TextAlignment.Right:
					return (GetRightCharacterSpacingOffset() + availableWidth - Width, 0);

				case TextAlignment.Center:
					float charSpacingOffset = (GetLeftCharacterSpacingOffset() + GetRightCharacterSpacingOffset());
					return ((charSpacingOffset + availableWidth - WidthWithoutTrailingSpaces) / 2, 0);

				default: // TextAlignment.Justify
					float justifySpaceOffset = 0;

					if (Wraps && availableWidth > WidthWithoutTrailingSpaces)
					{
						justifySpaceOffset = (availableWidth - WidthWithoutTrailingSpaces) / CountJustifySpaces();
					}

					return (GetLeftCharacterSpacingOffset(), justifySpaceOffset);
			}

			int CountJustifySpaces()
			{
				int justifySpaceCount = 0;

				for (int i = 0; i < _segmentSpans.Count - 1; i++)
				{
					justifySpaceCount += _segmentSpans[i].TrailingSpaces;
				}

				return justifySpaceCount;
			}

			// These methods compensate for trailing or leading character spacing on lines so the text aligns with the edge of the layout:

			float GetLeftCharacterSpacingOffset()
			{
				for (int i = 0; i < _segmentSpans.Count; i++)
				{
					var span = _segmentSpans[i];

					if (span.Width > 0)
					{
						return span.Segment.Direction == FlowDirection.RightToLeft ? -span.CharacterSpacing : 0;
					}
				}

				return 0;
			}

			float GetRightCharacterSpacingOffset()
			{
				for (int i = _segmentSpans.Count - 1; i >= 0; i--)
				{
					var span = _segmentSpans[i];

					if (span.Width > 0)
					{
						return span.Segment.Direction == FlowDirection.LeftToRight ? span.CharacterSpacing : 0;
					}
				}

				return 0;
			}
		}

		private float GetMaxStackHeight() => _segmentSpans.Max(s => s.Segment.Inline.LineHeight);

		private float GetMaxAboveBaselineHeight() => _segmentSpans.Max(s => s.Segment.Inline.AboveBaselineHeight);

		private float GetMaxBelowBaselineHeight() => _segmentSpans.Max(s => s.Segment.Inline.BelowBaselineHeight);

		/// <summary>
		/// Gets the offset of the baseline for this render line based on a custom line height. Scales the default baseline offset
		/// by the ratio of the default line height to the custom line height.
		/// </summary>
		private float GetBaselineOffsetY(float lineHeight, float maxStackHeight)
		{
			if (maxStackHeight == 0)
			{
				return 0;
			}

			var defaultBaselineOffsetY = -(float)GetMaxBelowBaselineHeight();
			return defaultBaselineOffsetY * lineHeight / maxStackHeight;
		}
	}
}
