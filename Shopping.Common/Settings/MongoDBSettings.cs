﻿

namespace Shopping.Common.Settings
{
    public class MongoDBSettings
    {
        public string Host { get; init; }
        public string Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}
