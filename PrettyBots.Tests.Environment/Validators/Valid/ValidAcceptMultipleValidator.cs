﻿using PrettyBots.Attributes.Validators;
using PrettyBots.Tests.Environment.Responses;
using PrettyBots.Tests.Environment.Validators.Configs;
using PrettyBots.Validators.Abstraction;
using PrettyBots.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Valid;

/// <summary>
/// Tests valid validator that validates generic response
/// and accept any configurations that is targeted to <see cref="IAbstractResponse"/>.
/// </summary>
[ConfigurableWith(typeof(TestGeneralConfig))]
[ConfigurableWith(typeof(AbstractConfigImpl))]
public class ValidAcceptMultipleValidator : ResponseValidator<IAbstractResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(IAbstractResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}