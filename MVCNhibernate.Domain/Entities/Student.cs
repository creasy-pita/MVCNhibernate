using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCNhibernate.Domain.Entities
{
    public class Student
    {
        public virtual string StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
    }
}
