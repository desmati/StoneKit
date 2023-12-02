using StoneKit.TransverseMapper.Config.Bindings;

namespace System.Reflection.Mapping
{
    /// <summary>
    /// Represents the configuration for binding between source and target types.
    /// </summary>
    public class BindingConfig
    {
        private readonly Dictionary<string, List<string>> _oneToOneBindFields = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<BindingFieldPath>> _bindFieldsPath = new Dictionary<string, List<BindingFieldPath>>();
        private readonly Dictionary<string, Type> _bindTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<object, object>> _customTypeConverters = new Dictionary<string, Func<object, object>>();
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        /// <summary>
        /// Binds a custom type converter function for the specified target field.
        /// </summary>
        internal void BindConverter(string targetName, Func<object, object> func)
        {
            _customTypeConverters[targetName] = func;
        }

        /// <summary>
        /// Binds one-to-one mapping between source and target fields.
        /// </summary>
        internal void BindFields(List<string> sourcePath, List<string> targetPath)
        {
            var bindingFieldPath = new BindingFieldPath(sourcePath, targetPath);

            if (!bindingFieldPath.HasPath)
            {
                if (_oneToOneBindFields.ContainsKey(bindingFieldPath.SourceHead))
                {
                    _oneToOneBindFields[bindingFieldPath.SourceHead].Add(bindingFieldPath.TargetHead);
                }
                else
                {
                    _oneToOneBindFields[bindingFieldPath.SourceHead] = new List<string> { bindingFieldPath.TargetHead };
                }
            }
            else
            {
                if (_bindFieldsPath.ContainsKey(bindingFieldPath.SourceHead))
                {
                    _bindFieldsPath[bindingFieldPath.SourceHead].Add(bindingFieldPath);
                }
                else
                {
                    _bindFieldsPath[bindingFieldPath.SourceHead] = new List<BindingFieldPath> { bindingFieldPath };
                }
            }
        }

        /// <summary>
        /// Binds the type of the target field.
        /// </summary>
        internal void BindType(string targetName, Type value)
        {
            _bindTypes[targetName] = value;
        }

        /// <summary>
        /// Gets the one-to-one binding field for the specified source name, if available.
        /// </summary>
        internal Maybe<List<string>> GetBindField(string sourceName)
        {
            bool exists = _oneToOneBindFields.TryGetValue(sourceName, out var result);
            return new Maybe<List<string>>(result!, exists);
        }

        /// <summary>
        /// Gets the binding field paths for the specified field name, if available.
        /// </summary>
        internal Maybe<List<BindingFieldPath>> GetBindFieldPath(string fieldName)
        {
            bool exists = _bindFieldsPath.TryGetValue(fieldName, out var result);
            return new Maybe<List<BindingFieldPath>>(result!, exists);
        }

        /// <summary>
        /// Gets the bound type for the specified target name, if available.
        /// </summary>
        internal Maybe<Type> GetBindType(string targetName)
        {
            bool exists = _bindTypes.TryGetValue(targetName, out var result);
            return new Maybe<Type>(result!, exists);
        }

        /// <summary>
        /// Gets the custom type converter function for the specified target name, if available.
        /// </summary>
        internal Maybe<Func<object, object>> GetCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.GetValue(targetName);
        }

        /// <summary>
        /// Checks if a custom type converter is available for the specified target name.
        /// </summary>
        internal bool HasCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.ContainsKey(targetName);
        }

        /// <summary>
        /// Ignores the specified source field during mapping.
        /// </summary>
        internal void IgnoreSourceField(string sourceName)
        {
            _ignoreFields.Add(sourceName);
        }

        /// <summary>
        /// Checks if the specified source field should be ignored during mapping.
        /// </summary>
        internal bool IsIgnoreSourceField(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                return true;
            }
            return _ignoreFields.Contains(sourceName);
        }
    }
}
