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
echo "Restoring database SmartLinkNew..."
psql -h localhost -p 5432 -U puser -d SmartLinkNew -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"
psql -h localhost -p 5432 -U puser -d SmartLinkNew -f /backup/SmartLink.sql

# Ждем, пока база станет доступна после восстановления
until psql -h localhost -p 5432 -U puser -d SmartLinkNew -c '\q'; do
  echo "Waiting for database after restore..."
  sleep 1
done

echo "Database SmartLinkNew is work"

# Оставляем сервер работать
wait $PG_PID