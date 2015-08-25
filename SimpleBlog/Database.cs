using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog.Models;

namespace SimpleBlog
{
    public static class Database
    {

        private const string SessionKey = "SimpleBlog.Database.SessionKey";

        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            // hard casting if obj not castable to ISession means something s wrong so throw exception
            get { return (ISession) HttpContext.Current.Items[SessionKey]; }
        }

        /// <summary>
        /// Invoke an App startup config hibernate
        /// </summary>
        public static void Configure()
        {
            var config = new Configuration();

            // config the connn string
            config.Configure(); // empty overload => will look into web.config
            
            // add our mapping
            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>();
            mapper.AddMapping<RoleMap>();

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            // create session factory
            _sessionFactory = config.BuildSessionFactory();

        }

        /// <summary>
        /// Invoke at the start of each session /  transaction
        /// Request Lvl
        /// </summary>
        public static void OpenSession()
        {
            // Web Server multi-thread can handle multiple request same time...
            // but we need our object to only exists on a particular request...and no other
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();

        }

        /// <summary>
        /// Invoke at the start of each session /  transaction
        /// Request Lvl
        /// </summary>
        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null) session.Close();

            HttpContext.Current.Items.Remove(SessionKey);
        }
    }
}