﻿// ------------------------------------------------------------------------------
// 
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// 
// This code is licensed under the MIT License.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore;

namespace Microsoft.Identity.Client.ApiConfig
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractAcquireTokenParameterBuilder<T>
        where T : AbstractAcquireTokenParameterBuilder<T>
    {
        internal AcquireTokenCommonParameters CommonParameters { get; } = new AcquireTokenCommonParameters();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken);

        internal abstract ApiEvent.ApiIds CalculateApiEventId();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<AuthenticationResult> ExecuteAsync()
        {
            return ExecuteAsync(CancellationToken.None);
        }

        /// <summary>
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        protected T WithScopes(IEnumerable<string> scopes)
        {
            CommonParameters.Scopes = scopes;
            return (T)this;
        }

        /// <summary>
        /// </summary>
        /// <param name="extraQueryParameters"></param>
        /// <returns></returns>
        public T WithExtraQueryParameters(Dictionary<string, string> extraQueryParameters)
        {
            CommonParameters.ExtraQueryParameters = extraQueryParameters ?? new Dictionary<string, string>();
            return (T)this;
        }

        // This exists for back compat with old-style API.  Once we deprecate it, we can remove this.
        internal T WithExtraQueryParameters(string extraQueryParameters)
        {
            return WithExtraQueryParameters(CoreHelpers.ParseKeyValueList(extraQueryParameters, '&', true, null));
        }

        /// <summary>
        ///     TODO: replicate the options here that we have in ApplicationBuilder for an AuthorityInfo class?
        /// </summary>
        /// <param name="authorityUri"></param>
        /// <returns></returns>
        public T WithAuthorityOverride(string authorityUri)
        {
            CommonParameters.AuthorityOverride = authorityUri;
            return (T)this;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Validate()
        {
        }

        internal void ValidateAndCalculateApiId()
        {
            Validate();
            CommonParameters.ApiId = CalculateApiEventId();
        }
    }
}