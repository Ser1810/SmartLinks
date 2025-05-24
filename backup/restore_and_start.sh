#!/bin/bash
set -e

# Запускаем PostgreSQL в фоне
docker-entrypoint.sh postgres &

PG_PID=$!

# Ждем готовности сервера
until pg_isready -h localhost -p 5432 -U puser; do
  echo "Waiting for PostgreSQL..."
  sleep 1
done

# Восстановление базы из бэкапа
echo "Restoring database..."
pg_restore -h localhost -p 5432 -U puser -d SmartLinkD --clean --if-exists /backup/SmartLink.backup

# Ждем, пока база станет доступна после восстановления
until psql -h localhost -p 5432 -U puser -d SmartLinkD -c '\q'; do
  echo "Waiting for database after restore..."
  sleep 1
done

# Оставляем сервер работать
wait $PG_PID