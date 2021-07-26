
# =====SRC=====
CD ../src

# =====Build=====
docker build -t clark159/mdp.webapp.lab -f ./MDP.WebApp.Lab/Dockerfile .

# =====Push=====
docker push clark159/mdp.webapp.lab