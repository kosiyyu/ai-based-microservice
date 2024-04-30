export type QuestionResponse = {
  question: string;
  response: string | null;
}

export function connPromise(address: string): Promise<WebSocket> {
  return new Promise((resolve, reject) => {
    const ws = new WebSocket(address);
    ws.onopen = () => {
      resolve(ws);
    }
    ws.onerror = (err) => {
      reject(err);
    }
  });
}

export function isWebSocket(obj: unknown): obj is WebSocket {
  return obj instanceof WebSocket;
}

export function checkConnection(ws: unknown): boolean {
  if (!isWebSocket(ws) || ws.readyState !== WebSocket.OPEN) {
    console.log("WebSocket not initialized or connection not open");
    return false;
  }
  return true;
}