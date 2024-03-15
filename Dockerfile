# Use the official .NET 6 runtime image for the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET 6 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Test copying a single csproj file
COPY ./IntervalProcessing/IntervalProcessing/IntervalProcessing.csproj ./IntervalProcessing/IntervalProcessing/
COPY ./CoreUtilities/CoreUtilities/CoreUtilities.csproj ./CoreUtilities/CoreUtilities/
COPY ./IntervalProcessing/IntervalProcessing.sln ./IntervalProcessing
COPY ./CoreUtilities/CoreUtilities.sln ./CoreUtilities

#RUN dotnet restore -v n
WORKDIR /src/CoreUtilities
RUN dotnet restore "CoreUtilities.sln"
WORKDIR /src/IntervalProcessing
RUN dotnet restore "IntervalProcessing.sln"

WORKDIR /src

# Copy the rest of the sources
COPY IntervalProcessing/IntervalProcessing/ IntervalProcessing/IntervalProcessing/
COPY CoreUtilities/CoreUtilities/ CoreUtilities/CoreUtilities/

WORKDIR /src/IntervalProcessing/IntervalProcessing
RUN dotnet publish -c Release -o /app


# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "IntervalProcessing.dll"]