//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TF_Base.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Reparacion
    {
        public int idReparacion { get; set; }
        public int idRepuesto { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int cantRepuesto { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int cantHoras { get; set; }
        public int idOrden { get; set; }
        public int idUsuario { get; set; }
        public double subtotal { get; set; }
    
        public virtual Orden Orden { get; set; }
        public virtual Repuesto Repuesto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}