FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Pwneu.Submissions/Pwneu.Submissions.csproj", "./Pwneu.Submissions/"]
RUN dotnet restore "./Pwneu.Submissions/Pwneu.Submissions.csproj"
COPY "src/" .
WORKDIR "/src/Pwneu.Submissions"
RUN dotnet build "Pwneu.Submissions.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pwneu.Submissions.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY ["scripts/wait-for-it.sh", "/usr/bin/wait-for-it.sh"]
USER root
RUN chmod +x /usr/bin/wait-for-it.sh
COPY --from=publish /app/publish .
ENTRYPOINT ["wait-for-it.sh", "-t", "0", "pwneu.submissions.db:5432", "--", "dotnet", "Pwneu.Submissions.dll"]
