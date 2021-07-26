
# =====SRC=====
CD ../src

# =====Build=====
gcloud docker build -t gcr.io/dockerrun-320206/mdp.webapp.lab -f ./MDP.WebApp.Lab/Dockerfile .

# =====Push=====
gcloud docker push gcr.io/dockerrun-320206/mdp.webapp.lab