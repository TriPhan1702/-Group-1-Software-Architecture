//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComicNow.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Page
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public int ChapterId { get; set; }
        public string FileName { get; set; }
        public string URL { get; set; }
        public int PageNumber { get; set; }
    
        public virtual Chapter Chapter { get; set; }
        public virtual Comic Comic { get; set; }
    }
}
