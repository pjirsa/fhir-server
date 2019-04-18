// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using EnsureThat;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Health.Fhir.Core.Configs;
using Microsoft.Health.Fhir.Core.Exceptions;
using Microsoft.Health.Fhir.Core.Features;
using Microsoft.Net.Http.Headers;

namespace Microsoft.Health.Fhir.Api.Features.Filters
{
    /// <summary>
    /// A filter that validates the Accept and Prefer headers as well as the destination related query parameters in the request.
    /// Short-circuits the pipeline if they are invalid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class ValidateExportHeadersFilterAttribute : ActionFilterAttribute
    {
        private const string PreferHeaderName = "Prefer";
        private const string PreferHeaderExpectedValue = "respond-async";
        private readonly List<string> _supportedDestinationTypes;

        // For now we will use a hardcoded list to determine what query parameters we will
        // allow for export requests. In the future, once we add export and other operations
        // to the capabilityes statement, we can derive this list from there (via the ConformanceProvider).
        private readonly List<string> _supportedQueryParams;

        public ValidateExportHeadersFilterAttribute(IOptions<OperationsConfiguration> operationsConfig)
        {
            EnsureArg.IsNotNull(operationsConfig?.Value?.Export, nameof(operationsConfig));

            _supportedDestinationTypes = operationsConfig.Value.Export.SupportedDestinations;
            _supportedQueryParams = new List<string>()
            {
                KnownQueryParameterNames.DestinationType,
                KnownQueryParameterNames.DestinationConnectionString,
            };
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Accept, out var acceptHeaderValue) ||
                acceptHeaderValue.Count != 1 ||
                !string.Equals(acceptHeaderValue[0], ContentType.JSON_CONTENT_HEADER, StringComparison.OrdinalIgnoreCase))
            {
                throw new RequestNotValidException(string.Format(Resources.UnsupportedHeaderValue, HeaderNames.Accept));
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(PreferHeaderName, out var preferHeaderValue) ||
                preferHeaderValue.Count != 1 ||
                !string.Equals(preferHeaderValue[0], PreferHeaderExpectedValue, StringComparison.OrdinalIgnoreCase))
            {
                throw new RequestNotValidException(string.Format(Resources.UnsupportedHeaderValue, PreferHeaderName));
            }

            var queryCollection = context.HttpContext.Request.Query;

            // Validate that the request does not contain query parameters that are not supported.
            foreach (string paramName in queryCollection.Keys)
            {
                if (!_supportedQueryParams.Contains(paramName))
                {
                    throw new RequestNotValidException(string.Format(Resources.UnsupportedParameter, paramName));
                }
            }

            if (!queryCollection.ContainsKey(KnownQueryParameterNames.DestinationType)
               || string.IsNullOrWhiteSpace(queryCollection[KnownQueryParameterNames.DestinationType])
               || !_supportedDestinationTypes.Contains(queryCollection[KnownQueryParameterNames.DestinationType]))
            {
                throw new RequestNotValidException(string.Format(Resources.UnsupportedParameterValue, KnownQueryParameterNames.DestinationType));
            }

            if (!queryCollection.ContainsKey(KnownQueryParameterNames.DestinationConnectionString)
                || string.IsNullOrWhiteSpace(queryCollection[KnownQueryParameterNames.DestinationConnectionString]))
            {
                throw new RequestNotValidException(string.Format(Resources.UnsupportedParameterValue, KnownQueryParameterNames.DestinationConnectionString));
            }
        }
    }
}
