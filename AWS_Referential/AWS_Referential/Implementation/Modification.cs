using AWS_Referential.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AWS_Referential.Implementation
{
    public class Modification
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }
        public ObjectModification ObjectModification { get; set; }

        public int ObjectModifiedId { get; set; }
        public string ModifiedField { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModificationDate { get; set; }

        public Modification() { }
        public Modification(ObjectModification objectModification, int objectmodifiedid, string modifiedfield, string oldvalue, string newvalue)
        {
            this.ObjectModification = objectModification;
            this.ObjectModifiedId = objectmodifiedid;
            this.ModifiedField = modifiedfield;
            this.OldValue = oldvalue;
            this.NewValue = newvalue;
            this.ModificationDate = DateTime.Today;
        }
    }
}
