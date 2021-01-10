using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class AuditTrail
    {
        public Guid Id { get; set; }                    /*Log id*/
        public DateTime AuditDateTime { get; set; }  /*Log time*/
        public string AuditType { get; set; }           /*Create, Update or Delete*/
        public string AuditUser { get; set; }           /*Log User*/
        public string TableName { get; set; }           /*Table where rows been 
                                                          created/updated/deleted*/
        public string KeyValues { get; set; }           /*Table Pk and it's values*/
        public string OldValue { get; set; }           /*Changed column name and old value*/
        public string NewValue { get; set; }           /*Changed column name 
                                                          and current value*/
        public string ChangedColumn { get; set; }
    }
}
