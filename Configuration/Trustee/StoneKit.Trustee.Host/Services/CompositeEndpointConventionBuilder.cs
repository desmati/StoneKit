namespace Trustee.Host;

using Microsoft.AspNetCore.Builder;

using System;
using System.Collections.Generic;


/// <summary>
/// Represents a composite endpoint convention builder.
/// </summary>
internal sealed class CompositeEndpointConventionBuilder : IEndpointConventionBuilder
{
    private readonly List<IEndpointConventionBuilder> _endpointConventionBuilders;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeEndpointConventionBuilder"/> class.
    /// </summary>
    /// <param name="endpointConventionBuilders">The list of endpoint convention builders to combine.</param>
    public CompositeEndpointConventionBuilder(List<IEndpointConventionBuilder> endpointConventionBuilders)
    {
        _endpointConventionBuilders = endpointConventionBuilders;
    }

    /// <summary>
    /// Adds an endpoint convention to all builders in the composite.
    /// </summary>
    /// <param name="convention">The endpoint convention to add.</param>
    public void Add(Action<EndpointBuilder> convention)
    {
        foreach (var endpointConventionBuilder in _endpointConventionBuilders)
        {
            endpointConventionBuilder.Add(convention);
        }
    }
}
