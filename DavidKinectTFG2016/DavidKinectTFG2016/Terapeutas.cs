//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DavidKinectTFG2016
{
    using System;
    using System.Collections.Generic;
    
    public partial class Terapeutas
    {
        public int idTerapeuta { get; set; }
        public string nombreTerapeuta { get; set; }
        public string apellidosTerapeuta { get; set; }
        public string usuario { get; set; }
        public string nifTerapeuta { get; set; }
        public System.DateTime nacimientoTerapeuta { get; set; }
    
        public virtual Usuarios Usuarios { get; set; }
    }
}