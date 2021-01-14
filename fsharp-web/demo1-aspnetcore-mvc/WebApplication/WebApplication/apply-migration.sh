#!/bin/sh

cat migration.sql | sqlite3 app.db

