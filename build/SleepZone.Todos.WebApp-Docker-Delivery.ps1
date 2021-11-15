
# =====SRC=====
CD ../src

# =====Build=====
docker build -t clark159/sleepzone-todos-webapp -f ./SleepZone.Todos.WebApp/Dockerfile .

# =====Run=====
docker run -d -p 44392:80 clark159/sleepzone-todos-webapp