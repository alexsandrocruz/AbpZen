import { useState, useRef, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import { ScrollArea } from "@/components/ui/scroll-area";
import { useToast } from "@/hooks/use-toast";
import { cn } from "@/lib/utils";
import {
  Bot,
  Plus,
  Send,
  Loader2,
  MessageSquare,
  Trash2,
  Sparkles,
  User,
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

interface AiSession {
  id: string;
  title: string;
  contextType: string | null;
  contextId: string | null;
  createdAt: string;
  updatedAt: string;
}

interface AiMessage {
  id: string;
  role: 'user' | 'assistant' | 'system';
  content: string;
  actionExecuted: string | null;
  tokensUsed: number | null;
  createdAt: string;
}

export default function SuperAiPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  const [selectedSessionId, setSelectedSessionId] = useState<string | null>(null);
  const [message, setMessage] = useState("");
  const [isSending, setIsSending] = useState(false);

  const { data: sessions = [], isLoading: sessionsLoading } = useQuery<AiSession[]>({
    queryKey: ["ai-sessions", currentWorkspace?.id],
    queryFn: () => api.getAiSessions(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: sessionData, isLoading: messagesLoading } = useQuery<{ session: AiSession; messages: AiMessage[] }>({
    queryKey: ["ai-session", selectedSessionId],
    queryFn: () => api.getAiSession(currentWorkspace!.id, selectedSessionId!),
    enabled: !!currentWorkspace && !!selectedSessionId,
  });

  const messages = sessionData?.messages || [];

  const createSessionMutation = useMutation({
    mutationFn: () => api.createAiSession(currentWorkspace!.id, { title: 'Nova conversa' }),
    onSuccess: (session) => {
      queryClient.invalidateQueries({ queryKey: ["ai-sessions"] });
      setSelectedSessionId(session.id);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteSessionMutation = useMutation({
    mutationFn: (sessionId: string) => api.deleteAiSession(currentWorkspace!.id, sessionId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ai-sessions"] });
      if (selectedSessionId) {
        setSelectedSessionId(null);
      }
      toast({ title: "Conversa excluída" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  useEffect(() => {
    if (messagesEndRef.current) {
      messagesEndRef.current.scrollIntoView({ behavior: "smooth" });
    }
  }, [messages]);

  useEffect(() => {
    if (selectedSessionId && inputRef.current) {
      inputRef.current.focus();
    }
  }, [selectedSessionId]);

  const handleSend = async () => {
    if (!message.trim() || !selectedSessionId || isSending) return;

    const currentMessage = message;
    setMessage("");
    setIsSending(true);

    try {
      await api.sendAiMessage(currentWorkspace!.id, selectedSessionId, currentMessage);
      queryClient.invalidateQueries({ queryKey: ["ai-session", selectedSessionId] });
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
      setMessage(currentMessage);
    } finally {
      setIsSending(false);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  if (!currentWorkspace) return null;

  return (
    <AppShell>
      <div className="h-[calc(100vh-8rem)] flex gap-4">
        <div className="w-72 flex flex-col border rounded-lg bg-card">
          <div className="p-3 border-b">
            <Button 
              className="w-full" 
              onClick={() => createSessionMutation.mutate()}
              disabled={createSessionMutation.isPending}
              data-testid="button-new-ai-session"
            >
              <Plus className="size-4 mr-2" />
              Nova Conversa
            </Button>
          </div>

          <ScrollArea className="flex-1 p-2">
            {sessionsLoading ? (
              <div className="space-y-2">
                {[1, 2, 3].map(i => (
                  <Skeleton key={i} className="h-16" />
                ))}
              </div>
            ) : sessions.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                <Bot className="size-8 mx-auto mb-2 opacity-50" />
                <p className="text-sm">Nenhuma conversa</p>
              </div>
            ) : (
              <div className="space-y-1">
                {sessions.map(session => (
                  <div
                    key={session.id}
                    className={cn(
                      "p-3 rounded-lg cursor-pointer transition-colors group",
                      selectedSessionId === session.id
                        ? "bg-primary/10 text-primary"
                        : "hover:bg-muted"
                    )}
                    onClick={() => setSelectedSessionId(session.id)}
                    data-testid={`ai-session-${session.id}`}
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex-1 min-w-0">
                        <p className="font-medium truncate text-sm">
                          {session.title}
                        </p>
                        <p className="text-xs text-muted-foreground">
                          {format(new Date(session.updatedAt), "dd/MM HH:mm", { locale: ptBR })}
                        </p>
                      </div>
                      <Button
                        variant="ghost"
                        size="icon"
                        className="opacity-0 group-hover:opacity-100 size-7"
                        onClick={(e) => {
                          e.stopPropagation();
                          deleteSessionMutation.mutate(session.id);
                        }}
                      >
                        <Trash2 className="size-3.5" />
                      </Button>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </ScrollArea>
        </div>

        <div className="flex-1 flex flex-col border rounded-lg bg-card">
          {!selectedSessionId ? (
            <div className="flex-1 flex flex-col items-center justify-center p-8">
              <div className="relative mb-6">
                <div className="size-20 rounded-full bg-gradient-to-br from-violet-500 to-indigo-600 flex items-center justify-center">
                  <Bot className="size-10 text-white" />
                </div>
                <Sparkles className="size-6 text-yellow-400 absolute -top-1 -right-1" />
              </div>
              <h2 className="text-2xl font-display font-bold mb-2">Super Work AI</h2>
              <p className="text-muted-foreground text-center max-w-md mb-6">
                Seu assistente inteligente para gerenciar o workspace. Pergunte sobre seus dados, crie tarefas, gere propostas e muito mais.
              </p>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-3 max-w-lg w-full">
                {[
                  "Quantos clientes ativos eu tenho?",
                  "Crie uma tarefa de follow-up",
                  "Quais projetos estão em andamento?",
                  "Gere uma descrição para proposta",
                ].map((suggestion, i) => (
                  <Button
                    key={i}
                    variant="outline"
                    className="justify-start text-left h-auto py-3 px-4"
                    onClick={() => {
                      createSessionMutation.mutate();
                      setMessage(suggestion);
                    }}
                  >
                    <MessageSquare className="size-4 mr-2 flex-shrink-0" />
                    <span className="truncate">{suggestion}</span>
                  </Button>
                ))}
              </div>
            </div>
          ) : (
            <>
              <div className="p-4 border-b">
                <h3 className="font-medium flex items-center gap-2">
                  <Bot className="size-5 text-primary" />
                  {sessionData?.session.title || 'Carregando...'}
                </h3>
              </div>

              <ScrollArea className="flex-1 p-4">
                {messagesLoading ? (
                  <div className="space-y-4">
                    {[1, 2].map(i => (
                      <Skeleton key={i} className="h-20" />
                    ))}
                  </div>
                ) : messages.length === 0 ? (
                  <div className="text-center py-12 text-muted-foreground">
                    <Bot className="size-12 mx-auto mb-3 opacity-30" />
                    <p>Envie uma mensagem para começar</p>
                  </div>
                ) : (
                  <div className="space-y-4">
                    {messages.map((msg) => (
                      <div
                        key={msg.id}
                        className={cn(
                          "flex gap-3",
                          msg.role === 'user' ? "justify-end" : "justify-start"
                        )}
                      >
                        {msg.role === 'assistant' && (
                          <div className="size-8 rounded-full bg-gradient-to-br from-violet-500 to-indigo-600 flex items-center justify-center flex-shrink-0">
                            <Bot className="size-4 text-white" />
                          </div>
                        )}
                        <div
                          className={cn(
                            "max-w-[80%] rounded-lg px-4 py-3",
                            msg.role === 'user'
                              ? "bg-primary text-primary-foreground"
                              : "bg-muted"
                          )}
                        >
                          <p className="whitespace-pre-wrap">{msg.content}</p>
                          {msg.actionExecuted && (
                            <div className="mt-2 pt-2 border-t border-current/10 text-xs opacity-70">
                              Ação executada
                            </div>
                          )}
                        </div>
                        {msg.role === 'user' && (
                          <div className="size-8 rounded-full bg-secondary flex items-center justify-center flex-shrink-0">
                            <User className="size-4" />
                          </div>
                        )}
                      </div>
                    ))}
                    {isSending && (
                      <div className="flex gap-3">
                        <div className="size-8 rounded-full bg-gradient-to-br from-violet-500 to-indigo-600 flex items-center justify-center">
                          <Bot className="size-4 text-white" />
                        </div>
                        <div className="bg-muted rounded-lg px-4 py-3">
                          <Loader2 className="size-5 animate-spin" />
                        </div>
                      </div>
                    )}
                    <div ref={messagesEndRef} />
                  </div>
                )}
              </ScrollArea>

              <div className="p-4 border-t">
                <div className="flex gap-2">
                  <Input
                    ref={inputRef}
                    value={message}
                    onChange={e => setMessage(e.target.value)}
                    onKeyDown={handleKeyDown}
                    placeholder="Digite sua mensagem..."
                    disabled={isSending}
                    className="flex-1"
                    data-testid="input-ai-message"
                  />
                  <Button 
                    onClick={handleSend} 
                    disabled={!message.trim() || isSending}
                    data-testid="button-send-ai-message"
                  >
                    {isSending ? (
                      <Loader2 className="size-4 animate-spin" />
                    ) : (
                      <Send className="size-4" />
                    )}
                  </Button>
                </div>
              </div>
            </>
          )}
        </div>
      </div>
    </AppShell>
  );
}
