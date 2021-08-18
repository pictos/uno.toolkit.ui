﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Uno.Toolkit.Samples.Entities;
using Uno.UI.ToolkitLib;

#if IS_WINUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
#endif

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uno.Toolkit.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, nameof(DrawerControl))]
	public sealed partial class DrawerControlSamplePage : Page
	{
		public DrawerControlSamplePage()
		{
			this.InitializeComponent();
			this.Loaded += (s, e) => SetupOptions();
		}

		private void SetupOptions()
		{
			var sampleDrawerControl = SamplePageLayout.GetSampleChild<DrawerControl>("SampleDrawerControl");
			var optionOpenDirection = SamplePageLayout.GetSampleChild<ComboBox>("OptionOpenDirection");
			var optionLightDismissOverlayBackground = SamplePageLayout.GetSampleChild<ComboBox>("OptionLightDismissOverlayBackground");
			var optionEdgeSwipeDetectionLengthIsNull = SamplePageLayout.GetSampleChild<CheckBox>("OptionEdgeSwipeDetectionLengthIsNull");
			var optionEdgeSwipeDetectionLengthValue = SamplePageLayout.GetSampleChild<Slider>("OptionEdgeSwipeDetectionLengthValue");

			optionOpenDirection.ItemsSource = typeof(DrawerOpenDirection).GetEnumValues();
			optionOpenDirection.SelectedIndex = 0;
			optionOpenDirection.SelectionChanged += (s, e) =>
			{
				sampleDrawerControl.OpenDirection = (DrawerOpenDirection)optionOpenDirection.SelectedValue;
			};

			optionLightDismissOverlayBackground.ItemsSource = "SystemControlBackgroundChromeMediumLowBrush,Pink,SkyBlue".Split(',');
			optionLightDismissOverlayBackground.SelectedIndex = 0;
			optionLightDismissOverlayBackground.SelectionChanged += (s, e) =>
			{
				sampleDrawerControl.LightDismissOverlayBackground = (string)optionLightDismissOverlayBackground.SelectedValue switch
				{
					"SystemControlBackgroundChromeMediumLowBrush" => (Brush)Resources["SystemControlBackgroundChromeMediumLowBrush"],
					"SkyBlue" => new SolidColorBrush(Colors.SkyBlue),
					"Pink" => new SolidColorBrush(Colors.Pink),

					_ => throw new ArgumentOutOfRangeException(),
				};
			};

			optionEdgeSwipeDetectionLengthIsNull.Checked += (s, e) => UpdateEdgeSwipeDetectionLength();
			optionEdgeSwipeDetectionLengthValue.ValueChanged += (s, e) => UpdateEdgeSwipeDetectionLength();
			void UpdateEdgeSwipeDetectionLength() =>
				sampleDrawerControl.EdgeSwipeDetectionLength =
					optionEdgeSwipeDetectionLengthIsNull.IsChecked == true
						? default(double?)
						: optionEdgeSwipeDetectionLengthValue.Value;
		}
	}
}