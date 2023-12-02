namespace StoneKit.TransverseMapper.Config.Bindings
{
    /// <summary>
    /// Represents a path for binding fields between source and target objects.
    /// </summary>
    internal sealed class BindingFieldPath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingFieldPath"/> class.
        /// </summary>
        /// <param name="sourcePath">The path of source fields.</param>
        /// <param name="targetPath">The path of target fields.</param>
        public BindingFieldPath(List<string> sourcePath, List<string> targetPath)
        {
            SourcePath = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));
            TargetPath = targetPath ?? throw new ArgumentNullException(nameof(targetPath));
            HasPath = sourcePath.Count != 1 || targetPath.Count != 1;
            SourceHead = sourcePath[0];
            TargetHead = targetPath[0];
        }

        /// <summary>
        /// Gets the path of source fields.
        /// </summary>
        public List<string> SourcePath { get; }

        /// <summary>
        /// Gets the path of target fields.
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
        /// Gets a value indicating whether the path has multiple steps.
        /// </summary>
        public bool HasPath { get; }
    }
}
