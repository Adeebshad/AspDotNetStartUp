using Autofac;
using Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.DataAccesEF
{
    public class DataAccesEFModule : Module
    {
        public DataAccesEFModule() {
           
        }

        protected override void Load(ContainerBuilder builder)
        {
    
            base.Load(builder);
        }
    }
}
