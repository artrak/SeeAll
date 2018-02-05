using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeAll.model
{
    class Model_CycleDateTime
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id_Cycle { get; set; }

        public long Id_DateTime { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string CycleTime { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string DownTime { get; set; }

        public bool? Microsimple { get; set; }

        public bool FlagDownTimeSmena { get; set; }
    }
}
