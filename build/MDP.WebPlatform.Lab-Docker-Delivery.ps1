
# =====SRC=====
CD ../src

# =====Build=====
docker build -t clark159/mdp.webplatform.lab -f ./MDP.WebPlatform.Lab/Dockerfile .

# =====Push=====


# =====Run=====
docker run -d -p 44392:80 clark159/mdp.webplatform.lab