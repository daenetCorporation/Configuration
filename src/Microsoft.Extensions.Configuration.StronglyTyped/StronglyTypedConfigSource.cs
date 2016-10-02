// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license informationusing Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Configuration source for StringType provider. It supports configuration specified as 'string' or as 'file'.
    /// </summary>
    public class StronglyTypedConfigSource : IConfigurationSource
    {
        private string m_ConfigFileName;

        private string m_Config;

        private Func<string, Dictionary<string, object>> m_SerializerAction;

        /// <summary>
        /// Configuration file name.
        /// </summary>
        public string ConfigFileName
        {
            get
            {
                return m_ConfigFileName;
            }
        }


        /// <summary>
        /// Configuration as string. If configuration is loaded from file, this value will be populated
        /// with configuration file content.
        /// </summary>
        public string Config
        {
            get
            {
                return m_Config;
            }
        }


        private static Dictionary<string, object> getObjectsFomJson(string configuration)
        {
            Dictionary<string, object> props = new Dictionary<string, object>();

            JObject vals = JsonConvert.DeserializeObject<JObject>(configuration);

            foreach (var val in vals)
            {
                props.Add(val.Key, val.Value.ToString());
            }
            return props;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="config"></param>
        /// <param name="propertyBuilderAction"></param>
        public StronglyTypedConfigSource(string configFileName = null, string config = null,
            Func<string, Dictionary<string, object>> propertyBuilderAction = null)
        {
            if (configFileName != null && config != null)
                throw new Exception(":( Either 'configFileName' or 'config' must be set.");

            if (configFileName == null && config == null)
                throw new Exception(":( Either 'configFileName' or 'config' must be set.");

            if (propertyBuilderAction == null)
                m_SerializerAction = getObjectsFomJson;

            m_ConfigFileName = configFileName;
            m_Config = config;
        }


        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            if (m_ConfigFileName != null)
            {
                if (File.Exists(m_ConfigFileName) == false)
                    throw new Exception($":( Specified configuration file {m_ConfigFileName} does not exist.");

                m_Config = File.ReadAllText(m_ConfigFileName);
            }

            if (m_Config != null)
            { 
                var props = m_SerializerAction(m_Config);

                foreach (var prop in props)
                {
                    builder.Properties.Add(prop.Key, prop.Value);
                }
            }

            return new StronglyTypedConfigProvider(m_Config, builder.Properties);
        }
    }
}
