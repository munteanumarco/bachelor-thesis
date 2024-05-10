#!/bin/sh
dotnet ef database update --startup-project API/API.csproj --project DataAccessLayer/DataAccessLayer.csproj

