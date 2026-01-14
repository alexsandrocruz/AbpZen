import { WebSocketServer, WebSocket } from 'ws';
import { Server } from 'http';
import { parse } from 'url';

interface Client {
  ws: WebSocket;
  userId: string;
  workspaceId?: string;
}

class WebSocketService {
  private wss: WebSocketServer;
  private clients: Map<string, Client[]> = new Map();

  constructor(server: Server) {
    this.wss = new WebSocketServer({ server, path: '/ws' });
    
    this.wss.on('connection', (ws, req) => {
      const url = parse(req.url || '', true);
      const userId = url.query.userId as string;
      const workspaceId = url.query.workspaceId as string;

      if (!userId) {
        ws.close(1008, 'User ID required');
        return;
      }

      const client: Client = { ws, userId, workspaceId };
      this.addClient(userId, client);

      ws.on('message', (data) => {
        try {
          const message = JSON.parse(data.toString());
          this.handleMessage(client, message);
        } catch (e) {
          console.error('WebSocket message parse error:', e);
        }
      });

      ws.on('close', () => {
        this.removeClient(userId, ws);
      });

      ws.on('error', (error) => {
        console.error('WebSocket error:', error);
        this.removeClient(userId, ws);
      });

      ws.send(JSON.stringify({ type: 'connected', userId }));
    });
  }

  private addClient(userId: string, client: Client) {
    const userClients = this.clients.get(userId) || [];
    userClients.push(client);
    this.clients.set(userId, userClients);
  }

  private removeClient(userId: string, ws: WebSocket) {
    const userClients = this.clients.get(userId) || [];
    const filtered = userClients.filter(c => c.ws !== ws);
    if (filtered.length === 0) {
      this.clients.delete(userId);
    } else {
      this.clients.set(userId, filtered);
    }
  }

  private handleMessage(client: Client, message: any) {
    switch (message.type) {
      case 'join-workspace':
        client.workspaceId = message.workspaceId;
        break;
      case 'leave-workspace':
        client.workspaceId = undefined;
        break;
      case 'ping':
        client.ws.send(JSON.stringify({ type: 'pong' }));
        break;
    }
  }

  sendToUser(userId: string, data: any) {
    const userClients = this.clients.get(userId) || [];
    const message = JSON.stringify(data);
    userClients.forEach(client => {
      if (client.ws.readyState === WebSocket.OPEN) {
        client.ws.send(message);
      }
    });
  }

  sendToWorkspace(workspaceId: string, data: any) {
    const message = JSON.stringify(data);
    this.clients.forEach(userClients => {
      userClients.forEach(client => {
        if (client.workspaceId === workspaceId && client.ws.readyState === WebSocket.OPEN) {
          client.ws.send(message);
        }
      });
    });
  }

  broadcastNewMessage(workspaceId: string, conversationId: string, message: any) {
    this.sendToWorkspace(workspaceId, {
      type: 'new-message',
      conversationId,
      message,
    });
  }

  broadcastNewComment(workspaceId: string, entityType: string, entityId: string, comment: any) {
    this.sendToWorkspace(workspaceId, {
      type: 'new-comment',
      entityType,
      entityId,
      comment,
    });
  }

  sendNotification(userId: string, notification: any) {
    this.sendToUser(userId, {
      type: 'notification',
      notification,
    });
  }
}

let wsService: WebSocketService | null = null;

export function initWebSocket(server: Server): WebSocketService {
  wsService = new WebSocketService(server);
  return wsService;
}

export function getWebSocketService(): WebSocketService | null {
  return wsService;
}
