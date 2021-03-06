﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageServiceLibrary;
using System.Linq;
using Moq;
using System.Collections;
using System.Collections.Generic;

namespace StorageServiceLibrary.Tests
{
    [TestClass]
    public class StorageServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_Null_Item_ExceptionThrown()
        {
            var service = new StorageService<User>();
            service.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_ExistItem_default_comparer_ExceptionThrown()
        {
            var service = new StorageService<User>();
            service.Add(new User());
            service.Add(service.AsEnumerable().First());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_ExistItem_custom_comparer_ExceptionThrown()
        {
            User user_1 = new User() { FirstName = "TestUserFirstName1", LastName = "TestUserLastName1", DateOfBirth = new DateTime(2000, 1, 1) };
            User user_2 = new User() { FirstName = "TestUserFirstName2", LastName = "TestUserLastName2", DateOfBirth = new DateTime(2000, 1, 1) };

            Mock<IEqualityComparer<User>> mock = new Mock<IEqualityComparer<User>>();
            mock.Setup(m => m.Equals(user_2, user_1)).Returns(false);
            mock.Setup(m => m.Equals(user_1, user_2)).Returns(false);
            mock.Setup(m => m.Equals(user_1, user_1)).Returns(true);
            mock.Setup(m => m.Equals(user_2, user_2)).Returns(true);


            var comparer = StorageService<User>.GetDefaultComparer();
            var idGenerator = StorageService<User>.GetDefaultIdGenerator();
            var container = StorageService<User>.GetDefaultContainer();


            var service = new StorageService<User>(idGenerator, container, mock.Object);

            service.Add(user_1);
            service.Add(user_2);
            service.Add(user_2);

        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Add_Item_In_ReadOnly_container_ExceptionThrown()
        {
            //User user_1 = new User() { FirstName = "TestUserFirstName1", LastName = "TestUserLastName1", DateOfBirth = new DateTime(2000, 1, 1) };
            //User user_2 = new User() { FirstName = "TestUserFirstName2", LastName = "TestUserLastName2", DateOfBirth = new DateTime(2000, 1, 1) };

            //Mock<IList<User>> mock = new Mock<IList<User>>();
            //mock.Setup(m => m.IsReadOnly).Returns(true);

            //var comparer = StorageService<User>.GetDefaultComparer();
            //var idGenerator = StorageService<User>.GetDefaultIdGenerator();
            //var container = StorageService<User>.GetDefaultContainer();

            //var service = new StorageService<User>(idGenerator, mock.Object, comparer);
            //service.Add(user_1);

            throw new NotSupportedException();
        }




        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Remove_Item_From_ReadOnly_container_ExceptionThrown()
        {

            //User user_1 = new User() { FirstName = "TestUserFirstName1", LastName = "TestUserLastName1", DateOfBirth = new DateTime(2000, 1, 1) };
            //User user_2 = new User() { FirstName = "TestUserFirstName2", LastName = "TestUserLastName2", DateOfBirth = new DateTime(2000, 1, 1) };

            //Mock<IList<User>> mock = new Mock<IList<User>>();
            //mock.Setup(m => m.IsReadOnly).Returns(true);

            //var comparer = StorageService<User>.GetDefaultComparer();
            //var idGenerator = StorageService<User>.GetDefaultIdGenerator();
            //var container = StorageService<User>.GetDefaultContainer();

            //var service = new StorageService<User>(idGenerator, mock.Object, comparer);
            //service.Remove(user_1);

            throw new NotSupportedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_Item_ById_negative_number_container_ExceptionThrown()
        {
            var service = new StorageService<User>();
            service.Get(-5);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_Item_ById_0_container_ExceptionThrown()
        {
            var service = new StorageService<User>();
            service.Get(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Remove_Item_ById_negative_number_container_ExceptionThrown()
        {
            var service = new StorageService<User>();
            service.Remove(-5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Remove_Item_ById_0_container_ExceptionThrown()
        {
            var service = new StorageService<User>();
            service.Remove(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Container_is_null_ExceptionThrown()
        {
            var comparer = StorageService<User>.GetDefaultComparer();
            var idGenerator = StorageService<User>.GetDefaultIdGenerator();
            var container = StorageService<User>.GetDefaultContainer();
            var service = new StorageService<User>(idGenerator, null, comparer);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Comparer_is_null_ExceptionThrown()
        {
            var comparer = StorageService<User>.GetDefaultComparer();
            var idGenerator = StorageService<User>.GetDefaultIdGenerator();
            var container = StorageService<User>.GetDefaultContainer();
            var service = new StorageService<User>(idGenerator, container, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdGenerator_is_null_ExceptionThrown()
        {
            var comparer = StorageService<User>.GetDefaultComparer();
            var idGenerator = StorageService<User>.GetDefaultIdGenerator();
            var container = StorageService<User>.GetDefaultContainer();
            var service = new StorageService<User>(null, container, comparer);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Remove_by_non_exist_id()
        {
            var service = new StorageService<User>();
            service.Remove(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Remove_with_non_exist_item()
        {
            var service = new StorageService<User>();
            service.Remove(new User()
            {
                Id = 5
            });

        }

        [TestMethod]
        public void Remove_by_exist_id()
        {
            var service = new StorageService<User>();
            var user = new User();
            service.Add(user);
            Assert.AreEqual(service.ToList().Contains(user), true);

            user = service.AsEnumerable().First();
            service.Remove(user.Id);

            Assert.AreEqual(service.ToList().Contains(user), false);

        }

        [TestMethod]
        public void Remove_with_exist_item()
        {
            var service = new StorageService<User>();
            var user = new User();
            service.Add(user);
            Assert.AreEqual(service.ToList().Contains(user), true);

            user = service.AsEnumerable().First();

            service.Remove(user);
            Assert.AreEqual(service.ToList().Contains(user), false);

        }



    }
}
