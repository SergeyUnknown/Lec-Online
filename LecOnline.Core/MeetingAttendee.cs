//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class MeetingAttendee
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string UserId { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<byte> Vote { get; set; }
    
        public virtual Meeting Meeting { get; set; }
    }
}
