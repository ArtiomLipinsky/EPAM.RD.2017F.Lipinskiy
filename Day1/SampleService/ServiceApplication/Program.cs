using StorageServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try {

            var userStorageService = new StorageService<User>();

            // 1. Add a new user to the storage.

            User user_1 = new User() { FirstName = "Donald", LastName = "Tramp", DateOfBirth = new DateTime(1946, 6, 14) };
            User user_2 = new User() { FirstName = "TestUserFirstName1", LastName = "TestUserLastName1", DateOfBirth = new DateTime(2000, 1, 1) };
            User user_3 = new User() { FirstName = "TestUserFirstName2", LastName = "TestUserLastName2", DateOfBirth = new DateTime(2000, 1, 1) };
            userStorageService.Add(user_1);
            userStorageService.Add(user_2);

            // 2. Remove an user from the storage.

            user_1 = userStorageService.AsEnumerable().First();
            userStorageService.Remove(user_1);

            // 3. Search for an user by the first name.

            IEnumerable<User> searchResult_1 = userStorageService.Search(u => u.FirstName.Contains("TestUserFirstName"));

            // 4. Search for an user by the last name.

            IEnumerable<User> searchResult_2 = userStorageService.Search(u => u.LastName.Contains("TestUserLastName"));

            Console.ReadKey();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
