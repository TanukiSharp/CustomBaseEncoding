rmdir /s /q .\_temp_
rmdir /s /q .\docs

mkdir .\_temp_
mkdir .\docs

dotnet publish .\CustomBaseEncoding\CustomBaseEncoding.csproj -c Release -o .\_temp_
xcopy /y /e .\_temp_\wwwroot\ .\docs
copy /b NUL .\docs\.nojekyll

rmdir /s /q .\_temp_
