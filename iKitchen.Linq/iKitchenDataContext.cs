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
    * Company: FalconMeta
    * Author: allentranks
    * Create Time: 2/5/2015 12:03:30 AM
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
        public DbSet<OpLog> OpLog { get; set; }     
        public DbSet<Role> Role { get; set; }     
        public DbSet<SignInLog> SignInLog { get; set; }     
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }     
        #endregion
	}
}

