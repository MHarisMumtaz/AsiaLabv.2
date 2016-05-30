using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaLabv1.Repositories
{
    public class StaticDbContext
    {
        protected static AsiaLabDbEntities Context = new AsiaLabDbEntities();
        public static AsiaLabDbEntities context
        {
            get
            {
                return Context;
            }
        }
    }
}