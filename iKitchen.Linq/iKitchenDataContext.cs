﻿using System;
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
    * Author: allentranks
    * Create Time: 2/22/2015 10:29:05 PM
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
        public DbSet<Role> Role { get; set; }     
        public DbSet<SignInLog> SignInLog { get; set; }     
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }     
        #endregion
	}
}

