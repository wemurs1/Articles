#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release
ARG APP_NAME=Journals.Api
ARG PROJECT_PATH=Services/Journals

WORKDIR /src
COPY ${PROJECT_PATH}/${APP_NAME}/${APP_NAME}.csproj ${PROJECT_PATH}/${APP_NAME}
COPY ${PROJECT_PATH}/Journals.Domain/Journals.Domain.csproj ${PROJECT_PATH}/Journals.Domain
COPY ${PROJECT_PATH}/Journals.Persistence/Journals.Persistence.csproj ${PROJECT_PATH}/Journals.Persistence
RUN dotnet restore ${PROJECT_PATH}

# Copy the rest of the files after restoring dependencies
COPY . .

# Publish the app - there is no need for "dontnet build" since "dotnet publish" will do that
RUN dotnet publish ${PROJECT_PATH}/${APP_NAME}.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# We need to define the ARG again here in order to create an ENV variable for the runtime
ARG APP_NAME
ENV APP_NAME=${APP_NAME}
ENTRYPOINT ["dotnet", "/app/Journals.Api.dll"]