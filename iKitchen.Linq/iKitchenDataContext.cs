using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace iKitchen.Linq
{
    /*
    * Title: DataContext
    * Description:
    * Copyright: Copyright (c) 2015
    * Company: iKitchen
    * Author: Allen.Xia
    * Create Time: 30/04/2015 9:39:30 p.m.
    * */    
	/// <summary>
	/// 
	/// </summary>
	public partial class iKitchenDataContext : DbContext
	{
		
        public iKitchenDataContext()
            : base("DefaultConnection")
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
		
        #region DbSet Method Definitions
        public DbSet<Event> Event { get; set; }     
        public DbSet<EventImage> EventImage { get; set; }     
        public DbSet<EventUser> EventUser { get; set; }     
        public DbSet<Feedback> Feedback { get; set; }     
        public DbSet<OpLog> OpLog { get; set; }     
        public DbSet<ResetPassword> ResetPassword { get; set; }     
        public DbSet<Role> Role { get; set; }     
        public DbSet<SignInLog> SignInLog { get; set; }     
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }     
        #endregion
	}
}

