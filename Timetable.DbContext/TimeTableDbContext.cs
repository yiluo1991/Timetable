using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Timetable.DbContext.Models;
using Timetable.DbContext.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Timetable.DbContext
{
    public class TimeTableDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private string _connectionName;
        public TimeTableDbContext(DbContextOptions options) : base(options) { 
           
        }

        public TimeTableDbContext() : base()
        {
            
        }
        public TimeTableDbContext(string guid) : base()
        {
            _connectionName = guid;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionName == null)
            {
                // var conn = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
                //  optionsBuilder.UseNpgsql(SurveyStar.Infrastructure.Security.WebConfigDES.ConfigDES.Decrypt(conn));
              // optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Server=127.0.0.1;Port=5432;User Id=postgres;Password=1234;Database=Timetable");

              optionsBuilder.UseLazyLoadingProxies().UseNpgsql(Security.WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString));
            }
            else
            {
                optionsBuilder.UseNpgsql(_connectionName);
            }
        }

        /// <summary>
        ///     行政班级人员信息备份
        /// </summary>
       public virtual DbSet<AdministrativeClass> AdministrativeClasses { get; set; }

        /// <summary>
        ///     行政班级历史备份
        /// </summary>
        public virtual DbSet<AdministrativeClassBackup> AdministrativeClassBackups { get; set; }


        /// <summary>
        ///     文章数据集
        /// </summary>
        public virtual DbSet<Article> Articles { get; set; }

        /// <summary>
        ///     频道数据集
        /// </summary>
        public virtual DbSet<Channel> Channels { get; set; }


        /// <summary>
        ///     学院数据集
        /// </summary>
        public virtual DbSet<College> Colleges { get; set; }

        /// <summary>
        ///     排课数据集
        /// </summary>
        public virtual DbSet<Course> Courses { get; set; }

        /// <summary>
        ///     单独选课数据集
        /// </summary>
        public virtual DbSet<CoursePick> CoursePicks { get; set; }

        /// <summary>
        ///     系数据集
        /// </summary>
        public virtual DbSet<Department> Departments { get; set; }

        /// <summary>
        ///     管理员数据集
        /// </summary>
        public virtual DbSet<Employee> Employees { get; set; }

        /// <summary>
        ///     管理员登录日志
        /// </summary>
        public virtual DbSet<EmployeeLoginLog> EmployeeLoginLogs { get; set; }

        /// <summary>
        ///     管理员角色数据集
        /// </summary>
        public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

        /// <summary>
        ///     通知公告数据集
        /// </summary>
        public virtual DbSet<Notice> Notices { get; set; }

        /// <summary>
        ///     问卷调查列表数据集
        /// </summary>
        public virtual DbSet<Paper> Papers { get; set; }

        /// <summary>
        ///     权限页数据集
        /// </summary>
        public virtual DbSet<PermissionGroup> PermissionGroups { get; set; }

        /// <summary>
        ///     权限项数据集
        /// </summary>
        public virtual DbSet<PermissionLine> PermissionLines { get; set; }

        /// <summary>
        ///     角色数据集
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        ///     角色权限关系数据集
        /// </summary>
        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        ///     学期数据集
        /// </summary>
        public virtual DbSet<SchoolTerm> SchoolTerms { get; set; }

        /// <summary>
        ///     学生数据集
        /// </summary>
        public virtual DbSet<Student> Students { get; set; }

        /// <summary>
        ///     科目数据集
        /// </summary>
        public virtual DbSet<Subject> Subjects { get; set; }

      
        /// <summary>
        ///     教师数据集
        /// </summary>
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.ApplyConfiguration(new AdministrativeClassConfiguration());
            modelBuilder.ApplyConfiguration(new AdministrativeClassBackupConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelConfiguration());
            modelBuilder.ApplyConfiguration(new CollegeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new CoursePIckConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeLoginLogConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeRoleConfiguration());
            modelBuilder.ApplyConfiguration(new NoticeConfiguration());
            modelBuilder.ApplyConfiguration(new PaperConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionGroupConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionLineConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolTermConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());

        }
    }
}