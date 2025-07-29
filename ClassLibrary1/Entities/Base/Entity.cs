using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Core.Entities.Base
{
    public abstract class Entity<T> : EntityBase, IEntity<T> where T : struct
    {
        public virtual T Id { get; set; }
    }
}
