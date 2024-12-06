using System.Reflection;

namespace EasyDI;

public static class TypeFactory
{
    public static Func<object> CreateFactory(Type implementationType, IContainer container)
    {
        ArgumentNullException.ThrowIfNull(implementationType);

        if (implementationType.IsInterface || implementationType.IsAbstract)
        {
            throw new ArgumentException(
                $"Cannot instantiate implementation type '{implementationType.FullName}' because it is an interface or abstract class.");
        }

        var constructors =
            implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public |
                                               BindingFlags.NonPublic);

        if (constructors.Length == 0)
        {
            throw new InvalidOperationException(
                $"Type '{implementationType.FullName}' does not have any accessible constructors.");
        }

        var constructor = constructors
            .OrderBy(c => c.GetParameters().Length)
            .First();

        var parameters = constructor.GetParameters();
        var args = parameters.Select(param => container.Resolve(param.ParameterType)).ToArray();

        return () => Activator.CreateInstance(implementationType, args) ??
                     throw new InvalidOperationException(
                         $"Failed to create an instance of type '{implementationType.FullName}'.");
    }
}