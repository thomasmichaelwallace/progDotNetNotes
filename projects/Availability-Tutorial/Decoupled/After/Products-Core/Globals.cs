﻿using System;

namespace Product_Service
{
    public static class Globals
    {
        public static string HostName { get; set; }
        public static Guid ProductEventStreamId { get; set; }
        public static string StoragePath {get; set; }
        public static int PageSize { get; set; }
        public static Uri ProductFeed { get; set; }
    }

}
