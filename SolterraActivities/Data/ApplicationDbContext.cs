using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Models;

namespace SolterraActivities.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //creating a Moods table from the model "Mood" - table is plural in this case, and singular mood refers to a singular description of a mood
        public DbSet<Mood> Moods { get; set; }

        // creating a Hobbies table from the model "Hobby" - singular hobby referring to one instance of that model
        public DbSet<Hobby> Hobbies { get; set; }

        // creating an Activities table from the model "Activity" - singular activity referring to one instance of that model
        public DbSet<Activity> Activities { get; set; }

        // creating an ActivityMoods table from the model "ActivityMood" - singular activitymood referring to one instance of that model
        public DbSet<ActivityMood> ActivityMoods { get; set; }


        // creating a UserActivities table from the model "UserActivity" - singular useractivity referring to one instance of that model
        public DbSet<UserActivity> UserActivities { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Inventory> Inventory { get; set; }

		public DbSet<Item> Items { get; set; }

		public DbSet<ItemType> ItemTypes { get; set; }

		public DbSet<ItemxType> ItemxTypes { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Species> Species { get; set; }
	}
}
