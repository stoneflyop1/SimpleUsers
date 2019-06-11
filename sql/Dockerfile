FROM mysql:latest

ARG SQL_PASS
# https://vsupalov.com/docker-build-pass-environment-variables/
ENV MYSQL_ROOT_PASSWORD=$SQL_PASS

WORKDIR /src
COPY . .
# https://blog.csdn.net/u013272009/article/details/84073136
COPY ./scripts/dump.sql /docker-entrypoint-initdb.d/
COPY mysql.cnf /etc/mysql/conf.d/mysqlnew.cnf
# ENTRYPOINT [ "sh", "/src/initdb.sh" ]
CMD ["mysqld"]