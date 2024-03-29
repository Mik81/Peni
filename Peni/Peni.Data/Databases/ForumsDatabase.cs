﻿using System;
using SQLite.Net;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Peni.Data
{
	public class ForumsDatabase : CloudDatabase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Peni.Data.ForumsDatabase"/> class.
		/// </summary>
		public ForumsDatabase () {
			base.Init ();
		}

		/// <summary>
		/// Gets all results in the Thread table and returns a list of threads
		/// </summary>
		/// <returns>A List of Threads</returns>
		public async Task<List<Thread>> GetAll() {
			var items = await client.GetTable<Thread>().ToListAsync();
			return items;
		}

		/// <summary>
		/// Gets the threads by user.
		/// </summary>
		/// <returns>The threads by user.</returns>
		/// <param name="username">Username.</param>
		public async Task<List<Thread>> GetThreadsByUser(string author) {
			var items = await client.GetTable<Thread> ().Where (x => x.TopicAuthor == author).ToListAsync ();
			return items;
		}

		/// <summary>
		/// Inserts a thread into the database
		/// </summary>
		/// <returns>true if successful, false otherwise.</returns>
		/// <param name="thread">Thread.</param>
		public async Task InsertThread(Thread thread) {
			if (client == null) {
				Debug.WriteLine ("AddItem Error: Client was null.");
				return;
			}

			try {
				await threadsTable.InsertAsync(thread); // Inserts into the local database
				await SyncAsync(); // Sends to the mobile service
			} catch (Exception ex) {
				Debug.WriteLine ("AddItem Error: " + ex.ToString());
			}
		}

		/// <summary>
		/// Inserts a comment relative to a thread.
		/// </summary>
		/// <returns>true if successful, false otherwise.</returns>
		/// <param name="comment">Comment.</param>
		public async Task InsertComment(UserComment comment) {
			if (client == null) {
				Debug.WriteLine ("AddItem Error: Client was null.");
				return;
			}

			try {
				await commentsTable.InsertAsync(comment); // Inserts into the local database
				await SyncAsync(); // Sends to the mobile service
			} catch (Exception ex) {
				Debug.WriteLine ("AddItem Error: " + ex.ToString());
			}
		}

		/// <summary>
		/// Adds the or update favorite.
		/// </summary>
		/// <returns>The or update favorite.</returns>
		/// <param name="thread">Thread.</param>
		public async Task AddOrUpdateFavorite(ThreadFavorite thread) {
			if (client == null) {
				Debug.WriteLine ("Client was null.");
				return;
			}

			// grab results from favorite table using user id
				// iterate through results looking at thread id
					// if thread id == thread.threadid
						// remove from favorite
						// return

			// otherwise add a new favorite

			try {
				await favoriteTable.InsertAsync(thread);
				await SyncAsync();
			} catch (Exception ex) {
				Debug.WriteLine (ex.Message.ToString ());
			}
		}

		/// <summary>
		/// Gets the user favorites from the database.
		/// </summary>
		/// <returns>A list containing threads that the user favorited.</returns>
		public async Task<List<Thread>> GetUserFavorites() {
			if (client == null) {
				Debug.WriteLine ("Client was null.");
				return null;
			}
				
			// Get results from favorites table which contain the users id.
			var favsResults = await client.GetSyncTable<ThreadFavorite>().Where(x => x.UserID == Globals.UserSession.id).ToListAsync();

			// Get all threads from the database
			var allThreads = await GetAll ();

			// Create a list to store favorited threads
			List<Thread> favs = new List<Thread> ();

			// Loop through each thread
			foreach (Thread thread in allThreads) {
				// Loop through each favorited thread
				foreach (ThreadFavorite fav in favsResults) {
					// Check that the two ID's match and add to our favs list
					if (thread.id == fav.ThreadID) {
						favs.Add (thread);
					}
				}
			}
			return favs;
		}

		/// <summary>
		/// Gets all comments left on a given thread id
		/// </summary>
		/// <returns>A list of user comments relating to the threadid parsed.</returns>
		/// <param name="ThreadID">The ID of the thread to get the comments from</param>
		public async Task<List<UserComment>> GetThreadComments(Guid ThreadID) {
			var comments = await client.GetTable<UserComment>().Where(x => x.ThreadID == ThreadID).ToListAsync();
			return comments;
		}
	}
}

