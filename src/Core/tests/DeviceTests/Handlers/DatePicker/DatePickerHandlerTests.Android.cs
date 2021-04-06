﻿using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class DatePickerHandlerTests
	{
		[Fact(DisplayName = "Minimum Date Initializes Correctly")]
		public async Task MinimumDateInitializesCorrectly()
		{
			DateTime xplatMinimumDate = new DateTime(2000, 01, 01);

			var datePicker = new DatePickerStub()
			{
				MinimumDate = xplatMinimumDate,
				Date = DateTime.Today
			};

			double expectedValue = (long)xplatMinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;

			var values = await GetValueAsync(datePicker, (handler) =>
			{
				return new
				{
					ViewValue = datePicker.MinimumDate,
					NativeViewValue = GetNativeMinimumDate(handler)
				};
			});

			Assert.Equal(xplatMinimumDate, values.ViewValue);
			Assert.Equal(expectedValue, values.NativeViewValue);
		}

		[Fact(DisplayName = "Maximum Date Initializes Correctly")]
		public async Task MaximumDateInitializesCorrectly()
		{
			DateTime xplatMaximumDate = new DateTime(2030, 01, 01);

			var datePicker = new DatePickerStub()
			{
				MinimumDate = new DateTime(2000, 01, 01),
				MaximumDate = new DateTime(2030, 01, 01),
				Date = DateTime.Today
			};

			double expectedValue = (long)xplatMaximumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;

			var values = await GetValueAsync(datePicker, (handler) =>
			{
				return new
				{
					ViewValue = datePicker.MaximumDate,
					NativeViewValue = GetNativeMaximumDate(handler)
				};
			});

			Assert.Equal(xplatMaximumDate, values.ViewValue);
			Assert.Equal(expectedValue, values.NativeViewValue);
		}

		[Fact(DisplayName = "CharacterSpacing Initializes Correctly")]
		public async Task CharacterSpacingInitializesCorrectly()
		{
			var xplatCharacterSpacing = 4;

			var datePicker = new DatePickerStub()
			{
				CharacterSpacing = xplatCharacterSpacing,
				Date = DateTime.Today
			};

			float expectedValue = datePicker.CharacterSpacing.ToEm();

			var values = await GetValueAsync(datePicker, (handler) =>
			{
				return new
				{
					ViewValue = datePicker.CharacterSpacing,
					NativeViewValue = GetNativeCharacterSpacing(handler)
				};
			});

			Assert.Equal(xplatCharacterSpacing, values.ViewValue);
			Assert.Equal(expectedValue, values.NativeViewValue, EmCoefficientPrecision);
		}

		MauiDatePicker GetNativeDatePicker(DatePickerHandler datePickerHandler) =>
			(MauiDatePicker)datePickerHandler.View;

		DateTime GetNativeDate(DatePickerHandler datePickerHandler)
		{
			var dateString = GetNativeDatePicker(datePickerHandler).Text;
			DateTime.TryParse(dateString, out DateTime result);

			return result;
		}

		long GetNativeMinimumDate(DatePickerHandler datePickerHandler)
		{
			var dialog = datePickerHandler.DatePickerDialog;
			var minDate = dialog.DatePicker.MinDate;

			return minDate;
		}

		long GetNativeMaximumDate(DatePickerHandler datePickerHandler)
		{
			var dialog = datePickerHandler.DatePickerDialog;
			var maxDate = dialog.DatePicker.MaxDate;

			return maxDate;
		}

		double GetNativeCharacterSpacing(DatePickerHandler datePickerHandler)
		{
			var mauiDatePicker = GetNativeDatePicker(datePickerHandler);

			if (mauiDatePicker != null)
			{
				return mauiDatePicker.LetterSpacing;
			}

			return -1;
		}
	}
}