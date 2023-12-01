# Code Generator

The Code Generator is a utility designed to simplify IL code generation in C# by providing a fluent interface for emitting Intermediate Language (IL) instructions. It is particularly useful when working with dynamic code generation scenarios, such as building expression trees or creating custom proxies.

## Usage

**Generate dynamic code**

   ```csharp
   // Example: Generate a dynamic method and invoke it
   DynamicMethod dynamicMethod = new DynamicMethod("DynamicMethodName", returnType, parameterTypes);
   ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

   // Use CodeGenerator to emit IL instructions
   CodeGenerator codeGenerator = new CodeGenerator(ilGenerator);

   // ... emit instructions ...

   // Create a delegate for the dynamic method
   Delegate dynamicDelegate = dynamicMethod.CreateDelegate(typeof(Func<YourReturnType, ParamType1, ParamType2, ...>));

   // Invoke the dynamically generated method
   YourReturnType result = (YourReturnType)dynamicDelegate.DynamicInvoke(param1, param2, ...);
   ```

**More complex example**

The code below is a simplified example of dynamically generating a type and method that creates an instance of a specified target type, with the method body using IL instructions for local variable handling and returning a boxed value.

Note: The specific use case and purpose of the code generator depend on the broader context of your application or framework.

```
// Define an assembly name
var assemblyName = new AssemblyName("AssemblyName");

// Define a scope name
var scopeName = "someScope";

// Define a parent type (e.g., a generic List)
Type parentType = typeof(List<>);

// Define a target type (some arbitrary type)
Type targetType = typeof(List<>); // Replace with your actual target type

// Generate a unique type name using the assembly name, scope name, and a unique identifier
var typeName = $"{assemblyName.Name}.{scopeName}.Mapper{Guid.NewGuid().ToString("N")}";

// Define method attributes for an overridden protected virtual method
MethodAttributes overrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

// Create a dynamic assembly and module
AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);

// Define a dynamic type that inherits from the parent type (List<>)
TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType, null);

// Define a dynamic method within the dynamic type
MethodBuilder methodBuilder = typeBuilder.DefineMethod("SomeNameCreateTargetInstance", overrideProtected, targetType, Type.EmptyTypes);

// Create a CodeGenerator instance using the ILGenerator of the dynamic method
var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

// Declare a local variable of the target type
LocalBuilder builder = codeGenerator.DeclareLocal(targetType);

// Emit IL instructions to declare the local variable
EmitLocalVariable.Declare(builder).Emit(codeGenerator);

// Load the value of the local variable onto the stack
IEmitterType result = EmitBox.Box(EmitLocal.Load(builder));

// Emit IL instructions to return the boxed value
EmitReturn.Return(result, targetType).Emit(codeGenerator);
```

Explanation:

1. **Assembly and Module Setup:**
   - An `AssemblyName` is created, representing the dynamic assembly.
   - A scope name is defined.
   - The parent type is specified as a generic `List<>`.
   - The target type is specified (replace with the actual target type).

2. **Dynamic Type and Method Setup:**
   - A unique type name is generated based on the assembly name, scope name, and a unique identifier.
   - An assembly builder and module builder are created for dynamic assembly generation.
   - A dynamic type is defined, inheriting from the parent type (`List<>`).
   - A dynamic method named "SomeNameCreateTargetInstance" is defined within the dynamic type.

3. **Code Generation:**
   - A `CodeGenerator` is created using the ILGenerator of the dynamic method.

4. **Dynamic Method Body:**
   - A local variable of the target type is declared within the dynamic method.
   - IL instructions are emitted to declare the local variable using the `EmitLocalVariable` utility.
   - The value of the local variable is loaded onto the stack using `EmitLocal.Load`.
   - The value is boxed using `EmitBox.Box`.
   - IL instructions are emitted to return the boxed value using `EmitReturn.Return`.
