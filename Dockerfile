FROM mcr.microsoft.com/dotnet/sdk as base

WORKDIR /workspace
COPY FAKEWEBCOMIC .
RUN dotnet build -c Release
RUN dotnet publish --no-build -c Release -o out FakeWebcomic.Client/*.csproj

FROM mcr.microsoft.com/dotnet/aspnet

WORKDIR /workspace
COPY --from=base workspace/out .
CMD ["dotnet", "FakeWebcomic.Client.dll"]  // not machine base, it can be run in mac, window, linux
