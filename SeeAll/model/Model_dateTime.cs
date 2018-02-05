using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeAll.model
{
    public class Model_dateTime
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id_DateTime { get; set; }
        public DateTime DateTime { get; set; }
    }
}
