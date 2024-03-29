﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Peni.Data;
using Peni.Data.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics;

namespace Peni
{
	public partial class AddWaterPage : ContentPage
	{
		public AddWaterPage()
		{


			InitializeComponent();
			BindingContext = App.Locator.AddWater;

		}
	}

	public class PeniWater : MasterDetailPage
	{
		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			ServiceLocator.Current.GetInstance<ForumPageViewModel> ().OnAppearing ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Peni.PeniForums"/> class.
		/// </summary>
		public PeniWater()
		{
			Detail = new AddWaterPage();
			MenuPage menuPage = new MenuPage();
			Master = menuPage;
			Title = "Add Water";

			// ItemTapped event handler for the side menu
			menuPage.Menu.ItemTapped += (sender, e) => {
				menuPage.Menu.SelectedItem = null;
				this.IsPresented = false;
			};
		}
	}
}
