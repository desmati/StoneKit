namespace StoneKit.TransverseMapper.Binding
{
    /// <summary>
    /// Represents the path information between source and target fields.
    /// </summary>
    internal sealed class BindingFieldPath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingFieldPath"/> class.
        /// </summary>
        /// <param name="sourcePath">The path of the source field.</param>
        /// <param name="targetPath">The path of the target field.</param>
        public BindingFieldPath(List<string> sourcePath, List<string> targetPath)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            HasPath = sourcePath.Count != 1 || targetPath.Count != 1;
            SourceHead = sourcePath[0];
            TargetHead = targetPath[0];
        }

        /// <summary>
        /// Gets the path of the source field.
        /// </summary>
        public List<string> SourcePath { get; }

        /// <summary>
        /// Gets the path of the target field.
        /// </summary>
        public List<string> TargetPath { get; }

        /// <summary>
        /// Gets the head of the source path.
        /// </summary>
        public string SourceHead { get; }

        /// <summary>
        /// Gets the head of the target path.
        /// </summary>
        public string TargetHead { get; }

        /// <summary>
        /// Gets a value indicating whether the binding has a path.
        /// </summary>
        public bool HasPath { get; }
    }
}
