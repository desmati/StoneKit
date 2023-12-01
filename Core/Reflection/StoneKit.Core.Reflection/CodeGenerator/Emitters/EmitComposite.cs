namespace System.Reflection
{
    /// <summary>
    /// Represents a composite emitter that combines multiple emitters into a single emitter.
    /// </summary>
    public sealed class EmitComposite : IEmitter
    {
        private readonly List<IEmitter> _nodes = new List<IEmitter>();

        /// <summary>
        /// Emits IL instructions by invoking the Emit method on each node in the composite.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        public void Emit(CodeGenerator generator)
        {
            // Iterate through each node in the composite and invoke their Emit method
            _nodes.ForEach(x => x.Emit(generator));
        }

        /// <summary>
        /// Adds an emitter node to the composite.
        /// </summary>
        /// <param name="node">The IEmitter representing the node to add.</param>
        /// <returns>The EmitComposite instance for method chaining.</returns>
        public EmitComposite Add(IEmitter node)
        {
            // Add the node to the list if it is not null
            if (node.IsNotNull())
            {
                _nodes.Add(node);
            }
            return this;
        }
    }
}
