
# =====SRC=====
CD ../src

# =====Build=====
docker build -t asia-east1-docker.pkg.dev/mdp-net/sleepzone-todos-webapp/sleepzone-todos-webapp -f ./SleepZone.Todos.WebApp/Dockerfile .

# =====Push=====
docker push asia-east1-docker.pkg.dev/mdp-net/sleepzone-todos-webapp/sleepzone-todos-webapp

# =====Deploy=====
gcloud run deploy sleepzone-todos-webapp --image asia-east1-docker.pkg.dev/mdp-net/sleepzone-todos-webapp/sleepzone-todos-webapp --region asia-east1

