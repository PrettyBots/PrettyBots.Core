﻿using PrettyBots.Environment.Responses;
using PrettyBots.Environment.Responses.TextBased;
using PrettyBots.Interactions.Validators.Abstraction;
using PrettyBots.Interactions.Validators.Abstraction.Model;

namespace PrettyBots.Tests.Environment.Validators.Invalid;

/// <summary>
/// Tests abstract validator definition.
/// </summary>
public abstract class InvalidAbstractValidator : ResponseValidator<TextResponse>
{
    protected override ValueTask<ValidationResult> ValidateAsync(TextResponse response, IValidatorConfig config) { throw new NotImplementedException(); }
}