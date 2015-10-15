﻿using System;
using SQLite.Net;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace Peni.Data
{
	public class ProfileDatabase : CloudDatabase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Peni.Data.ProfileDatabase"/> class.
		/// </summary>
		public ProfileDatabase () {
			base.Init ();
		}

		/// <summary>
		/// Inserts the profile.
		/// </summary>
		/// <returns>The profile.</returns>
		/// <param name="Profile">Profile.</param>
		public async Task InsertProfile(Account Profile) {
			if (client == null) {
				Debug.WriteLine ("AddItem Error: Client was null.");
				return;
			}

			try {
				await accountTable.InsertAsync(Profile); // Inserts into the local database
				await SyncAsync(); // Sends to the mobile service
			} catch (Exception ex) {
				Debug.WriteLine ("AddItem Error: " + ex.ToString());
			}
		}

		/// <summary>
		/// Gets the thread comments.
		/// </summary>
		/// <returns>The thread comments.</returns>
		/// <param name="ThreadID">Thread I.</param>
		public async Task<List<Account>> AttemptLogin(string email, string password) {
			var account = await client.GetTable<Account>().Where(x => x.Email == email && x.Password == password).ToListAsync();
			return account;
		}
	}
}

