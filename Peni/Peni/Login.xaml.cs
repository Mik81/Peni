﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Peni
{
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
		}

		protected void ButtonClicked(object sender, EventArgs e)
		{
			// These conditions need to be changed once the database is setup
			if (entryUsername.Text != null) {
				if (entryPassword.Text != null) {
					Navigation.PushModalAsync( new PeniMainContet ());
				} else {
						xLabel.Text = "Please Enter Username and / or Password Again";
				}
			} else {
				xLabel.Text = "Please Enter Username and / or Password Again";
			}
		}
	}
}

