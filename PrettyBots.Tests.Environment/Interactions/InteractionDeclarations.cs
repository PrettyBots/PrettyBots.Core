using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Abstraction.Model;
using PrettyBots.Interactions.Abstraction.Model.Responses;
using PrettyBots.Interactions.Builders;
using PrettyBots.Tests.Environment.Parsers.Generic;
using PrettyBots.Tests.Environment.Parsers.Valid;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Validators.Configs;
using PrettyBots.Tests.Environment.Validators.Invalid;
using PrettyBots.Tests.Environment.Validators.Valid;

namespace PrettyBots.Tests.Environment.Interactions;

public static class InteractionDeclarations
{
    public const string V1_TEST1_KEY = nameof(V1_TEST1_KEY);
    public const string V2_TEST1_KEY = nameof(V2_TEST1_KEY);
    public const string V2_TEST2_KEY = nameof(V2_TEST2_KEY);
    public const string V2_TEST3_KEY = nameof(V2_TEST3_KEY);
    
    public const string V2_TEST1_CONFIG_PARAMETER = nameof(V2_TEST1_CONFIG_PARAMETER);
    
    // Tests the setting of default parser that will not be loaded automatically.
    // Parser should be a ValidDefaultTestResponseParser
    public static readonly IInteraction ValidAfterParsersLoadingInteraction =
        InteractionBuilder
            .WithId((uint)TestInteraction.V1)
            .WithResponse(TestResponseModelBuilder<TestResponse>
                  .WithKey(V1_TEST1_KEY))
            .Build();
    
    // Tests the interaction with configs and validators, defining them by instance or type,
    // that will not be loaded automatically. The parsers should also be loaded previously.
    public static readonly IInteraction ValidAfterValidatorLoadingInteraction =
        InteractionBuilder
            .WithId((uint)TestInteraction.V2)
            .WithResponse(TestResponseModelBuilder<AbstractResponseImpl>
                .WithKey(V2_TEST1_KEY)
                .WithValidator<ValidAcceptMultipleValidator>()
                .WithConfig(new AbstractConfigImpl(V2_TEST1_CONFIG_PARAMETER)))
            .WithResponse(TestResponseModelBuilder<TextResponse>
                .WithKey(V2_TEST2_KEY)
                .WithValidator(new ValidAcceptAnyValidator())
                .WithConfig(new GeneralConfig()))
            .WithResponse(TestResponseModelBuilder<AdditionalResponse>
                .WithKey(V2_TEST3_KEY))
            .Build();

    public static readonly IInteraction[] InvalidInteractions = {
        // Interaction that has already been defined
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.V1)
            .Build(),
        
        // Interaction that has the responses of the same type
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I1)
            .WithResponse(TestResponseModelBuilder<TextResponse>.WithKey("test"))
            .WithResponse(TestResponseModelBuilder<TextResponse>.WithKey("test1"))
            .Build(),
        
        // Interaction that has the responses with the same keys
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I2)
            .WithResponse(TestResponseModelBuilder<TextResponse>.WithKey("test"))
            .WithResponse(TestResponseModelBuilder<ImageResponse>.WithKey("test"))
            .Build(),

        // Interaction that has the response with the parser of the incorrect type
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I3)
            .WithResponse(TestResponseModelBuilder<TextResponse>
                .WithKey("test")
                .WithParser(typeof(TestResponse)))
            .Build(),
        
        // Interaction that has the response with the parser of the incorrect type
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I4)
            .WithResponse(TestResponseModelBuilder<TextResponse>
                .WithKey("test")
                .WithParser(typeof(ValidDefaultTestResponseParser)))
            .Build(),
        
        // Interaction that has the response with the validator of the incorrect type
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I5)
            .WithResponse(TestResponseModelBuilder<TextResponse>
                .WithKey("test")
                .WithValidator(typeof(TestResponse)))
            .Build(),
        
        // Interaction that has the response with the validator of the incorrect type
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I6)
            .WithResponse(TestResponseModelBuilder<TestResponse>
                .WithKey("test")
                .WithValidator(typeof(ValidAcceptMultipleValidator)))
            .Build(),
        
        // Interaction that has the response with the validator that isn't loaded
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I7)
            .WithResponse(TestResponseModelBuilder<TestResponse>
                .WithKey("test")
                .WithValidator(typeof(NeverLoadingValidator)))
            .Build(),
        
        // Interaction that has the config type incorrect for generic parameter.
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I8)
            .WithResponse(new DynamicallyTypedValidatableResponseModel("test", typeof(TestResponse), 
                typeof(ValidTestResponseParser), typeof(ValidGenericValidator),
                null, new ImageTestConfig()))
            .Build(),
        
        // Interaction that has the config type that validator does not accept as a config.
        InteractionBuilder<TestInteraction>
            .WithId(TestInteraction.I9)
            .WithResponse(new DynamicallyTypedValidatableResponseModel("test", typeof(AbstractResponseImpl), 
                typeof(ValidGenericParser), typeof(ValidAcceptMultipleValidator),
                null, new TestConfig()))
            .Build(),
    };
}