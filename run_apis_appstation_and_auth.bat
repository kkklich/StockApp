cd appstation/backend/Netsoftware.Xanthos.Api
start cmd.exe @cmd /k "TITLE Xanthos.Api & dotnet watch run"
cd ../../frontend
start cmd.exe @cmd /k "TITLE App Station && ng serve -o"
TIMEOUT /T 3
cd ../../

cd users/backend/Netsoftware.Xanthos.Users.Api
start cmd.exe @cmd /k "TITLE Xanthos.Users.Api & dotnet run"
TIMEOUT /T 3
cd ../../../