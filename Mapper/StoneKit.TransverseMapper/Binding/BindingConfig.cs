namespace StoneKit.TransverseMapper.Binding
{
    internal class BindingConfig
    {
        private readonly Dictionary<string, List<string>> _oneToOneBindFields = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<BindingFieldPath>> _bindFieldsPath = new Dictionary<string, List<BindingFieldPath>>();
        private readonly Dictionary<string, Type> _bindTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<object, object>> _customTypeConverters = new Dictionary<string, Func<object, object>>();
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        /// <summary>
        /// Binds a custom converter to the specified target field.
        /// </summary>
        internal void BindConverter(string targetName, Func<object, object> func)
        {
            _customTypeConverters[targetName] = func;
        }

        /// <summary>
        /// Binds fields based on source and target paths.
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
        /// Binds a type to the specified target field.
        /// </summary>
        internal void BindType(string targetName, Type value)
        {
            _bindTypes[targetName] = value;
        }

        /// <summary>
        /// Retrieves the binding field for the specified source field name.
        /// </summary>
        internal Maybe<List<string>> GetBindField(string sourceName)
        {
            List<string> result;
            bool exists = _oneToOneBindFields.TryGetValue(sourceName, out result);
            return new Maybe<List<string>>(result, exists);
        }

        /// <summary>
        /// Retrieves the binding field path for the specified field name.
        /// </summary>
        internal Maybe<List<BindingFieldPath>> GetBindFieldPath(string fieldName)
        {
            List<BindingFieldPath> result;
            bool exists = _bindFieldsPath.TryGetValue(fieldName, out result);
            return new Maybe<List<BindingFieldPath>>(result, exists);
        }

        /// <summary>
        /// Retrieves the bound type for the specified target field name.
        /// </summary>
        internal Maybe<Type> GetBindType(string targetName)
        {
            Type result;
            bool exists = _bindTypes.TryGetValue(targetName, out result);
            return new Maybe<Type>(result, exists);
        }

        /// <summary>
        /// Retrieves the custom type converter for the specified target field name.
        /// </summary>
        internal Maybe<Func<object, object>> GetCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.GetValue(targetName);
        }

        /// <summary>
        /// Checks if a custom type converter is defined for the specified target field name.
        /// </summary>
        internal bool HasCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.ContainsKey(targetName);
        }

        /// <summary>
        /// Ignores the specified source field.
        /// </summary>
        internal void IgnoreSourceField(string sourceName)
        {
            _ignoreFields.Add(sourceName);
        }

        /// <summary>
        /// Checks if the specified source field is ignored.
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
