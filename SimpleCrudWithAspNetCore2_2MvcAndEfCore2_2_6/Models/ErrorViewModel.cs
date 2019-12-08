using System;

namespace SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}