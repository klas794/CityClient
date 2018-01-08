using System;
using System.Collections.Generic;
using System.Linq;
using CityClient.Models;
using SQLite;
using System.Threading.Tasks;

namespace CityClient
{
    public class FavouritesRepository
    {
        private readonly SQLiteAsyncConnection conn;

        public string StatusMessage { get; set; }

        public FavouritesRepository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Favourite>().Wait();
        }

        public async Task AddNewFavouriteAsync(string name)
        {
            try
            {
                //basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                //insert a new Favourites into the Favourites table
                var result = await conn.InsertAsync(new Favourite { Name = name }).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }

        public async Task RemoveFavouriteAsync(string name)
        {
            try
            {
                //basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                //delete Favourite from the Favourites table
                var all = await GetAllFavouritesAsync();
                var fav = all.SingleOrDefault(x => x.Name == name);

                if(fav != null) { 
                    var result = await conn.DeleteAsync(fav).ConfigureAwait(continueOnCapturedContext: false);
                    StatusMessage = string.Format("{0} record(s) removed [Name: {1})", result, name);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }

        public Task<List<Favourite>> GetAllFavouritesAsync()
        {
            //return a list of people saved to the Favourites table in the database
            return conn.Table<Favourite>().ToListAsync();
        }
    }
}