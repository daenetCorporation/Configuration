// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Provide useful helper method for dealing with stringly typed configuration.
    /// </summary>
    public static class StronglyTypedConfigExtensions
    {
        /// <summary>
        /// Extracts the configuration value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type of the value to be retrieved.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The section name of the configuration from where the value has to be returned</param>
        /// <param name="serializerFnc">The function to be used to deserialize the configuration string value into the specified type.
        /// If not specified, JSON deserizlizer will be used.
        /// By using of this callback function, you can inject any kind of deserizlizetion.</param>
        /// <returns>The converted value.</returns>
        public static T GetStronglyTypedValue<T>(this IConfiguration configuration, string key,
         Func<string, T> serializerFnc = null)
        {
            return GetStronglyTypedValue<T>(configuration, key, default(T), serializerFnc);
        }

        /// <summary>
        /// Extracts the configuration value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The configuration key for the value to convert. In a case of JSON, this would be a section name.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        /// <param name="serializerFnc">The function to be used to deserialize the configuration string value into the specified type.
        /// If not specified, JSON deserizlizer will be used.
        /// By using of this callback function, you can inject any kind of deserizlizetion.</param>
        /// <returns>The converted value.</returns>
        public static T GetStronglyTypedValue<T>(this IConfiguration configuration, string key, T defaultValue,
            Func<string, T> serializerFnc = null)
        {
            var value = configuration.GetSection(key).Value;
            if (value != null)
            {           
                T tVal;

                if (serializerFnc == null)
                    tVal = deserializeJson<T>(value);
                else
                    tVal = serializerFnc(value);

                return tVal;
            }
            else
                return defaultValue;           
        }


        /// <summary>
        /// Adds the stronlgy type provider.
        /// </summary>
        /// <param name="cfgBuilder">Configuration builder instance.</param>
        /// <param name="config">Configuration as string.</param>
        /// <param name="propertyBuilderAction">Callback function, which extracts property list of
        /// all values. If not specified, it is assumed tht configuration is in JSON format.</param>
        public static void AddStronglyTypedConfig(this ConfigurationBuilder cfgBuilder,
         string config = null, Func<string,Dictionary<string, object>> propertyBuilderAction = null)
        {
            cfgBuilder.Add(new StronglyTypedConfigSource(config: config));
        }

        /// <summary>
        /// Adds the stronlgy type provider.
        /// </summary>
        /// <param name="cfgBuilder">Configuration builder instance.</param>
        /// <param name="configFileName">The file which contains configuration.</param>
        /// <param name="propertyBuilderAction">Callback function, which extracts property list of
        /// all values. If not specified, it is assumed tht configuration is in JSON format.</param>
        public static void AddStronglyTypedConfigFile(this ConfigurationBuilder cfgBuilder,
            string configFileName = null, 
            Func<string,Dictionary<string, object>> propertyBuilderAction = null)
        {
            cfgBuilder.Add(new StronglyTypedConfigSource(configFileName: configFileName));
        }


        #region Private Methods
        /// <summary>
        /// Deserializes the given string into given object.
        /// </summary>
        /// <typeparam name="T">The type to be deserialized.</typeparam>
        /// <param name="jsonConfig">The string value to be deserialized.</param>
        /// <returns></returns>
        private static T deserializeJson<T>(string jsonConfig)
        {
            T tVal = JsonConvert.DeserializeObject<T>(jsonConfig);
            return tVal;
        }
        #endregion
    }
}
