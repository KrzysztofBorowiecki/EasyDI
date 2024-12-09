using System.Reflection;

namespace EasyDI;

/// <summary>
/// Provides factory methods for creating instances of types using dependency injection.
/// </summary>
public static class TypeFactory
{
    /// <summary>
    /// Creates a factory function to instantiate objects of the specified implementation type,
    /// resolving constructor parameters from the provided container.
    /// </summary>
    /// <param name="implementationType">The type to be instantiated.</param>
    /// <param name="container">The dependency injection container used to resolve constructor dependencies.</param>
    /// <returns>A factory function that creates an instance of the specified type.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="implementationType"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="implementationType"/> is an interface or abstract class.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the type does not have any accessible constructors or if instance creation fails.
    /// </exception>
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
        
        return () =>
        {
            var args = parameters.Select(param => container.Resolve(param.ParameterType)).ToArray();
            return Activator.CreateInstance(implementationType, args) ??
                   throw new InvalidOperationException(
                       $"Failed to create an instance of type '{implementationType.FullName}'.");
        };
    }
}
