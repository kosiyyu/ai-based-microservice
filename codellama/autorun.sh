#!/bin/bash

docker build -t codellama .

docker run -d -p 11434:11434 --name codellama codellama

until [ "$(docker inspect -f {{.State.Running}} codellama)"=="true" ]; do
  sleep 1
done

docker exec codellama ollama run codellama

# The "ollama run codellama" command can slow down significantly towards the end of the run,
# potentially taking hours to fetch when nearing completion. To address this issue,
# you should terminate the process and rerun the command "docker exec codellama ollama run codellama". 
# Note that you will not lose any download progress, it does not guartee that the process will not slow down again.
# More details can be found at: https://github.com/ollama/ollama/issues/1736
