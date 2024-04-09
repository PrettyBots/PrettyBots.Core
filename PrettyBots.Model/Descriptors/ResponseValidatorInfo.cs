using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using PrettyBots.Environment;
using PrettyBots.Interactions.Validators.Abstraction;

namespace PrettyBots.Model.Descriptors;

/// <summary>
/// Contains information about the response validator.
/// Gets generated when the validator is loaded into the interactions service. 
/// </summary>
public class ResponseValidatorInfo
{
    /// <summary>
    /// Type of the validator that implements the <see cref="IResponseValidator"/> type.
    /// </summary>
    public Type ValidatorType { get; }
    
    /// <summary>
    /// Type of the response that implements the <see cref="IUserResponse"/> class
    /// and gets validated using the validator.
    /// </summary>
    public Type ResponseType { get; }
    
    /// <summary>
    /// Specifies whether the validator is registered in a service collection or not.
    /// If so, the <see cref="ServiceProvider"/> will contain the provider that will be used
    /// to instantiate instances.
    /// If not, the activator should be used on a parameterless constructor
    /// to instantiate instances.
    /// </summary>
    [MemberNotNullWhen(true, nameof(ServiceProvider))]
    public bool RegisteredInTheSP { get; }
    
    /// <summary>
    /// Service provider to get the validator from if it was registered in any.
    /// </summary>
    public IServiceProvider? ServiceProvider { get; }
    
    /// <summary>
    /// List of types that implement <see cref="IValidatorConfig{TResponse}"/> type
    /// and are marked as available for the validator using ConfigurableWith attributes.
    /// </summary>
    public IReadOnlyList<Type> AvailableConfigTypes { get; }
    
    private ResponseValidatorInfo(Type validatorType, Type responseType, bool registeredInTheSp, 
        IList<Type> availableConfigTypes, IServiceProvider? serviceProvider)
    {
        ResponseType         = responseType;
        ValidatorType        = validatorType;
        ServiceProvider      = serviceProvider;
        RegisteredInTheSP    = registeredInTheSp;
        AvailableConfigTypes = new ReadOnlyCollection<Type>(availableConfigTypes);
    }

    public static ResponseValidatorInfo WithSP(Type validatorType, Type responseType, 
        IList<Type> availableConfigTypes, IServiceProvider? serviceProvider)
    {
        return new ResponseValidatorInfo(validatorType, responseType, true,
            availableConfigTypes, serviceProvider);
    }

    public static ResponseValidatorInfo WithNoSP(Type validatorType, Type responseType, 
        IList<Type> availableConfigTypes)
    {
        return new ResponseValidatorInfo(validatorType, responseType, false, 
            availableConfigTypes, null);
    }

    public IResponseValidator CreateInstance()
    {
        return RegisteredInTheSP 
            ? (IResponseValidator)ServiceProvider.GetService(ValidatorType)! 
            : (IResponseValidator)Activator.CreateInstance(ValidatorType)!;
    }
}