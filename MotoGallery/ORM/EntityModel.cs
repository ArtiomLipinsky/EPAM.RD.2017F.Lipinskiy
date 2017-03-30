namespace ORM
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.IO;
    using ORM.Entity;
    using System.Collections.Generic;

    public class EntityModel : DbContext
    {
        // Your context has been configured to use a 'EntityModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ORM.EntityModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'EntityModel' 
        // connection string in the application configuration file.

        static EntityModel()
        {
            if (!Database.Exists("name=GalleryModel"))
                Database.SetInitializer(new GalleryDbInitializer());
        }

        public EntityModel()
            : base("name=GalleryModel")
        {

            //this.Configuration.ProxyCreationEnabled = false;
        }



        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Image> Images { get; set; }


        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type {0}| in state {1}| has thefollowing validation errors: ", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException e)
            {
                //Add your code to inspect the inner exception and/or
                //e.Entries here.
                //Or just use the debugger.
                //Added this catch (after the comments below) to make it more obvious 
                //how this code might help this specific problem

                Debug.WriteLine(e.Message);
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            //modelBuilder.Entity<Image>()
            //    .HasRequired(i => i.Album)
            //    .WithMany(i => i.Images)
            //    .HasForeignKey(i => i.AlbumId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Image>()
            //    .HasRequired(i => i.Extension)
            //    .WithMany(e => e.Images)
            //    .HasForeignKey(i => i.ExtensionId)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasRequired(u => u.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Image>()
                .HasRequired(i => i.Album)
                .WithMany(a => a.Images)
                .HasForeignKey(i => i.AlbumId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Album>()
                .HasMany(a => a.Images)
                .WithRequired(i => i.Album)
                .HasForeignKey(a => a.AlbumId)
                .WillCascadeOnDelete(false);

            //base.OnModelCreating(modelBuilder);
        }

        //new public void Dispose()
        //{
        //    base.Dispose();
        //}

    }


    public class GalleryDbInitializer : DropCreateDatabaseAlways<EntityModel>
    {


        protected override void Seed(EntityModel db)
        {

            Role[] roles = {
            new Role() { Id = 1, Name = "administrator" },
            new Role() { Id = 2, Name = "moderator" },
            new Role() { Id = 3, Name = "user" }
        };


            db.Roles.AddRange(roles);


            User[] users = {
                new User
            {
                Id = 1,
                Name = "Admin",
                Password = "12345678",
                RoleId = 1,
                Role = roles[0]
            },

                new User
            {
                Id = 2,
                Name = "Moder",
                Password = "12345678",
                RoleId = 2,
                Role = roles[1]
            },


                new User
            {
                Id = 4,
                Name = "User",
                Password = "12345678",
                RoleId = 3,
                Role = roles[2]
            }};


            db.Users.AddRange(users);


            var album1 = new Album()
            {
                Id = 1,
                Name = "Clasic",
                User = users[0],
                UserId = 1,
                ImagesId = new List<int>() { 1, 2, 3, 4 },
                Images = new List<Image>()

            };

            var album2 = new Album()
            {
                Id = 2,
                Name = "Sport",
                User = users[0],
                UserId = 1,
                ImagesId = new List<int>() { 5, 6, 7 },
                Images = new List<Image>()

            };

            var album3 = new Album()
            {
                Id = 3,
                Name = "Cruiser",
                User = users[0],
                UserId = 1,
                ImagesId = new List<int>() { 8, 9, 10, 11, 12, 13, 14 },
                Images = new List<Image>()

            };



            Image[] img = {
             new Image() { Id = 1, AlbumId=1, Album=album1, Name = "Custom Cafe Racer",Desc= "Rat 250cc", UserId = 1, CreationDate = new DateTime(2000, 5, 10),  Path = Path.Combine("Content", "img", "1.jpg") },
             new Image() { Id = 2, AlbumId=1, Album=album1, Name = "Morini Scrambler 1200", Desc = "Enduro 400cc L",  UserId = 1, CreationDate = new DateTime(2001, 5, 10),  Path = Path.Combine("Content", "img", "2.jpg") },
             new Image() { Id = 3, AlbumId=1, Album=album1,  Name = "Bobber V9", Desc = "Clasic Black V 1200cc", UserId = 1, CreationDate = new DateTime(2002, 5, 10),  Path = Path.Combine("Content", "img", "3.jpg") },
             new Image() { Id = 4, AlbumId=1, Album=album1,  Name = "MotoGuzzi", Desc = "Yellow V 1200cc",  UserId = 1, CreationDate = new DateTime(2003, 5, 10),  Path = Path.Combine("Content", "img", "4.jpg") },
             new Image() { Id = 5, AlbumId=2, Album=album2,  Name = "Kawasaki zx6r", Desc = "Sport 600cc",  UserId = 2, CreationDate = new DateTime(2004, 5, 10),  Path = Path.Combine("Content", "img", "5.jpg") },
             new Image() { Id = 6, AlbumId=2, Album=album2, Name = "Yamaha R6", Desc = "Sport 600cc",  UserId = 2, CreationDate = new DateTime(2005, 5, 10),  Path = Path.Combine("Content", "img", "6.jpg") },
             new Image() { Id = 7, AlbumId=2, Album=album2, Name = "Ducati 1099", Desc = "Sport 1000cc",  UserId = 2, CreationDate = new DateTime(2006, 5, 10),  Path = Path.Combine("Content", "img", "7.jpg") },
             new Image() { Id = 8, AlbumId=3, Album=album3, Name = "Harley V-Rod", Desc = "Power Cruiser Gray",  UserId = 3, CreationDate = new DateTime(2017, 1, 1),  Path = Path.Combine("Content", "img", "8.jpg") },
             new Image() { Id = 9, AlbumId=3, Album=album3, Name = "Yamaha V-Max", Desc = "Power Cruiser 1800cc 2015", UserId = 3, CreationDate = new DateTime(2010, 7, 3),  Path = Path.Combine("Content", "img", "9.jpg") },
             new Image() { Id = 10, AlbumId=3, Album=album3, Name = "Some Cruiser", Desc = "Some description",  UserId = 3, CreationDate = new DateTime(1991, 3, 2),  Path = Path.Combine("Content", "img", "10.jpg") },
               new Image() { Id = 11, AlbumId=3, Album=album3, Name = "Old Cruiser", Desc = "Some description",  UserId = 3, CreationDate = new DateTime(1991, 3, 2),  Path = Path.Combine("Content", "img", "11.bmp") },
                 new Image() { Id = 12, AlbumId=3, Album=album3, Name = "Motard", Desc = "Some description",  UserId = 3, CreationDate = new DateTime(1991, 3, 2),  Path = Path.Combine("Content", "img", "12.jpg") },
                   new Image() { Id = 13, AlbumId=3, Album=album3, Name = "Yamaha R1", Desc = "Some description",  UserId = 3, CreationDate = new DateTime(1991, 3, 2),  Path = Path.Combine("Content", "img", "13.png") },
                   new Image() { Id = 14, AlbumId=3, Album=album3, Name = "GSR", Desc = "Some description",  UserId = 3, CreationDate = new DateTime(1991, 3, 2),  Path = Path.Combine("Content", "img", "14.png") }
                };

            foreach (Image pic in img)
            {
                db.Images.Add(pic);
            }

            album1.Images.Add(img[0]);
            album1.Images.Add(img[1]);
            album1.Images.Add(img[2]);
            album1.Images.Add(img[3]);
            album2.Images.Add(img[4]);
            album2.Images.Add(img[5]);
            album2.Images.Add(img[6]);
            album3.Images.Add(img[7]);
            album3.Images.Add(img[8]);
            album3.Images.Add(img[9]);

            db.Albums.Add(album1);
            db.Albums.Add(album2);
            db.Albums.Add(album3);



            db.SaveChanges();

            base.Seed(db);

            db.SaveChanges();
        }
    }


}
