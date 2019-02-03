cd `dirname $0`

apt-get update
apt-get install -y --no-install-recommends libc6-dev

dotnet tool install --global coverlet.console
dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools

for path in `find "tests" -maxdepth 2 -type f -name "*.csproj"`
do
    output="../TestResults/$(basename ${path%.*})-opencover.xml"
    dotnet test "$path" -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:CoverletOutput=$output;
done

tools\reportgenerator "--reports:tests/TestResults/*-opencover.xml" "--targetdir:tests/TestResults/html"
