﻿//
// Copyright 2015 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Steeltoe.CloudFoundry.Connector.Services;

using Xunit;

namespace Steeltoe.CloudFoundry.Connector.PostgreSql.Test
{
    public class PostgresProviderConfigurerTest
    {
        [Fact]
        public void UpdateConfiguration_WithNullPostgresServiceInfo_ReturnsExpected()
        {
            PostgresProviderConfigurer configurer = new PostgresProviderConfigurer();
            PostgresProviderConnectorOptions config = new PostgresProviderConnectorOptions()
            {
                Host ="localhost",
                Port = 1234,
                Username = "username",
                Password = "password",
                Database = "database"
            };
            configurer.UpdateConfiguration(null, config);

            Assert.Equal("localhost", config.Host);
            Assert.Equal(1234, config.Port);
            Assert.Equal("username", config.Username);
            Assert.Equal("password", config.Password);
            Assert.Equal("database", config.Database);
            Assert.Null( config.ConnectionString);

        }
        [Fact]
        public void UpdateConfiguration_WithPostgresServiceInfo_ReturnsExpected()
        {
            PostgresProviderConfigurer configurer = new PostgresProviderConfigurer();
            PostgresProviderConnectorOptions config = new PostgresProviderConnectorOptions()
            {
                Host = "localhost",
                Port = 1234,
                Username = "username",
                Password = "password",
                Database = "database"
            };
            PostgresServiceInfo si = new PostgresServiceInfo("MyId", "postgres://Dd6O1BPXUHdrmzbP:7E1LxXnlH2hhlPVt@192.168.0.90:5432/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355");

            configurer.UpdateConfiguration(si, config);

            Assert.Equal("192.168.0.90", config.Host);
            Assert.Equal(5432, config.Port);
            Assert.Equal("Dd6O1BPXUHdrmzbP", config.Username);
            Assert.Equal("7E1LxXnlH2hhlPVt", config.Password);
            Assert.Equal("cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355", config.Database);

        }

        [Fact]
        public void Configure_NoServiceInfo_ReturnsExpected()
        {
            PostgresProviderConnectorOptions config = new PostgresProviderConnectorOptions()
            {
                Host = "localhost",
                Port = 1234,
                Username = "username",
                Password = "password",
                Database = "database"
            };

            PostgresProviderConfigurer configurer = new PostgresProviderConfigurer();
            var opts = configurer.Configure(null, config);
            Assert.True(opts.Contains("Host=localhost;"));
            Assert.True(opts.Contains("Port=1234;"));
            Assert.True(opts.Contains("Username=username;"));
            Assert.True(opts.Contains("Password=password;"));
            Assert.True(opts.Contains("Database=database;"));
        }

        [Fact]
        public void Configure_ServiceInfoOveridesConfig_ReturnsExpected()
        {
            PostgresProviderConnectorOptions config = new PostgresProviderConnectorOptions()
            {
                Host = "localhost",
                Port = 1234,
                Username = "username",
                Password = "password",
                Database = "database"
            };

            PostgresProviderConfigurer configurer = new PostgresProviderConfigurer();
            PostgresServiceInfo si = new PostgresServiceInfo("MyId", "postgres://Dd6O1BPXUHdrmzbP:7E1LxXnlH2hhlPVt@192.168.0.90:5432/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355");

            var opts = configurer.Configure(si, config);

            Assert.True(opts.Contains("Host=192.168.0.90;"));
            Assert.True(opts.Contains("Port=5432;"));
            Assert.True(opts.Contains("Username=Dd6O1BPXUHdrmzbP;"));
            Assert.True(opts.Contains("Password=7E1LxXnlH2hhlPVt;"));
            Assert.True(opts.Contains("Database=cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355;"));
        }
    }
}
