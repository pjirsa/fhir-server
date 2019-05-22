﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

namespace Microsoft.Health.Fhir.Core.Features.Search.Expressions
{
    /// <summary>
    /// Represents a visitor for expression tree.
    /// </summary>
    /// <typeparam name="TContext">The type of the context parameter passed into each Visit method</typeparam>
    /// <typeparam name="TOutput">The output type</typeparam>
    public interface IExpressionVisitor<in TContext, out TOutput>
    {
        /// <summary>
        /// Visits the <see cref="SearchParameterExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitSearchParameter(SearchParameterExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="BinaryExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitBinary(BinaryExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="ChainedExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitChained(ChainedExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="MissingFieldExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitMissingField(MissingFieldExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="MissingSearchParameterExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitMissingSearchParameter(MissingSearchParameterExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="MultiaryExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitMultiary(MultiaryExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="StringExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitString(StringExpression expression, TContext context);

        /// <summary>
        /// Visits the <see cref="CompartmentSearchExpression"/>.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <param name="context">The input</param>
        TOutput VisitCompartment(CompartmentSearchExpression expression, TContext context);
    }
}
