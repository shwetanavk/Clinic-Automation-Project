//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicAutomationProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Deliveries = new HashSet<Delivery>();
        }
    
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int OrderQuantityID { get; set; }
        public int AdminId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNumber { get; set; }
        public int SupplierId { get; set; }
    
        public virtual Admin Admin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual Product Product { get; set; }
        public virtual QuantityTable QuantityTable { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
