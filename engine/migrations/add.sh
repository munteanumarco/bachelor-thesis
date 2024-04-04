#!/bin/sh
dotnet ef migrations add $1 --startup-project API/API.csproj --project DataAccessLayer/DataAccessLayer.csproj

