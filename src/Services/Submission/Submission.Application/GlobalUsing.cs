// Third party libraries
global using MediatR;
global using FluentValidation;

// Internal libraries
global using Articles.Abstractions;
global using Articles.Abstractions.Enums;
global using Blocks.Core.FluentValidation;
global using Blocks.EntityFrameworkCore;

// Domain
global using Submission.Domain.Entities;
global using Submission.Domain.Enums;

// Application
global using Submission.Application.Features.Shared;

// Persistence
global using Submission.Persistence.Repositories;

global using AssetTypeDefinitionRepository = Blocks.EntityFrameworkCore.CachedRepository<
    Submission.Persistence.SubmissionDbContext,
    Submission.Domain.Entities.AssetTypeDefinition,
    Articles.Abstractions.Enums.AssetType>;