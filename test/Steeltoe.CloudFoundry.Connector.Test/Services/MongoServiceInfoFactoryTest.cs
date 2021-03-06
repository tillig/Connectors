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

using Steeltoe.Extensions.Configuration.CloudFoundry;
using Xunit;

namespace Steeltoe.CloudFoundry.Connector.Services.Test
{
    public class MongoServiceInfoFactoryTest
    {
        [Fact]
        public void Accept_AcceptsValidServiceBinding()
        {
            Service s = new Service()
            {
                Label = "p-mongodb",
                Tags = new string[] { "mongodb" },
                Name = "mongoService",
                Plan = "free",
                Credentials = new Credential() {
                    { "hostname", new Credential("192.168.0.90")},
                    { "port", new Credential("3306")},
                    { "name", new Credential("cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355")},
                    { "username", new Credential("Dd6O1BPXUHdrmzbP") },
                    { "password", new Credential("7E1LxXnlH2hhlPVt") },
                    { "uri", new Credential("mongodb://Dd6O1BPXUHdrmzbP:7E1LxXnlH2hhlPVt@192.168.0.90:3306/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355?reconnect=true") },
                    }
            };
            MongoServiceInfoFactory factory = new MongoServiceInfoFactory();
            Assert.True(factory.Accept(s));
        }

        [Fact]
        public void Accept_AcceptsNoLabelNoTagsServiceBinding()
        {
            Service s = new Service()
            {
                Label = "",
                Tags = new string[0],
                Name = "mongoService",
                Plan = "free",
                Credentials = new Credential() {
                    { "hostname", new Credential("192.168.0.90")},
                    { "port", new Credential("3306")},
                    { "name", new Credential("cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355")},
                    { "username", new Credential("Dd6O1BPXUHdrmzbP") },
                    { "password", new Credential("7E1LxXnlH2hhlPVt") },
                    { "uri", new Credential("mongodb://Dd6O1BPXUHdrmzbP:7E1LxXnlH2hhlPVt@192.168.0.90:3306/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355?reconnect=true") },
                    }
            };
            MongoServiceInfoFactory factory = new MongoServiceInfoFactory();
            Assert.True(factory.Accept(s));
        }

        [Fact]
        public void Accept_RejectsInvalidServiceBinding()
        {
            Service s = new Service()
            {
                Label = "p-mysql",
                Tags = new string[] { "foobar", "relational" },
                Name = "mySqlService",
                Plan = "100mb-dev",
                Credentials = new Credential() {
                    { "hostname", new Credential("192.168.0.90")},
                    { "port", new Credential("3306")},
                    { "name", new Credential("cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355")},
                    { "username", new Credential("Dd6O1BPXUHdrmzbP") },
                    { "password", new Credential("7E1LxXnlH2hhlPVt") },
                    { "uri", new Credential("mysql://Dd6O1BPXUHdrmzbP:7E1LxXnlH2hhlPVt@192.168.0.90:3306/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355?reconnect=true") },
                    { "jdbcUrl", new Credential("jdbc:mysql://192.168.0.90:3306/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355?user=Dd6O1BPXUHdrmzbP&password=7E1LxXnlH2hhlPVt") }
                    }
            };
            MongoServiceInfoFactory factory = new MongoServiceInfoFactory();
            Assert.False(factory.Accept(s));
        }

        [Fact]
        public void Create_CreatesValidServiceBinding()
        {
            Service s = new Service()
            {
                Label = "p-mongodb",
                Tags = new string[] { "mongodb"},
                Name = "mongodbService",
                Plan = "free",
                Credentials = new Credential() {
                    { "hostname", new Credential("192.168.0.90")},
                    { "port", new Credential("3306")},
                    { "name", new Credential("cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355")},
                    { "username", new Credential("Dd6O1BPXUHdrmzbP") },
                    { "password", new Credential("7E1LxXnlH2hhlPVt") },
                    { "uri", new Credential("mongodb://Dd6O1BPXUHdrmzbP:7E1LxXnlH2hhlPVt@192.168.0.90:3306/cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355?reconnect=true") },
                    }
            };
            MongoServiceInfoFactory factory = new MongoServiceInfoFactory();
            var info = factory.Create(s) as MongoServiceInfo;
            Assert.NotNull(info);
            Assert.Equal("mongodbService", info.Id);
            Assert.Equal("7E1LxXnlH2hhlPVt", info.Password);
            Assert.Equal("Dd6O1BPXUHdrmzbP", info.UserName);
            Assert.Equal("192.168.0.90", info.Host);
            Assert.Equal(3306, info.Port);
            Assert.Equal("cf_b4f8d2fa_a3ea_4e3a_a0e8_2cd040790355", info.Path);
            Assert.Equal("reconnect=true", info.Query);
        }
    }
}

