// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license informationusing Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.IO;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// .NET Core configuration provider, which deals with strongly typed configuration values.
    /// </summary>
    public class StronglyTypedConfigProvider : IConfigurationProvider
    {
        private Dictionary<string, object> m_Properties;

        private string m_Config;

        /// <summary>
        /// Creates te instance of provider.
        /// </summary>
        /// <param name="config">Configuration as string.</param>
        /// <param name="props">List of properies extracted from configuration. Each property
        /// can hold a strongly typed value. Configuration must have at least one property.</param>
        public StronglyTypedConfigProvider(string config, Dictionary<string, object> props)
        {
            m_Config = config;
            m_Properties = props;
        }


        /// <summary>
        /// Returns an empty list of child keys.
        /// </summary>
        /// <param name="earlierKeys"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            return new List<string>();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new LoadToken();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the configuration value as string.
        /// </summary>
        /// <param name="key">The name of configuration proeprty.</param>
        /// <param name="value">The value to be retrieved.</param>
        /// <returns>string serialized value.</returns>
        public bool TryGet(string key, out string value)
        {
            if (this.m_Properties.ContainsKey(key))
            {
                value = this.m_Properties[key] as string;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }

    /// <summary>
    /// Relaod is not supported in this version.
    /// </summary>
    public class LoadToken : IChangeToken
    {
        /// <summary>
        /// Not supported
        /// </summary>
        public bool ActiveChangeCallbacks
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public bool HasChanged
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return null;
        }
    }
}
