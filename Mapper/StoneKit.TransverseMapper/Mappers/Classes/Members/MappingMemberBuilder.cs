using StoneKit.TransverseMapper.Binding;
using StoneKit.TransverseMapper.Mappers.Builder;

using System.Reflection;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Mappers.Classes.Members
{
    /// <summary>
    /// Builds a list of <see cref="MappingMemberPath"/> instances based on the provided type pair.
    /// </summary>
    internal sealed class MappingMemberBuilder
    {
        private readonly IMapperBuilderConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMemberBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration for the mapper builder.</param>
        public MappingMemberBuilder(IMapperBuilderConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Builds a list of <see cref="MappingMemberPath"/> instances based on the provided type pair.
        /// </summary>
        /// <param name="typePair">The type pair for which to build the mapping member paths.</param>
        /// <returns>A list of <see cref="MappingMemberPath"/> instances.</returns>
        public List<MappingMemberPath> Build(TypePair typePair)
        {
            return ParseMappingTypes(typePair);
        }

        private static MemberInfo[] GetPublicMembers(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            PropertyInfo[] properties = type.GetProperties(flags);
            FieldInfo[] fields = type.GetFields(flags);
            MemberInfo[] members = new MemberInfo[properties.Length + fields.Length];
            properties.CopyTo(members, 0);
            fields.CopyTo(members, properties.Length);
            return members;
        }

        private static List<MemberInfo> GetSourceMembers(Type sourceType)
        {
            var result = new List<MemberInfo>();

            MemberInfo[] members = GetPublicMembers(sourceType);
            foreach (MemberInfo member in members)
            {
                if (member.IsProperty())
                {
                    MethodInfo method = ((PropertyInfo)member).GetGetMethod();
                    if (method.IsNull())
                    {
                        continue;
                    }
                }
                result.Add(member);
            }
            return result;
        }

        private static List<MemberInfo> GetTargetMembers(Type targetType)
        {
            var result = new List<MemberInfo>();

            MemberInfo[] members = GetPublicMembers(targetType);
            foreach (MemberInfo member in members)
            {
                if (member.IsProperty())
                {
                    MethodInfo method = ((PropertyInfo)member).GetSetMethod();
                    if (method.IsNull() || method.GetParameters().Length != 1)
                    {
                        continue;
                    }
                }
                result.Add(member);
            }
            return result;
        }

        private List<string> GetTargetName(
            Maybe<BindingConfig> bindingConfig,
            TypePair typePair,
            MemberInfo sourceMember,
            Dictionary<string, string> targetBindings)
        {
            Maybe<List<string>> targetName;
            List<BindAttribute> binds = sourceMember.GetAttributes<BindAttribute>();
            BindAttribute bind = binds.FirstOrDefault(x => x.TargetType.IsNull());
            if (bind.IsNull())
            {
                bind = binds.FirstOrDefault(x => typePair.Target.IsAssignableFrom(x.TargetType));
            }
            if (bind.IsNotNull())
            {
                targetName = new Maybe<List<string>>(new List<string> { bind.MemberName });
            }
            else
            {
                targetName = bindingConfig.Map(x => x.GetBindField(sourceMember.Name));
                if (targetName.HasNoValue)
                {
                    if (targetBindings.TryGetValue(sourceMember.Name, out var targetMemberName))
                    {
                        targetName = new Maybe<List<string>>(new List<string> { targetMemberName });
                    }
                    else
                    {
                        targetName = new Maybe<List<string>>(new List<string> { sourceMember.Name });
                    }
                }
            }
            return targetName.Value;
        }

        private Dictionary<string, string> GetTest(TypePair typePair, List<MemberInfo> targetMembers)
        {
            var result = new Dictionary<string, string>();
            foreach (MemberInfo member in targetMembers)
            {
                Maybe<BindAttribute> bindAttribute = member.GetAttribute<BindAttribute>().ToMaybe()!;
                if (bindAttribute.HasNoValue)
                {
                    continue;
                }

                if (bindAttribute.Value.TargetType.IsNull() || typePair.Source.IsAssignableFrom(bindAttribute.Value.TargetType))
                {
                    result[bindAttribute.Value.MemberName] = member.Name;
                }
            }
            return result;
        }

        private bool IsIgnore(Maybe<BindingConfig> bindingConfig, TypePair typePair, MemberInfo sourceMember)
        {
            List<IgnoreAttribute> ignores = sourceMember.GetAttributes<IgnoreAttribute>();
            if (ignores.Any(x => x.TargetType.IsNull()))
            {
                return true;
            }
            if (ignores.FirstOrDefault(x => typePair.Target.IsAssignableFrom(x.TargetType))?.IsNotNull() == true)
            {
                return true;
            }
            return bindingConfig.Map(x => x.IsIgnoreSourceField(sourceMember.Name)).Value;
        }

        private List<MemberInfo> GetSourceMemberPath(List<string> fieldPath, Type sourceType)
        {
            var result = new List<MemberInfo>();
            var dummyType = sourceType;
            foreach (var path in fieldPath)
            {
                var member = GetSourceMembers(dummyType).Single(x => string.Equals(x.Name, path, StringComparison.Ordinal));
                result.Add(member);
                dummyType = member.GetMemberType();
            }
            return result;
        }

        private List<MappingMemberPath> ParseMappingTypes(TypePair typePair)
        {
            var result = new List<MappingMemberPath>();

            List<MemberInfo> sourceMembers = GetSourceMembers(typePair.Source);
            List<MemberInfo> targetMembers = GetTargetMembers(typePair.Target);

            Dictionary<string, string> targetBindings = GetTest(typePair, targetMembers);

            Maybe<BindingConfig> bindingConfig = _config.GetBindingConfig(typePair);

            foreach (MemberInfo sourceMember in sourceMembers)
            {
                if (IsIgnore(bindingConfig, typePair, sourceMember))
                {
                    continue;
                }

                List<string> targetNames = GetTargetName(bindingConfig, typePair, sourceMember, targetBindings);

                foreach (var targetName in targetNames)
                {
                    MemberInfo targetMember = targetMembers.FirstOrDefault(x => _config.NameMatching(targetName, x.Name))!;
                    if (targetMember?.IsNull() == true)
                    {
                        result.AddRange(GetBindMappingMemberPath(typePair, bindingConfig, sourceMember));
                        continue;
                    }
                    Maybe<Type> concreteBindingType = bindingConfig.Map(x => x.GetBindType(targetName));
                    if (concreteBindingType.HasValue)
                    {
                        var mappingTypePair = new TypePair(sourceMember.GetMemberType(), concreteBindingType.Value);
                        result.Add(new MappingMemberPath(sourceMember, targetMember!, mappingTypePair));
                    }
                    else
                    {
                        result.Add(new MappingMemberPath(sourceMember, targetMember!));
                    }

                    result.AddRange(GetBindMappingMemberPath(typePair, bindingConfig, sourceMember));
                }

            }
            return result;
        }

        private List<MappingMemberPath> GetBindMappingMemberPath(TypePair typePair, Maybe<BindingConfig> bindingConfig, MemberInfo sourceMember)
        {
            var result = new List<MappingMemberPath>();

            var bindFieldPath = bindingConfig.Map(x => x.GetBindFieldPath(sourceMember.Name));

            if (bindFieldPath.HasValue)
            {
                foreach (BindingFieldPath item in bindFieldPath.Value)
                {
                    var sourceMemberPath = GetSourceMemberPath(item.SourcePath, typePair.Source);
                    var targetMemberPath = GetSourceMemberPath(item.TargetPath, typePair.Target);
                    result.Add(new MappingMemberPath(sourceMemberPath, targetMemberPath));

                }
            }
            return result;
        }
    }
}
