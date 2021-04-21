using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public partial class NavigationPage : IView
	{
		Thickness IView.Margin => Thickness.Zero;

		internal override void InvalidateMeasureInternal(InvalidationTrigger trigger)
		{
			IsArrangeValid = false;
			base.InvalidateMeasureInternal(trigger);
		}

		public override bool IsMeasureValid
		{
			get
			{
				return base.IsMeasureValid 
					&& Content.IsMeasureValid;
			}

			protected set => base.IsMeasureValid = value;
		}

		public override bool IsArrangeValid
		{
			get
			{
				return base.IsArrangeValid 
					&& Content.IsArrangeValid;
			}

			internal protected set => base.IsArrangeValid = value;
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.Measure(widthConstraint, heightConstraint);
			}

			IsMeasureValid = true;
			return new Size(widthConstraint, heightConstraint);
		}

		protected override Size ArrangeOverride(Rectangle bounds)
		{
			if (IsArrangeValid)
			{
				return bounds.Size;
			}

			IsArrangeValid = true;

			// Update the Bounds (Frame) for this page
			Layout(bounds);

			if (Content is IFrameworkElement element)
			{
				element.Arrange(bounds);
				element.Handler?.SetFrame(element.Frame);
			}

			return Frame.Size;
		}

		protected override void InvalidateMeasureOverride()
		{
			base.InvalidateMeasureOverride();
			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.InvalidateMeasure();
			}
		}

		IFrameworkElement Content =>
			this.CurrentPage;
	}

}
