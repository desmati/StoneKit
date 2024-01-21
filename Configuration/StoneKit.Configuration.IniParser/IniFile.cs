using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace StoneKit.Configuration.IniParser
{
    /// <summary>
    /// Represents a simple INI file parser.
    /// </summary>
    public class IniFile<T>
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string?>> _data =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, string?>>();

        private string[]? _sections;

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class.
        /// </summary>
        public IniFile() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class and loads the content from the specified file.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        public IniFile(string path)
        {
            LoadFile(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class and loads the content from the specified file with the specified encoding.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        /// <param name="encoding">The encoding of the INI file.</param>
        public IniFile(string path, Encoding encoding)
        {
            LoadFile(path, encoding);
        }

        /// <summary>
        /// Loads the content of the INI file from the specified path.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        public void LoadFile(string path)
        {
            try
            {
                var strings = System.IO.File.ReadAllLines(path);
                ParseText(strings);
            }
            catch (System.IO.FileNotFoundException)
            {
                ParseText();
            }
        }

        /// <summary>
        /// Loads the content of the INI file from the specified path with the specified encoding.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        /// <param name="encoding">The encoding of the INI file.</param>
        public void LoadFile(string path, Encoding encoding)
        {
            try
            {
                var strings = System.IO.File.ReadAllLines(path, encoding);
                ParseText(strings);
            }
            catch (System.IO.FileNotFoundException)
            {
                ParseText();
            }
        }

        /// <summary>
        /// Asynchronously loads the content of the INI file from the specified path.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadFileAsync(string path)
        {
            try
            {
                var strings = await System.IO.File.ReadAllLinesAsync(path).ConfigureAwait(false);
                ParseText(strings);
            }
            catch (System.IO.FileNotFoundException)
            {
                ParseText();
            }
        }

        /// <summary>
        /// Asynchronously loads the content of the INI file from the specified path with the specified encoding.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        /// <param name="encoding">The encoding of the INI file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadFileAsync(string path, Encoding encoding)
        {
            try
            {
                var strings = await System.IO.File.ReadAllLinesAsync(path, encoding).ConfigureAwait(false);
                ParseText(strings);
            }
            catch (System.IO.FileNotFoundException)
            {
                ParseText();
            }
        }

        private void ParseText(IEnumerable<string>? strings = null)
        {
            if (strings == null)
            {
                strings = new string[] { };
            }

            _data.Clear();
            _sections = null;
            ConcurrentDictionary<string, string?>? lastSec = null;

            foreach (var line in strings)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var pureText = line;
                if (char.IsWhiteSpace(pureText[0]))
                    pureText = pureText.Trim();

                if (pureText[0] == ';') continue;

                if (pureText[0] == '[' && pureText[^1] == ']')
                {
                    var lastBlock = pureText[1..^1];
                    lastSec = _data.GetOrAdd(lastBlock, new ConcurrentDictionary<string, string?>());
                    continue;
                }

                if (lastSec is null) continue;

                var data = pureText.Split(new[] { '=' }, 2);
                if (data.Length != 2) continue;

                var key = Trim(data[0]);
                lastSec.AddOrUpdate(key, Trim(data[1]), (_, existingValue) => Trim(data[1]));
            }
        }

        private static string Trim(string t)
        {
            if (t.Length == 0) return t;
            return char.IsWhiteSpace(t[0]) || char.IsWhiteSpace(t[^1]) ? t.Trim() : t;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified section and name.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <param name="name">The key name.</param>
        /// <returns>The value associated with the specified section and name, or <c>null</c> if not found.</returns>
        public string? this[string section, string name]
        {
            get
            {
                if (_data.TryGetValue(section, out var dsection) && dsection.TryGetValue(name, out var text))
                    return text;

                return null;
            }

            set
            {
                if (string.IsNullOrEmpty(section) || string.IsNullOrEmpty(name))
                    throw new ArgumentException("Section and name must not be null or empty.");

                _data.AddOrUpdate(section,
                    key => new ConcurrentDictionary<string, string?>() { [name] = value },
                    (key, existing) =>
                    {
                        existing.AddOrUpdate(name, value, (_, oldValue) => value);
                        return existing;
                    });
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified section and name, with a default value if not found.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <param name="name">The key name.</param>
        /// <param name="defaultValue">The default value to return if the specified section and name are not found.</param>
        /// <returns>The value associated with the specified section and name, or the default value if not found.</returns>
        public string this[string section, string name, string defaultValue]
        {
            get => this[section, name] ?? defaultValue;
        }

        /// <summary>
        /// Gets an array of section names in the INI file.
        /// </summary>
        public string[] Sections
        {
            get
            {
                if (_sections is null)
                    _sections = _data.Keys.ToArray();

                return _sections;
            }
        }

        /// <summary>
        /// Gets an array of key names in the specified section.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <returns>An array of key names in the specified section.</returns>
        public string[] GetKeys(string section)
        {
            return _data.TryGetValue(section, out var dsection) ? dsection.Keys.ToArray() : Array.Empty<string>();
        }

        #region " Writing to file "

        /// <summary>
        /// Writes the content of the INI file to the specified path.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        public void Save(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                foreach (var section in _data)
                {
                    writer.WriteLine($"[{section.Key}]");

                    foreach (var keyValuePair in section.Value)
                    {
                        writer.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
                    }

                    writer.WriteLine(); // Add a blank line after each section
                }
            }
        }

        /// <summary>
        /// Asynchronously writes the content of the INI file to the specified path.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SaveAsync(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                foreach (var section in _data)
                {
                    await writer.WriteLineAsync($"[{section.Key}]").ConfigureAwait(false);

                    foreach (var keyValuePair in section.Value)
                    {
                        await writer.WriteLineAsync($"{keyValuePair.Key}={keyValuePair.Value}").ConfigureAwait(false);
                    }

                    await writer.WriteLineAsync().ConfigureAwait(false); // Add a blank line after each section
                }
            }
        }

        #endregion

        #region " Object Handlers "


        /// <summary>
        /// Adds the content of the object to the INI file.
        /// </summary>
        /// <param name="sectionName">The section name for the object properties.</param>
        public T? this[string sectionName]
        {
            set
            {
                var obj = value;
                if (obj == null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                var objectType = obj.GetType();
                var objectProperties = objectType.GetProperties();

                if (_data.TryGetValue(sectionName, out var sectionData))
                {
                    // Section already exists, clear it before adding new properties
                    sectionData.Clear();
                }
                else
                {
                    sectionData = new ConcurrentDictionary<string, string?>();
                    _data.TryAdd(sectionName, sectionData);
                }

                foreach (var property in objectProperties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(obj);

                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                    {
                        sectionData.TryAdd(propertyName, propertyValue?.ToString());
                    }
                    else if (property.PropertyType.IsValueType)
                    {
                        // Handle value types
                        sectionData.TryAdd(propertyName, propertyValue?.ToString());
                    }
                    else if (propertyValue != null)
                    {
                        // Handle complex types
                        var complexTypeSectionName = $"{sectionName}-{propertyName}";
                        var complexTypeProperties = propertyValue.GetType().GetProperties();

                        if (!_data.ContainsKey(complexTypeSectionName))
                        {
                            _data.TryAdd(complexTypeSectionName, new ConcurrentDictionary<string, string?>());
                        }

                        var complexTypeSection = _data[complexTypeSectionName];

                        foreach (var complexProperty in complexTypeProperties)
                        {
                            var complexPropertyName = complexProperty.Name;
                            var complexPropertyValue = complexProperty.GetValue(propertyValue);

                            if (complexProperty.PropertyType.IsPrimitive || complexProperty.PropertyType == typeof(string))
                            {
                                complexTypeSection.TryAdd(complexPropertyName, complexPropertyValue?.ToString());
                            }
                        }
                    }
                }
            }

            get
            {
                var objectType = typeof(T);
                var objectInstance = Activator.CreateInstance<T>();
                var objectProperties = objectType.GetProperties();

                if (_data.TryGetValue(sectionName, out var sectionData))
                {
                    foreach (var property in objectProperties)
                    {
                        var propertyName = property.Name;

                        if (sectionData.TryGetValue(propertyName, out var propertyValue))
                        {
                            property.SetValue(objectInstance, ConvertValue(property.PropertyType, propertyValue));
                        }
                        else
                        {
                            // Set default value if property not found in INI file and default value is not null
                            property.SetValue(objectInstance, default);
                        }
                    }
                }
                else
                {
                    // Section not found, set all properties to default values
                    foreach (var property in objectProperties)
                    {
                        // Set default value if property not found in INI file and default value is not null
                        property.SetValue(objectInstance, default);
                    }
                }

                return objectInstance;
            }
        }

        private object? ConvertValue(Type targetType, string? value)
        {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            }

            return value != null
                ? Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture)
                : targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        #endregion

    }

    public sealed class IniFile : IniFile<object>
    {
        /// <summary>
        /// Loads information from the specified INI file and returns an instance of <see cref="IniFile"/>.
        /// </summary>
        /// <param name="path">The path to the INI file.</param>
        /// <returns>An instance of <see cref="IniFile"/> containing the loaded data.</returns>
        public static IniFile Parse(string path)
        {
            var iniFile = new IniFile();
            iniFile.LoadFile(path);
            return iniFile;
        }

        public static IniFile<T> Parse<T>(string path)
        {
            var iniFile = new IniFile<T>();
            iniFile.LoadFile(path);

            return iniFile;
        }
    }
}
