
# =====Pull=====
gcloud docker pull gcr.io/dockerrun-320206/mdp.webapp.lab

# =====Run=====
gcloud docker run -d -p 44392:80 gcr.io/dockerrun-320206/mdp.webapp.lab