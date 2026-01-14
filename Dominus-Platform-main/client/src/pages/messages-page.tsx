import { useState, useRef, useEffect } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuth } from "@/lib/auth-store";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Skeleton } from "@/components/ui/skeleton";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { cn } from "@/lib/utils";
import { MessageSquare, Send, Plus, Users, ArrowLeft, Loader2 } from "lucide-react";
import { format, formatDistanceToNow, isToday, isYesterday } from "date-fns";
import { ptBR } from "date-fns/locale";

interface Conversation {
  id: string;
  name: string | null;
  type: "DIRECT" | "GROUP";
  lastMessageAt: string | null;
  lastMessage?: {
    content: string;
    senderId: string;
    senderName: string;
  };
  participants: {
    userId: string;
    user: {
      id: string;
      name: string;
      avatar?: string;
    };
  }[];
  unreadCount?: number;
}

interface Message {
  id: string;
  content: string;
  senderId: string;
  conversationId: string;
  createdAt: string;
  sender: {
    id: string;
    name: string;
    avatar?: string;
  };
}

const API_BASE = "/api";

async function handleResponse(response: Response) {
  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: "Request failed" }));
    throw new Error(error.message || `HTTP error! status: ${response.status}`);
  }
  return response.json();
}

const messagesApi = {
  getConversations: async (workspaceId: string): Promise<Conversation[]> => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/conversations`, {
      credentials: "include",
    });
    return handleResponse(res);
  },

  getConversation: async (workspaceId: string, conversationId: string): Promise<Conversation> => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/conversations/${conversationId}`, {
      credentials: "include",
    });
    return handleResponse(res);
  },

  getMessages: async (workspaceId: string, conversationId: string): Promise<Message[]> => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/conversations/${conversationId}/messages`, {
      credentials: "include",
    });
    return handleResponse(res);
  },

  sendMessage: async (workspaceId: string, conversationId: string, content: string): Promise<Message> => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/conversations/${conversationId}/messages`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({ content }),
    });
    return handleResponse(res);
  },

  startDirectConversation: async (workspaceId: string, userId: string): Promise<Conversation> => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/conversations/direct/${userId}`, {
      method: "POST",
      credentials: "include",
    });
    return handleResponse(res);
  },
};

function getInitials(name: string): string {
  return name
    .split(" ")
    .map((n) => n[0])
    .join("")
    .toUpperCase()
    .slice(0, 2);
}

function formatMessageTime(date: string): string {
  const d = new Date(date);
  if (isToday(d)) {
    return format(d, "HH:mm", { locale: ptBR });
  }
  if (isYesterday(d)) {
    return "Ontem";
  }
  return format(d, "dd/MM/yyyy", { locale: ptBR });
}

function formatFullMessageTime(date: string): string {
  const d = new Date(date);
  if (isToday(d)) {
    return format(d, "HH:mm", { locale: ptBR });
  }
  return format(d, "dd/MM/yyyy 'às' HH:mm", { locale: ptBR });
}

interface WorkspaceMember {
  userId: string;
  user: {
    id: string;
    name: string;
    email: string;
    avatar?: string;
  };
}

export default function MessagesPage() {
  const { user, currentWorkspace } = useAuth();
  const queryClient = useQueryClient();
  const [selectedConversationId, setSelectedConversationId] = useState<string | null>(null);
  const [messageInput, setMessageInput] = useState("");
  const [isMobileThreadView, setIsMobileThreadView] = useState(false);
  const [showNewConversationDialog, setShowNewConversationDialog] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const { data: conversations = [], isLoading: conversationsLoading } = useQuery({
    queryKey: ["conversations", currentWorkspace?.id],
    queryFn: () => messagesApi.getConversations(currentWorkspace!.id),
    enabled: !!currentWorkspace,
    refetchInterval: 30000,
  });

  const sortedConversations = [...conversations].sort((a, b) => {
    const dateA = a.lastMessageAt ? new Date(a.lastMessageAt).getTime() : 0;
    const dateB = b.lastMessageAt ? new Date(b.lastMessageAt).getTime() : 0;
    return dateB - dateA;
  });

  const { data: messages = [], isLoading: messagesLoading } = useQuery({
    queryKey: ["messages", currentWorkspace?.id, selectedConversationId],
    queryFn: () => messagesApi.getMessages(currentWorkspace!.id, selectedConversationId!),
    enabled: !!currentWorkspace && !!selectedConversationId,
    refetchInterval: 10000,
  });

  const selectedConversation = conversations.find((c) => c.id === selectedConversationId);

  const { data: workspaceMembers = [] } = useQuery<WorkspaceMember[]>({
    queryKey: ["workspaceMembers", currentWorkspace?.id],
    queryFn: async () => {
      const res = await fetch(`${API_BASE}/workspaces/${currentWorkspace!.id}/members`, {
        credentials: "include",
      });
      return handleResponse(res);
    },
    enabled: !!currentWorkspace && showNewConversationDialog,
  });

  const sendMessageMutation = useMutation({
    mutationFn: (content: string) =>
      messagesApi.sendMessage(currentWorkspace!.id, selectedConversationId!, content),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages", currentWorkspace?.id, selectedConversationId] });
      queryClient.invalidateQueries({ queryKey: ["conversations", currentWorkspace?.id] });
      setMessageInput("");
    },
  });

  const startConversationMutation = useMutation({
    mutationFn: (userId: string) => messagesApi.startDirectConversation(currentWorkspace!.id, userId),
    onSuccess: (conversation) => {
      queryClient.invalidateQueries({ queryKey: ["conversations", currentWorkspace?.id] });
      setSelectedConversationId(conversation.id);
      setShowNewConversationDialog(false);
      setIsMobileThreadView(true);
    },
  });

  const handleSendMessage = () => {
    if (!messageInput.trim() || !selectedConversationId) return;
    sendMessageMutation.mutate(messageInput.trim());
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSendMessage();
    }
  };

  const handleSelectConversation = (conversationId: string) => {
    setSelectedConversationId(conversationId);
    setIsMobileThreadView(true);
  };

  const handleBackToList = () => {
    setIsMobileThreadView(false);
  };

  useEffect(() => {
    if (messagesEndRef.current) {
      messagesEndRef.current.scrollIntoView({ behavior: "smooth" });
    }
  }, [messages]);

  const getConversationName = (conversation: Conversation): string => {
    if (conversation.name) return conversation.name;
    if (conversation.type === "DIRECT") {
      const otherParticipant = conversation.participants.find((p) => p.userId !== user?.id);
      return otherParticipant?.user.name || "Conversa";
    }
    return "Grupo";
  };

  const getConversationAvatar = (conversation: Conversation) => {
    if (conversation.type === "DIRECT") {
      const otherParticipant = conversation.participants.find((p) => p.userId !== user?.id);
      return otherParticipant?.user;
    }
    return null;
  };

  return (
    <AppShell>
      <div className="h-[calc(100vh-8rem)] flex flex-col">
        <div className="flex items-center justify-between mb-4">
          <h1 className="text-3xl font-display font-bold tracking-tight" data-testid="text-page-title">
            Mensagens
          </h1>
        </div>

        <div className="flex-1 flex rounded-xl border bg-card overflow-hidden shadow-sm">
          <div
            className={cn(
              "w-full md:w-80 lg:w-96 border-r flex flex-col",
              isMobileThreadView && "hidden md:flex"
            )}
          >
            <div className="p-4 border-b flex items-center justify-between">
              <h2 className="font-semibold text-lg" data-testid="text-conversations-header">Conversas</h2>
              <Button 
                size="sm" 
                variant="outline" 
                onClick={() => setShowNewConversationDialog(true)}
                data-testid="button-new-conversation"
              >
                <Plus className="size-4 mr-1" />
                Nova conversa
              </Button>
            </div>

            <ScrollArea className="flex-1">
              {conversationsLoading ? (
                <div className="p-4 space-y-3">
                  {[...Array(5)].map((_, i) => (
                    <div key={i} className="flex items-center gap-3">
                      <Skeleton className="size-12 rounded-full" />
                      <div className="flex-1">
                        <Skeleton className="h-4 w-24 mb-2" />
                        <Skeleton className="h-3 w-full" />
                      </div>
                    </div>
                  ))}
                </div>
              ) : sortedConversations.length === 0 ? (
                <div className="p-8 text-center text-muted-foreground" data-testid="text-empty-conversations">
                  <MessageSquare className="size-12 mx-auto mb-3 opacity-30" />
                  <p className="font-medium">Nenhuma conversa</p>
                  <p className="text-sm">Inicie uma conversa com um membro da equipe</p>
                </div>
              ) : (
                <div className="divide-y">
                  {sortedConversations.map((conversation) => {
                    const avatar = getConversationAvatar(conversation);
                    const isSelected = conversation.id === selectedConversationId;
                    
                    return (
                      <button
                        key={conversation.id}
                        onClick={() => handleSelectConversation(conversation.id)}
                        data-testid={`button-conversation-${conversation.id}`}
                        className={cn(
                          "w-full p-4 flex items-start gap-3 text-left hover:bg-accent/50 transition-colors",
                          isSelected && "bg-accent"
                        )}
                      >
                        <Avatar className="size-12">
                          {avatar?.avatar ? (
                            <AvatarImage src={avatar.avatar} />
                          ) : null}
                          <AvatarFallback>
                            {conversation.type === "GROUP" ? (
                              <Users className="size-5" />
                            ) : (
                              getInitials(getConversationName(conversation))
                            )}
                          </AvatarFallback>
                        </Avatar>
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center justify-between gap-2">
                            <span className="font-medium truncate" data-testid={`text-conversation-name-${conversation.id}`}>
                              {getConversationName(conversation)}
                            </span>
                            {conversation.lastMessageAt && (
                              <span className="text-xs text-muted-foreground shrink-0">
                                {formatMessageTime(conversation.lastMessageAt)}
                              </span>
                            )}
                          </div>
                          {conversation.lastMessage && (
                            <p className="text-sm text-muted-foreground truncate mt-0.5">
                              {conversation.lastMessage.senderId === user?.id
                                ? "Você: "
                                : `${conversation.lastMessage.senderName.split(" ")[0]}: `}
                              {conversation.lastMessage.content}
                            </p>
                          )}
                        </div>
                        {conversation.unreadCount && conversation.unreadCount > 0 && (
                          <span className="size-5 rounded-full bg-primary text-primary-foreground text-xs flex items-center justify-center">
                            {conversation.unreadCount}
                          </span>
                        )}
                      </button>
                    );
                  })}
                </div>
              )}
            </ScrollArea>
          </div>

          <div
            className={cn(
              "flex-1 flex flex-col",
              !isMobileThreadView && "hidden md:flex"
            )}
          >
            {!selectedConversationId ? (
              <div className="flex-1 flex items-center justify-center text-center p-8" data-testid="text-no-conversation-selected">
                <div>
                  <MessageSquare className="size-16 mx-auto mb-4 opacity-20" />
                  <h3 className="text-lg font-medium mb-1">Selecione uma conversa</h3>
                  <p className="text-muted-foreground text-sm">
                    Escolha uma conversa na lista ou inicie uma nova
                  </p>
                </div>
              </div>
            ) : (
              <>
                <div className="p-4 border-b flex items-center gap-3">
                  <Button
                    variant="ghost"
                    size="icon"
                    className="md:hidden"
                    onClick={handleBackToList}
                    data-testid="button-back-to-list"
                  >
                    <ArrowLeft className="size-5" />
                  </Button>
                  {selectedConversation && (
                    <>
                      <Avatar className="size-10">
                        {getConversationAvatar(selectedConversation)?.avatar ? (
                          <AvatarImage src={getConversationAvatar(selectedConversation)!.avatar} />
                        ) : null}
                        <AvatarFallback>
                          {selectedConversation.type === "GROUP" ? (
                            <Users className="size-4" />
                          ) : (
                            getInitials(getConversationName(selectedConversation))
                          )}
                        </AvatarFallback>
                      </Avatar>
                      <div>
                        <h3 className="font-semibold" data-testid="text-thread-title">
                          {getConversationName(selectedConversation)}
                        </h3>
                        {selectedConversation.type === "GROUP" && (
                          <p className="text-xs text-muted-foreground">
                            {selectedConversation.participants.length} participantes
                          </p>
                        )}
                      </div>
                    </>
                  )}
                </div>

                <ScrollArea className="flex-1 p-4">
                  {messagesLoading ? (
                    <div className="space-y-4">
                      {[...Array(5)].map((_, i) => (
                        <div key={i} className={cn("flex gap-3", i % 2 === 0 ? "" : "justify-end")}>
                          {i % 2 === 0 && <Skeleton className="size-8 rounded-full shrink-0" />}
                          <div className={cn("space-y-1", i % 2 !== 0 && "items-end")}>
                            <Skeleton className="h-4 w-20" />
                            <Skeleton className="h-16 w-48 rounded-lg" />
                          </div>
                        </div>
                      ))}
                    </div>
                  ) : messages.length === 0 ? (
                    <div className="h-full flex items-center justify-center text-center" data-testid="text-empty-messages">
                      <div>
                        <MessageSquare className="size-12 mx-auto mb-3 opacity-20" />
                        <p className="text-muted-foreground">Nenhuma mensagem ainda</p>
                        <p className="text-sm text-muted-foreground">Envie a primeira mensagem!</p>
                      </div>
                    </div>
                  ) : (
                    <div className="space-y-4">
                      {messages.map((message) => {
                        const isOwn = message.senderId === user?.id;
                        
                        return (
                          <div
                            key={message.id}
                            className={cn("flex gap-3", isOwn && "justify-end")}
                            data-testid={`message-${message.id}`}
                          >
                            {!isOwn && (
                              <Avatar className="size-8 shrink-0">
                                {message.sender.avatar ? (
                                  <AvatarImage src={message.sender.avatar} />
                                ) : null}
                                <AvatarFallback className="text-xs">
                                  {getInitials(message.sender.name)}
                                </AvatarFallback>
                              </Avatar>
                            )}
                            <div className={cn("max-w-[70%] space-y-1", isOwn && "items-end")}>
                              <div className={cn("flex items-center gap-2", isOwn && "justify-end")}>
                                <span className="text-xs font-medium">
                                  {isOwn ? "Você" : message.sender.name}
                                </span>
                                <span className="text-xs text-muted-foreground">
                                  {formatFullMessageTime(message.createdAt)}
                                </span>
                              </div>
                              <div
                                className={cn(
                                  "px-4 py-2.5 rounded-2xl",
                                  isOwn
                                    ? "bg-primary text-primary-foreground rounded-br-md"
                                    : "bg-muted rounded-bl-md"
                                )}
                              >
                                <p className="text-sm whitespace-pre-wrap break-words">{message.content}</p>
                              </div>
                            </div>
                            {isOwn && (
                              <Avatar className="size-8 shrink-0">
                                {user?.avatar ? (
                                  <AvatarImage src={user.avatar} />
                                ) : null}
                                <AvatarFallback className="text-xs">
                                  {getInitials(user?.name || "")}
                                </AvatarFallback>
                              </Avatar>
                            )}
                          </div>
                        );
                      })}
                      <div ref={messagesEndRef} />
                    </div>
                  )}
                </ScrollArea>

                <div className="p-4 border-t">
                  <div className="flex gap-2">
                    <Input
                      value={messageInput}
                      onChange={(e) => setMessageInput(e.target.value)}
                      onKeyDown={handleKeyDown}
                      placeholder="Digite sua mensagem..."
                      disabled={sendMessageMutation.isPending}
                      data-testid="input-message"
                      className="flex-1"
                    />
                    <Button
                      onClick={handleSendMessage}
                      disabled={!messageInput.trim() || sendMessageMutation.isPending}
                      data-testid="button-send-message"
                    >
                      <Send className="size-4" />
                    </Button>
                  </div>
                </div>
              </>
            )}
          </div>
        </div>
      </div>

      <Dialog open={showNewConversationDialog} onOpenChange={setShowNewConversationDialog}>
        <DialogContent className="max-w-md">
          <DialogHeader>
            <DialogTitle>Nova Conversa</DialogTitle>
          </DialogHeader>
          <div className="space-y-4">
            <p className="text-sm text-muted-foreground">
              Selecione um membro do workspace para iniciar uma conversa:
            </p>
            <ScrollArea className="h-64">
              <div className="space-y-2">
                {workspaceMembers
                  .filter((member) => member.userId !== user?.id)
                  .map((member) => (
                    <button
                      key={member.userId}
                      onClick={() => startConversationMutation.mutate(member.userId)}
                      disabled={startConversationMutation.isPending}
                      className="w-full flex items-center gap-3 p-3 rounded-lg hover:bg-muted transition-colors text-left disabled:opacity-50"
                      data-testid={`button-start-conversation-${member.userId}`}
                    >
                      <Avatar className="size-10">
                        {member.user.avatar ? (
                          <AvatarImage src={member.user.avatar} />
                        ) : null}
                        <AvatarFallback>{getInitials(member.user.name)}</AvatarFallback>
                      </Avatar>
                      <div className="flex-1 min-w-0">
                        <p className="font-medium truncate">{member.user.name}</p>
                        <p className="text-sm text-muted-foreground truncate">{member.user.email}</p>
                      </div>
                      {startConversationMutation.isPending && (
                        <Loader2 className="size-4 animate-spin" />
                      )}
                    </button>
                  ))}
                {workspaceMembers.filter((m) => m.userId !== user?.id).length === 0 && (
                  <div className="text-center py-8 text-muted-foreground">
                    <Users className="size-12 mx-auto mb-2 opacity-30" />
                    <p>Nenhum outro membro no workspace</p>
                  </div>
                )}
              </div>
            </ScrollArea>
          </div>
        </DialogContent>
      </Dialog>
    </AppShell>
  );
}
