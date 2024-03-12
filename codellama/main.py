from langchain_community.llms import Ollama
import requests
import time

### langchain
llm = Ollama(model="codellama")

def langchain():
  start = time.time()
  output = llm.invoke("Write simple python code:")
  end = time.time()
  print("langchain time: ", end - start)
  print("langchain output: ", output)

### api call
url = "http://localhost:11434/api/generate"
data = {
  "model": "codellama",
  "prompt": "Write simple python code:",
  "stream": False,
}

def api_call():
  start = time.time()
  with requests.post(url, json=data) as response:
    print(response.content)
    end = time.time()
    print("api_call time: ", end - start)
    print("api_call output: ", response.content)
    if not response.ok:
      print("api_call time: ", end - start)
      print(response)

### main
if __name__ == "__main__":
  langchain()
  api_call()