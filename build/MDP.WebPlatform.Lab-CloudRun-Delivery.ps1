
# =====SRC=====
CD ../src

# =====Build=====
docker build -t gcr.io/mdp-webplatform-lab/mdp.webplatform.lab -f ./MDP.WebPlatform.Lab/Dockerfile .

# =====Push=====
docker push gcr.io/mdp-webplatform-lab/mdp.webplatform.lab

# =====Deploy=====
gcloud run deploy mdp-webplatform-lab --image gcr.io/mdp-webplatform-lab/mdp.webplatform.lab --region asia-east1