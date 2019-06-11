$VERSION = ((git rev-parse --short=8 HEAD) | Out-String).Trim()
#####################################################################
# build and run sql
Set-Location ./sql
$sqlpass = "123456"

docker build --no-cache --build-arg SQL_PASS=$sqlpass -t dotnetusers/db:$VERSION .
docker run -d -p 3306:3306 --name dotnetuserdb dotnetusers/db:$VERSION --character-set-server=utf8 --collation-server=utf8_general_ci
#####################################################################
# build and run webapp
Set-Location ../
$connStr = "Server=dotnetuserdb;Database=dotnetusers;Uid=root;Pwd=123456;CHARSET=utf8;";
docker build --no-cache --build-arg CONN_STR=$connStr -t dotnetusers/webapp .
docker run -d -p 5000:80 --link dotnetuserdb --name dotnetuserapp dotnetusers/webapp