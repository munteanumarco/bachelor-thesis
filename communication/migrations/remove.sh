#!/bin/sh
dotnet ef migrations remove --startup-project API/API.csproj --project DataAccessLayer/DataAccessLayer.csproj

