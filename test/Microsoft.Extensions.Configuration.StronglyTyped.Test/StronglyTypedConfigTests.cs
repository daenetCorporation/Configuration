// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license informationusing System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using System;

namespace Microsoft.Extensions.Configuration.StronglyTyped.Test
{
    public class StronglyTypedConfigTests
    {
        /// <summary>
        /// Demonstrates and Tests loading of typed configuration from string.
        /// </summary>
        [Fact]
        public void LosdConfigFromString()
        {

            string config = @"{
                              ""Test"": 10,
  
                              ""MySettingClass"": {
                                            ""SomeInt"": 77,
                                            ""SomeString"": ""Hello :)"",
                                            ""SomeFloat"": 123.567
                              },
                             ""TestDouble"" : 23.445}";

            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();

            cfgBuilder.AddStronglyTypedConfig(config);

            var configRoot = cfgBuilder.Build();

            int intVal = configRoot.GetStronglyTypedValue<int>("Test");

            Assert.True(intVal == 10);

            double dblVal = configRoot.GetStronglyTypedValue<double>("TestDouble");

            Assert.True(dblVal == 23.445);

            MySettings mySettVAl = configRoot.GetStronglyTypedValue<MySettings>("MySettingClass");

            Assert.True(mySettVAl.SomeFloat == (float)123.567);
            Assert.True(mySettVAl.SomeInt == 77);
            Assert.True(mySettVAl.SomeString == "Hello :)");
        }


        /// <summary>
        /// Demonstrates and Tests loading of typed configuration from JSON file.
        /// </summary>
        [Fact]
        public void LoadConfig1FromFile()
        {
            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();

            cfgBuilder.AddStronglyTypedConfigFile("TestSettings1.json");

            var configRoot = cfgBuilder.Build();

            int intVal = configRoot.GetStronglyTypedValue<int>("TestInt");
            Assert.True(intVal == 32767);

            double dblVal = configRoot.GetStronglyTypedValue<double>("TestDouble");
            Assert.True(dblVal == 77.71);

            double dblLong = configRoot.GetStronglyTypedValue<double>("TestLong");
            Assert.True(dblLong == 65538);

            MySettings mySettVAl = configRoot.GetStronglyTypedValue<MySettings>("ComplexType");


            Assert.True(mySettVAl.SomeFloat == (float)123.567);
            Assert.True(mySettVAl.SomeInt == 77);
            Assert.True(mySettVAl.SomeString == "Hello :)");
            Assert.True(new Guid(mySettVAl.SomeGuid) == new Guid("3699e0ae-8f4b-4441-a739-690ada2735fc"));
        }


        /// <summary>
        /// Demonstrates and Tests loading of typed configuration from JSON file,
        /// which contains complex type with nested complex type.
        /// </summary>
        [Fact]
        public void LoadConfig2NestedFile()
        {

            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();

            cfgBuilder.AddStronglyTypedConfigFile("TestSettings2.json");

            var configRoot = cfgBuilder.Build();

            int intVal = configRoot.GetStronglyTypedValue<int>("TestInt");
            Assert.True(intVal == 10);

            double dblVal = configRoot.GetStronglyTypedValue<double>("TestDouble");
            Assert.True(dblVal == 77.71);

            double dblLong = configRoot.GetStronglyTypedValue<double>("TestLong");
            Assert.True(dblLong == 65538);

            MySettings2 mySett2 = configRoot.GetStronglyTypedValue<MySettings2>("MySettings2");

            Assert.True(mySett2.SomeFloat == (float)0.99);
            Assert.True(mySett2.SomeInt == 77);
            Assert.True(mySett2.SomeString == "Hello :)");

            Assert.True(mySett2.Settings.SomeString == "Hello from nested :)");
            Assert.True(mySett2.Settings.SomeFloat == (float)71.701);
            Assert.True(mySett2.Settings.SomeInt == 88);
        }

    }
}
